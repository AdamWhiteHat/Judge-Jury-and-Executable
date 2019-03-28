using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FilePropertiesBaselineGUI
{
	public static class DialogHelper
	{
		public static string BrowseForFolderDialog(string initialDirectory = "")
		{
			using (FolderBrowserDialog browseDialog = new FolderBrowserDialog())
			{
				browseDialog.SelectedPath = initialDirectory;
				//browseDialog.RootFolder = Environment.SpecialFolder.ProgramFilesX86;
				if (browseDialog.ShowDialog() == DialogResult.OK)
				{
					return browseDialog.SelectedPath;
				}
			}

			return string.Empty;
		}

		public static string BrowseForFileDialog(string initialDirectory = "")
		{
			using (OpenFileDialog browseDialog = new OpenFileDialog())
			{
				browseDialog.InitialDirectory = initialDirectory;
				//browseDialog.RootFolder = Environment.SpecialFolder.ProgramFilesX86;
				if (browseDialog.ShowDialog() == DialogResult.OK)
				{
					return browseDialog.FileName;
				}
			}

			return string.Empty;
		}
	}
}
