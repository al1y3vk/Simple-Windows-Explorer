﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExamWindowsForms
{
    public partial class Name : Form
    {
        public string DirName { get; private set; }

        public Name()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DirName = textBox1.Text;
            DialogResult = DialogResult.OK;
        }
    }
}