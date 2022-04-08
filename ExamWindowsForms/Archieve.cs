using System;
using System.Windows.Forms;
using System.IO.Compression;

namespace ExamWindowsForms
{
    public partial class Archieve : Form
    {
        public string Path { get { return textBox1.Text; } set { textBox1.Text = value; } }
        public bool Tick { get { return checkBox1.Checked; } set { checkBox1.Checked = value; } }
        public CompressionLevel CompressionLevel { get { return (CompressionLevel)comboBox1.SelectedItem; } set { comboBox1.SelectedItem = value; } }

        public Archieve()
        {
            InitializeComponent();

            CompressionLevel[] level = new[] { CompressionLevel.Fastest, CompressionLevel.Optimal, CompressionLevel.NoCompression };
            comboBox1.DataSource = level;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.ShowDialog();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;   
        }
    }
}