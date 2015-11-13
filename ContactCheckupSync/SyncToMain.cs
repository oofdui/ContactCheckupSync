using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _ContactCheckupSync
{
    public partial class SyncToMain : Form
    {
        #region GlobalVariable
        int syncTimerSecond = int.Parse(System.Configuration.ConfigurationManager.AppSettings["syncTimerSecond"]);
        int syncTimerSecondCount = 0;
        #endregion
        public SyncToMain()
        {
            InitializeComponent();
        }
        private void btStart_Click(object sender, EventArgs e)
        {
            tmDefault.Enabled = true;
            tmDefault.Start();
        }
        private void btStop_Click(object sender, EventArgs e)
        {
            bwDefault.CancelAsync();
            bwTimer.CancelAsync();
            tmDefault.Stop();
            tmDefault.Enabled = false;
            #region btStart
            if (btStart.InvokeRequired)
            {
                btStart.Invoke(new MethodInvoker(delegate
                {
                    btStart.Enabled = true;
                }));
            }
            else
            {
                btStart.Enabled = true;
            }
            #endregion
            #region btStop
            if (btStop.InvokeRequired)
            {
                btStop.Invoke(new MethodInvoker(delegate
                {
                    btStop.Enabled = false;
                }));
            }
            else
            {
                btStop.Enabled = false;
            }
            #endregion
            #region Animation
            if (anLoading.InvokeRequired)
            {
                anLoading.Invoke(new MethodInvoker(delegate
                {
                    anLoading.Visible = false;
                }));
            }
            else
            {
                anLoading.Visible = false;
            }
            #endregion
            #region Label
            if (lblDefault.InvokeRequired)
            {
                lblDefault.Invoke(new MethodInvoker(delegate
                {
                    lblDefault.Text = "- รอดำเนินการ -";
                }));
            }
            else
            {
                lblDefault.Text = "- รอดำเนินการ -";
            }
            #endregion
        }
        private void bwDefault_DoWork(object sender, DoWorkEventArgs e)
        {
            if (btStart.InvokeRequired)
            {
                btStart.Invoke(new MethodInvoker(delegate
                {
                    btStart.Enabled = false;
                }));
            }
            else
            {
                btStart.Enabled = false;
            }
            if (btStop.InvokeRequired)
            {
                btStop.Invoke(new MethodInvoker(delegate
                {
                    btStop.Enabled = true;
                }));
            }
            else
            {
                btStop.Enabled = true;
            }
            if (anLoading.InvokeRequired)
            {
                anLoading.Invoke(new MethodInvoker(delegate
                {
                    anLoading.Visible = true;
                }));
            }
            MessageBox.Show("Start");
        }
        private void tmDefault_Tick(object sender, EventArgs e)
        {
            if (syncTimerSecondCount>=syncTimerSecond)
            {
                syncTimerSecondCount = 0;
            }
            if (syncTimerSecondCount == 0)
            {
                bwDefault.RunWorkerAsync();
            }
            syncTimerSecondCount += 1;
            bwTimer.RunWorkerAsync();
        }

        private void bwTimer_DoWork(object sender, DoWorkEventArgs e)
        {
            #region Timer
            if (lblDefault.InvokeRequired)
            {
                lblDefault.Invoke(new MethodInvoker(delegate
                {
                    lblDefault.Text = string.Format("Countdown {0} / {1}", syncTimerSecondCount.ToString(), syncTimerSecond.ToString());
                }));
            }
            #endregion
        }
    }
}
