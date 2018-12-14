using System;
using System.ComponentModel;
using System.Windows.Forms;
using NETCONLib;

namespace internetfacex
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int connDescription, int ReservedValue);

        public static bool IsConnectionAvailable()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.BeginInvoke(new Action(() =>
            {
                this.Hide();
                this.Opacity = 1;
            }));
            

            int nowhour = DateTime.Now.Hour;

            if (nowhour>= 18)
            {
                if (IsConnectionAvailable() == true)
                {
                    localnetface("off");
                }

            }
            timer1.Enabled = true;
            startToolStripMenuItem.Checked = true;

        }
        public void localnetface(string signer)
        {
            NetSharingManagerClass netSharingMgr = new NetSharingManagerClass();
            INetSharingEveryConnectionCollection connections = netSharingMgr.EnumEveryConnection;

            foreach (INetConnection connection in connections)
            {
                INetConnectionProps connProps = netSharingMgr.get_NetConnectionProps(connection);
                if (connProps.MediaType == tagNETCON_MEDIATYPE.NCM_LAN)
                {

                    if (signer == "on")
                    {
                        connection.Connect();    //启用网络

                    }
                    else if (signer == "off")
                    {
                        connection.Disconnect(); //禁用网络
                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (IsConnectionAvailable() == true)
            {
                localnetface("off");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (IsConnectionAvailable() == false)
            {
                localnetface("on");
            }

        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                //取消"关闭窗口"事件
                e.Cancel = true; // 取消关闭窗体 

                //使关闭时窗口向右下角缩小的效果
                this.WindowState = FormWindowState.Minimized;
                this.notifyIcon1.Visible = true;
                //this.m_cartoonForm.CartoonClose();
                this.Hide();
                return;
            }
          
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
            {
                this.WindowState = FormWindowState.Minimized;
                this.notifyIcon1.Visible = true;
                this.Hide();
            }
            else
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
           
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to quit?", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {

                this.notifyIcon1.Visible = false;
                this.Close();
                this.Dispose();
                System.Environment.Exit(System.Environment.ExitCode);

            }
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            //string nowtime = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second;
            //string plantime = "18:00:00";

            int nowhour = DateTime.Now.Hour;

            if (nowhour >= 18 || nowhour <=9)
            {
                if (IsConnectionAvailable() == true)
                {
                    localnetface("off");
                }
            }

           
        }

        private void onToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (IsConnectionAvailable() == false)
            {
                localnetface("on");
            }

        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (IsConnectionAvailable() == true)
            {
                localnetface("off");
            }

        }
        
        private void endToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startToolStripMenuItem.Checked = false;
            timer1.Enabled = false;
            (sender as ToolStripMenuItem).Checked = !(sender as ToolStripMenuItem).Checked;

        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            endToolStripMenuItem.Checked = false;
            timer1.Enabled = true;
            (sender as ToolStripMenuItem).Checked = !(sender as ToolStripMenuItem).Checked;   
             
        }
    }
}
