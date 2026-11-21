using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using FilePropertiesDataObject;
using FilePropertiesDataObject.Parameters;
using System.IO.Filesystem.Ntfs;

namespace FilePropertiesEnumerator
{
    using NtfsNodeAttributes = System.IO.Filesystem.Ntfs.Attributes;

    public class FileEnumerator
    {
        private static string[] IgnoreTheseRootFiles = Properties.Settings.Default.IgnoreTheseRootFiles.Cast<string>().ToArray();

        private static string[] IgnoreTheseRootDirectories = Properties.Settings.Default.IgnoreTheseRootDirectories.Cast<string>().ToArray();

        private static NtfsNodeAttributes ExcludeAttributes = (NtfsNodeAttributes.Device | NtfsNodeAttributes.Directory | NtfsNodeAttributes.ReparsePoint | NtfsNodeAttributes.SparseFile);

        private static char[] DirectorySeperatorChars = new char[]
        {
            Path.DirectorySeparatorChar,
            Path.AltDirectorySeparatorChar
        };

        public static void LaunchFileEnumerator(FileEnumeratorParameters parameters)
        {
            TimingMetrics.Verbosity = parameters.LoggingVerbosity;

            var fileEnumerationDelegate
                = new Func<FileEnumeratorParameters, FileEnumeratorReport>((args) => Worker(args));

            if (!parameters.DisableWorkerThread)
            {
                Task<FileEnumeratorReport> enumerationTask = Task.Run(() =>
                {
                    FileEnumeratorReport results = fileEnumerationDelegate.Invoke(parameters);
                    return results;
                },
                parameters.CancelToken);

                // Always report back to the caller, even if the enumeration task faulted or was cancelled.
                // Accessing antecedent.Result on a faulted/cancelled task throws, which would skip
                // ReportResultsFunction and leave the UI stuck in its "processing" state.
                Task reportResultsTask = enumerationTask.ContinueWith((antecedent) =>
                {
                    FileEnumeratorReport report;
                    if (antecedent.Status == TaskStatus.RanToCompletion)
                    {
                        report = antecedent.Result;
                    }
                    else
                    {
                        if (antecedent.IsFaulted)
                        {
                            parameters.ReportExceptionFunction.Invoke(nameof(LaunchFileEnumerator), string.Empty, antecedent.Exception.GetBaseException());
                        }
                        report = new FileEnumeratorReport(new List<FailSuccessCount>());
                    }
                    parameters.ReportResultsFunction(report);
                }, TaskScheduler.Default);
            }
            else
            {
                FileEnumeratorReport results = fileEnumerationDelegate.Invoke(parameters);
                parameters.ReportResultsFunction(results);
            }
        }

