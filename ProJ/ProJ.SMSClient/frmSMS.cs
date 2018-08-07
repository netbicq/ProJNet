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
            SMSSend.SendHour = DateTime.Now.Hour+1;//运行时设置为晚于一小时发送
            SMSSend.SendLastTime = DateTime.Now.AddDays(-1);//设置为昨天，确何第一次运行的当天可以发送一次

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

               // int smstime = int.Parse(System.Configuration.ConfigurationManager.AppSettings["smstime"]);

                var dt = DateTime.Now;
                //非当天就发送
                bool send = Microsoft.VisualBasic.DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Day, dt, SMSSend.SendLastTime) != 0;
                ///如果当天未发送，且当前时间为发送时间
                if(send && dt.Hour ==SMSSend.SendHour)
                {
                    Task.Run(async () =>
                    {
                        await new SMSSend().SendSMS();
                    });
                    //设置最后发送时间为当天，确保当天只发送一次
                    SMSSend.SendLastTime = DateTime.Now;
                    //_smstimer.Stop();
                }

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(frmSMS), ex);
            }
        }
    }
}
