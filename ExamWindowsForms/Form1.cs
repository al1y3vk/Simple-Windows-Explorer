using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Compression;
using System.Collections.Generic;

namespace ExamWindowsForms
{
    public partial class Form1 : Form
    {
        private Stack<string> Paths = new Stack<string>();

        public Form1()
        {
            InitializeComponent();

            listView1.ColumnClick += new ColumnClickEventHandler(ClickOnColumn);
            ColumnHeader CH1 = new ColumnHeader();
            CH1.Text = "Name"; CH1.Width += 100;
            ColumnHeader CH2 = new ColumnHeader();
            CH2.Text = "Size"; CH2.Width += 60;
            ColumnHeader CH3 = new ColumnHeader();
            CH3.Text = "Type"; CH3.Width += 60;
            ColumnHeader CH4 = new ColumnHeader();
            CH4.Text = "Changed"; CH4.Width += 60;

            listView1.Columns.Add(CH1);
            listView1.Columns.Add(CH2);
            listView1.Columns.Add(CH3);
            listView1.Columns.Add(CH4);

            listView1.LargeImageList = listView1.SmallImageList = new ImageList();
            listView1.LargeImageList.Images.Add(Properties.Resources.Folder);
            DirectoryInfo dir = new DirectoryInfo("C:/");

            Fill(dir);
        }

        private void Fill (DirectoryInfo dir)
        {
            try
            {
                Paths.Push(dir.FullName);
                listView1.Items.Clear();
                foreach (DirectoryInfo di in dir.GetDirectories())
                {
                    ListViewItem LVI = new ListViewItem();
                    LVI.Name = di.Name;
                    LVI.Text = di.Name;
                    LVI.Tag = di;
                    LVI.ImageIndex = 0;
                    LVI.SubItems.Add("");
                    LVI.SubItems.Add("Folder");
                    LVI.SubItems.Add(di.LastWriteTime.ToString());
                    listView1.Items.Add(LVI);
                }

                foreach (FileInfo fi in dir.GetFiles())
                {
                    ListViewItem LVI2 = new ListViewItem();

                    if (!listView1.LargeImageList.Images.ContainsKey($"{fi.Extension}"))
                    {
                        try
                        {
                            Icon I = Icon.ExtractAssociatedIcon(fi.FullName);
                            listView1.LargeImageList.Images.Add($"{fi.Extension}", I);
                        }
                        catch (Exception) { }
                    }

                    LVI2.ImageIndex = listView1.LargeImageList.Images.Keys.IndexOf($"{fi.Extension}");
                    LVI2.Name = fi.Name;
                    LVI2.Text = fi.Name;
                    LVI2.Tag = fi;
                    LVI2.SubItems.Add(fi.Length.ToString() + " Bytes");
                    LVI2.SubItems.Add("File");
                    LVI2.SubItems.Add(fi.LastWriteTime.ToString());
                    listView1.Items.Add(LVI2);
                }
                toolStripTextBox2.Text = dir.FullName;
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("This Folder is Inaccessible!");
                Fill(dir.Parent);
            }
        }

        private void ClickOnColumn(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == 0)
            {
                if (listView1.Sorting == SortOrder.Descending)
                {
                    listView1.Sorting = SortOrder.Ascending;
                }
                else { listView1.Sorting = SortOrder.Descending; }
            }
        }

        private void ıconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.SmallIcon;
        }
        private void ımageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.LargeIcon;
        }
        private void tileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.Tile;
        }
        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.List;
        }
        private void chartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.View = View.Details;
        }
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fill(new DirectoryInfo(Paths.Pop()));
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listView1.SelectedItems[0].Tag is DirectoryInfo)
            {
                DirectoryInfo selectedDirectory = listView1.SelectedItems[0].Tag as DirectoryInfo;
                Fill(selectedDirectory);
            }

            else
            {
                FileInfo fi = listView1.SelectedItems[0].Tag as FileInfo;
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.FileName = fi.FullName;
                process.Start();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (Paths.Count >= 2)
            {
                Paths.Pop();
                Fill(new DirectoryInfo(Paths.Pop()));
            }
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Fill(new DirectoryInfo(path));
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Fill(new DirectoryInfo(path));
        }
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo("C:/");
            Fill(dir);
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo("F:/");
            Fill(dir);
        }

        private void createFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Name name = new Name();

            if (name.ShowDialog() == DialogResult.OK)
            {
                string path = Paths.Peek() + "\\" + name.DirName;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                else
                {
                    MessageBox.Show("Folder with that name already exsists!");
                }
            }

            Fill(new DirectoryInfo(Paths.Pop()));
        }
        private void deleteFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedItems[0].Tag is DirectoryInfo)
            {
                string Path = (listView1.SelectedItems[0].Tag as DirectoryInfo).FullName;
                Directory.Delete(Path, true);
            }

            else if (listView1.SelectedItems[0].Tag is FileInfo)
            {
                string Path = (listView1.SelectedItems[0].Tag as FileInfo).FullName;
                File.Delete(Path);
            }

            Fill(new DirectoryInfo(Paths.Pop()));
        }
        private void addToArchieveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Archieve archieve = new Archieve();
            DirectoryInfo directory =  listView1.SelectedItems[0].Tag as DirectoryInfo;
            string path = Paths.Peek() + "\\" + directory.Name + ".zip";
            archieve.Path = path;

            if (archieve.ShowDialog() == DialogResult.OK)
            {
                ZipFile.CreateFromDirectory(directory.FullName, archieve.Path, archieve.CompressionLevel, archieve.Tick);
            }

            Fill(new DirectoryInfo(Paths.Pop()));
            
        }
        private void extractHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Archieve archieve = new Archieve();
            FileInfo file = listView1.SelectedItems[0].Tag as FileInfo;
            string path = Paths.Peek() + "\\" + Path.GetFileNameWithoutExtension(file.Name);

            ZipFile.ExtractToDirectory(file.FullName, path);
        }
        private void extractToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
        }
        private void showContainingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileInfo file = (FileInfo)listView1.SelectedItems[0].Tag;
            ZipArchive zip = new ZipArchive(File.OpenRead(file.FullName), ZipArchiveMode.Read);
            string Result = "";
            foreach(ZipArchiveEntry entry in zip.Entries)
            {
                Result += "+ " + entry.Name + "\n";
            }
            MessageBox.Show(Result);
        }
    }
}