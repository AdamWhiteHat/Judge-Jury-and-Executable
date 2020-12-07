using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;

namespace FilePropertiesBaselineGUI
{
	public static class DialogHelper
	{
		public static class Filters
		{
			public static string AllFiles = "All files (*.*)|*.*";
			public static string CsvFiles = "csv files (*.csv)|*.csv|" + AllFiles;
			public static string JsonFiles = "JSON files (*.json)|*.json|" + AllFiles;
			public static string YaraFiles = "YARA files (*.yar)|*.yar|" + AllFiles;
			public static string SqliteFiles = "Sqlite files (*.sqlite;*.db;*.db3)|*.sqlite;*.db;*.db3|" + AllFiles;
		}

		private static string _lastDirectory = "";

		public static string BrowseForFolderDialog(string initialDirectory = default(string))
		{
			string result = string.Empty;

			using (FolderBrowserDialog browseFolderDialog = new FolderBrowserDialog())
			{
				browseFolderDialog.SelectedPath = (!string.IsNullOrWhiteSpace(initialDirectory)) ? initialDirectory : _lastDirectory;

				if (browseFolderDialog.ShowDialog() == DialogResult.OK)
				{
					result = browseFolderDialog.SelectedPath;
					_lastDirectory = Path.GetDirectoryName(result);
				}
			}

			return result;
		}

		public static string BrowseForFileDialog(string filter = default(string), string initialDirectory = default(string))
		{
			string result = string.Empty;

			using (OpenFileDialog browseFileDialog = new OpenFileDialog())
			{
				browseFileDialog.Filter = (!string.IsNullOrWhiteSpace(filter)) ? filter : Filters.AllFiles;
				browseFileDialog.InitialDirectory = (!string.IsNullOrWhiteSpace(initialDirectory)) ? initialDirectory : _lastDirectory;

				if (browseFileDialog.ShowDialog() == DialogResult.OK)
				{
					result = browseFileDialog.FileName;
					_lastDirectory = Path.GetDirectoryName(result);
				}
			}

			return result;
		}

		public static string SaveFileDialog(string filter = default(string), string initialDirectory = default(string))
		{
			string result = string.Empty;

			using (SaveFileDialog saveDialog = new SaveFileDialog())
			{
				saveDialog.Filter = (!string.IsNullOrWhiteSpace(filter)) ? filter : Filters.AllFiles;
				saveDialog.InitialDirectory = (!string.IsNullOrWhiteSpace(initialDirectory)) ? initialDirectory : _lastDirectory;

				if (saveDialog.ShowDialog() == DialogResult.OK)
				{
					result = saveDialog.FileName;
					_lastDirectory = Path.GetDirectoryName(result);
				}
			}

			return result;
		}

		public static string[] BrowseForFilesDialog(string filter = default(string), string initialDirectory = default(string))
		{
			string[] results = new string[0];

			using (OpenFileDialog browseFilesDialog = new OpenFileDialog())
			{
				browseFilesDialog.Multiselect = true;
				browseFilesDialog.Filter = (!string.IsNullOrWhiteSpace(filter)) ? filter : Filters.AllFiles;
				browseFilesDialog.InitialDirectory = (!string.IsNullOrWhiteSpace(initialDirectory)) ? initialDirectory : _lastDirectory;

				if (browseFilesDialog.ShowDialog() == DialogResult.OK)
				{
					results = browseFilesDialog.FileNames;
					_lastDirectory = Path.GetDirectoryName(browseFilesDialog.FileName);
				}
			}

			return results;
		}
	}
}
