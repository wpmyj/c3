﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Xdgk.GR.UI;
using Xdgk.Common;
using RemoteClient;

namespace RemoteClientWindows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RemoteController c = new RemoteController();
            frmXD100ModbusTemperatureControl f = new frmXD100ModbusTemperatureControl(c);
            f.ShowDialog();
        }
    }
}