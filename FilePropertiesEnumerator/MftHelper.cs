using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.IO.Filesystem.Ntfs;

namespace FilePropertiesEnumerator
{
    using NtfsNodeAttributes = System.IO.Filesystem.Ntfs.Attributes;

    public static class MftHelper
    {
        public static IEnumerable<INode> EnumerateMft(DriveInfo driveToAnalyze)
        {
            NtfsReader ntfsReader = new NtfsReader(driveToAnalyze, RetrieveMode.All);

            IEnumerable<INode> nodes =
                ntfsReader.GetNodes(driveToAnalyze.Name)
                    .Where(n => (n.Attributes &
                                 (
                                    NtfsNodeAttributes.Device
                                  | NtfsNodeAttributes.Directory
                                  | NtfsNodeAttributes.ReparsePoint
                                  | NtfsNodeAttributes.SparseFile
                                 )
                                ) == 0); // This means that we DONT want any matches of the above NtfsNodeAttributes types.
                                         //.OrderByDescending(n => n.Size);

            return nodes;
        }
    }
}
