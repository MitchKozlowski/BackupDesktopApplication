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

namespace BackupApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listBoxFiles.AllowDrop = true;
            listBoxFiles.DragEnter += ListBoxFiles_DragEnter;
            listBoxFiles.DragDrop += ListBoxFiles_DragDrop;
            buttonBackup.Click += ButtonBackup_Click;
            buttonSelectDir.Click += ButtonSelectDir_Click; // Optional for selecting directory
            buttonRemove.Click += ButtonRemove_Click; // Handle the remove button click event
        }

        private void ListBoxFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void ListBoxFiles_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    listBoxFiles.Items.Add(file);
                }
            }
        }

        private void ButtonBackup_Click(object sender, EventArgs e)
        {
            string backupDir = textBoxBackupDir.Text;

            if (string.IsNullOrWhiteSpace(backupDir) || !Directory.Exists(backupDir))
            {
                MessageBox.Show("Please select a valid backup directory.");
                return;
            }

            foreach (string filePath in listBoxFiles.Items)
            {
                string fileName = Path.GetFileName(filePath);
                string destPath = Path.Combine(backupDir, fileName);

                try
                {
                    File.Copy(filePath, destPath, true);
                    MessageBox.Show($"File '{fileName}' backed up successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error backing up file '{fileName}': {ex.Message}");
                }
            }
        }

        private void ButtonSelectDir_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBoxBackupDir.Text = fbd.SelectedPath;
                }
            }
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            // Remove selected items from the listBox
            var selectedItems = listBoxFiles.SelectedItems.OfType<string>().ToList();
            foreach (var item in selectedItems)
            {
                listBoxFiles.Items.Remove(item);
            }
        }
    }
}