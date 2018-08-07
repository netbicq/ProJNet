using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProJ.SMSClient
{
    public partial class frmSMS : Form
    {
        Timer _smstimer = null;

        public frmSMS()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LogHelper.WriteLog(typeof(frmSMS), "SMS Service Start");
            _smstimer = new Timer();
            _smstimer.Interval = 2000;
            _smstimer.Tick += _smstimer_Tick;
            _smstimer.Start();
        }

        /// <summary>
        /// 定期检查
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _smstimer_Tick(object sender, EventArgs e)
        {
            try
            {

                int smstime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["smstime"]);

                var dt = DateTime.Now;

                Task.Run(async () =>
                {
                    await new SMSSend().SendSMS();
                });
                //_smstimer.Stop();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(frmSMS), ex);
            }
        }
    }
}