        private static FileEnumeratorReport Worker(FileEnumeratorParameters parameters)
        {
            TimingMetrics.ResetMetrics();
            FailSuccessCount fileEnumCount = new FailSuccessCount("OS files enumerated");
            FailSuccessCount databaseInsertCount = new FailSuccessCount("OS database rows updated");
            IDataPersistenceLayer dataPersistenceLayer = parameters.DataPersistenceLayer;

            try
            {
                parameters.CancelToken.ThrowIfCancellationRequested();

                StringBuilder currentPath = new StringBuilder(parameters.SelectedFolder);
                string lastParent = currentPath.ToString();

                string temp = currentPath.ToString();
                if (temp.Contains(':') && (temp.Length == 2 || temp.Length == 3)) // Is a root directory, i.e. "C:" or "C:\"
                {
                    lastParent = ".";
                }

                string drive = parameters.SelectedFolder[0].ToString().ToUpper();

                List<DriveInfo> ntfsDrives = null;
                DriveInfo driveToAnalyze = null;
                NtfsReader ntfsReader = null;
                IEnumerable<INode> mftNodes = null;

                using (var timer = new TimingMetrics(TimingMetric.ParsingMFT))
                {
                    ntfsDrives = DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveFormat == "NTFS").ToList();

                    driveToAnalyze = ntfsDrives.Where(dr => dr.Name.ToUpper().Contains(drive)).Single();

                    if (driveToAnalyze.DriveFormat != "NTFS")
                    {
                        throw new BadImageFormatException("Drive not formatted as NTFS.");
                    }
                    if (!driveToAnalyze.IsReady)
                    {
                        throw new DriveNotFoundException("Drive not ready.");
                    }

                    ntfsReader = new NtfsReader(driveToAnalyze, RetrieveMode.All);

                    mftNodes = ntfsReader.GetNodes(driveToAnalyze.Name)
                                        .Where(node => (node.Attributes & ExcludeAttributes) == 0) // ExcludeAttributes = Device | Directory | ReparsePoint | SparseFile
                                        .Where(node => FileMatchesPattern(node.FullName, parameters.SearchPatterns));
                    //.OrderByDescending(n => n.Size);

                    if (parameters.SelectedFolder.ToCharArray().Length > 3)
                    {
                        string selectedFolderUppercase = parameters.SelectedFolder.ToUpperInvariant().TrimEnd(new char[] { '\\' });
                        mftNodes = mftNodes.Where(node => node.FullName.ToUpperInvariant().Contains(selectedFolderUppercase));
                    }
                }

                foreach (INode node in mftNodes)
                {
                    string message = $"MFT#: {node.MFTRecordNumber.ToString().PadRight(7)} Seq.#: {node.SequenceNumber.ToString().PadRight(4)} Path: {node.FullName}";
                    parameters.ReportAndLogOutputFunction.Invoke(message);

                    FileProperties prop = new FileProperties();
                    try
                    {
                        prop.PopulateFileProperties(parameters, parameters.SelectedFolder[0], node);
                    }
                    catch (OperationCanceledException)
                    {
                        throw;
                    }
                    catch (Exception ex)
                    {
                        // A single unreadable file (e.g. a bad sector reported by the drive) must not abort the whole scan.
                        parameters.ReportExceptionFunction.Invoke($"{nameof(Worker)}: {node.FullName}", string.Empty, ex);
                        fileEnumCount.IncrementFailedCount();
                        parameters.CancelToken.ThrowIfCancellationRequested();
                        continue; // Don't persist a partially populated record.
                    }

                    fileEnumCount.IncrementSucceededCount();

                    // INSERT file properties into _DATABASE_
                    using (var timer = new TimingMetrics(TimingMetric.PersistingFileProperties))
                    {
                        bool insertResult = dataPersistenceLayer.PersistFileProperties(prop);
                        if (insertResult)
                        {
                            databaseInsertCount.IncrementSucceededCount();
                        }
                        else
                        {
                            databaseInsertCount.IncrementFailedCount();
                        }
                    }

                    parameters.CancelToken.ThrowIfCancellationRequested();
                }
            }
            catch (OperationCanceledException)
            { }
            catch (Exception ex)
            {
                // Unhandled exceptions used to fault the Task silently, leaving the UI hung.
                parameters.ReportExceptionFunction.Invoke(nameof(Worker), string.Empty, ex);
            }
            finally
            {
                try
                {
                    dataPersistenceLayer?.Dispose();
                    FileProperties.CleanUp();
                }
                catch (Exception cleanupException)
                {
                    parameters.ReportExceptionFunction.Invoke($"{nameof(Worker)}: cleanup", string.Empty, cleanupException);
                }
            }

            return new FileEnumeratorReport(new List<FailSuccessCount> { fileEnumCount, databaseInsertCount });
        }

        private static bool FileMatchesPattern(string fullName, string[] searchPatterns)
        {
            string filename = Path.GetFileName(fullName);
            string rootDirectory = Path.GetDirectoryName(fullName).Substring(3);

            int seperatorIndex = rootDirectory.IndexOfAny(DirectorySeperatorChars);
            if (seperatorIndex != -1)
            {
                rootDirectory = rootDirectory.Substring(0, seperatorIndex);
            }

            /* if (filename.FirstOrDefault() == '$') { return false; } */

            if (string.IsNullOrWhiteSpace(rootDirectory))
            {
                if (IgnoreTheseRootFiles.Contains(filename))
                {
                    return false;
                }
            }
            else
            {
                if (IgnoreTheseRootDirectories.Any(dir => rootDirectory.Equals(dir)))
                {
                    return false;
                }
            }

            if (searchPatterns.Contains("*"))
            {
                return true;
            }

            string extension = Path.GetExtension(filename);
            if (extension.Length == 0)
            {
                return false;
            }

            foreach (string pattern in searchPatterns)
            {
                if (pattern.Contains("."))
                {
                    if (extension.Contains(pattern))
                    {
                        return true;
                    }
                }
                else
                {
                    if (filename.Contains(pattern))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
