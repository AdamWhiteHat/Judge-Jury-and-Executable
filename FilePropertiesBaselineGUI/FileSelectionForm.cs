using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilePropertiesBaselineGUI
{
    public partial class FileSelectionForm : Form
    {
        public List<string> FileNames { get; set; }
        public string Filter { get; set; }
        public string InitialDirectory { get; set; }

        public FileSelectionForm()
        {
            InitializeComponent();
        }

        private void FileSelectionForm_Load(object sender, EventArgs e)
        {
            if (!FileNames.Any())
            {
                BrowseForFiles();
            }
        }

        private void FileSelectionForm_Shown(object sender, EventArgs e)
        {
            UpdateListbox();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            BrowseForFiles();
            UpdateListbox();
        }

        private void BrowseForFiles()
        {
            using (OpenFileDialog browseDialog = new OpenFileDialog())
            {
                browseDialog.InitialDirectory = InitialDirectory;
                browseDialog.Multiselect = true;
                browseDialog.Filter = Filter;
                if (browseDialog.ShowDialog() == DialogResult.OK)
                {
                    FileNames.AddRange(browseDialog.FileNames);
                }
            }
        }

        private void UpdateListbox()
        {
            listBoxFiles.Items.Clear();
            foreach (string file in FileNames)
            {
                listBoxFiles.Items.Add(Path.GetFileName(file));
            }
        }

        private void listBoxFiles_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                if (e.KeyCode == Keys.A)
                {
                    int index = 0;
                    int count = listBoxFiles.Items.Count;
                    while (index < count)
                    {
                        listBoxFiles.SetSelected(index, true);
                        index++;
                    }
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            else if (e.KeyCode == Keys.Delete)
            {
                if (listBoxFiles.SelectedItems.Count > 0)
                {
                    int lastIndex = listBoxFiles.SelectedIndices.Cast<int>().Last();
                    List<string> selectedFilenames = listBoxFiles.SelectedItems.Cast<string>().ToList();
                    var toRemove = FileNames.Where(s => selectedFilenames.Contains(Path.GetFileName(s)));
                    FileNames = FileNames.Except(toRemove).ToList();

                    UpdateListbox();

                    int newIndex = Math.Min(lastIndex, listBoxFiles.Items.Count - 1);
                    listBoxFiles.SelectedIndex = newIndex;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            FileNames.Clear();
            UpdateListbox();
        }
    }
}
