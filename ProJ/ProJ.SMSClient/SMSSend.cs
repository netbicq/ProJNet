using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

 
namespace ProJ.SMSClient
{

    public class SMSSend
    {
        /// <summary>
        /// 最后发送日期
        /// </summary>
        public static DateTime SendLastTime { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public static int SendHour { get; set; }

        public async Task SendSMS()
        {
            try
            {
               
                var dbcontext = new ORM.dbcontext();
                ORM.IUnitwork _work = new ORM.Unitwork(dbcontext);

                var smscontent = new Bll.ProJService(_work).GetProjectSMS();
                if(smscontent.state ==200)
                {
                    HttpClient http = new HttpClient();
                    var url = System.Configuration.ConfigurationManager.AppSettings["smsurl"];
                    foreach (var sms in smscontent.data)
                    {
                        if (string.IsNullOrEmpty(sms.ProjectContact.HandlerTEL))
                        {
                            sms.ProjectContact.HandlerTEL = sms.OwerInfo.HandlerTEL;
                        }
                        if (string.IsNullOrEmpty(sms.ProjectContact.LeaderTEL))
                        {
                            sms.ProjectContact.LeaderTEL = sms.OwerInfo.HandlerTEL;
                        }
                        if (string.IsNullOrEmpty(sms.ProjectContact.PrincipalTEL))
                        {
                            sms.ProjectContact.PrincipalTEL = sms.OwerInfo.HandlerTEL;
                        }
                        if (string.IsNullOrEmpty(sms.ProjectContact.SiteLinkTEL))
                        {
                            sms.ProjectContact.SiteLinkTEL = sms.OwerInfo.HandlerTEL;
                        }
                        if (string.IsNullOrEmpty(sms.ProjectContact.SitePrincipalTEL))
                        {
                            sms.ProjectContact.SitePrincipalTEL = sms.OwerInfo.HandlerTEL;
                        }
                        if (sms.Timeouts.Count()>0)
                        {
                            StringContent para;
                            //节点1周
                            var points = "";
                            //节点2周
                            var points2 = "";
                            foreach (var tsms in sms.Timeouts.Where(q=>q.WeekInt>=1))
                            {
                                Model.DB.Project_SMS dbsms;

                                switch (tsms.WeekInt)
                                {
                                    case 1:
                                        points = points + "_" + tsms.PointName;
                                        dbsms = new Model.DB.Project_SMS
                                        {
                                            ID = Guid.NewGuid(),
                                            PointName = tsms.PointName,
                                            SendDate = DateTime.Now,
                                            WeekInt = 1, ProjectID =sms.ProjectInfo.ID
                                        };
                                        break;
                                    case 2:
                                        points2 = points2 + "_" + tsms.PointName;
                                        dbsms = new Model.DB.Project_SMS
                                        {
                                            ID = Guid.NewGuid(),
                                            PointName = tsms.PointName,
                                            SendDate = DateTime.Now,
                                            WeekInt = 2,ProjectID =sms.ProjectInfo.ID
                                        };
                                        break; 
                                    default:
                                        points2 = points2 + "_" + tsms.PointName;
                                        dbsms = new Model.DB.Project_SMS
                                        {
                                            ID = Guid.NewGuid(),
                                            PointName = tsms.PointName,
                                            SendDate = DateTime.Now,
                                            WeekInt = 2,ProjectID =sms.ProjectInfo.ID
                                        };
                                        break;
                                }
                                var db = _work.Repository<Model.DB.Project_SMS>();
                                if(!db.Any(q=>q.ProjectID == sms.ProjectInfo.ID && q.PointName == tsms.PointName && q.WeekInt == tsms.WeekInt))
                                {
                                    db.Add(dbsms);
                                    _work.Commit();
                                }
                            }
                            if (points!="")
                            {
                                //发送1周
                                SMSPacket jj = new SMSPacket();
                                List<string> ss = new List<string>();
                                ss.Add(sms.ProjectInfo.ProjectName);
                                ss.Add(points);
                                jj.mobile = sms.ProjectContact.HandlerTEL + "," + sms.ProjectContact.PrincipalTEL + "," + sms.ProjectContact.SiteLinkTEL;
                                jj.templateno = System.Configuration.ConfigurationManager.AppSettings["smsmu1"];
                                jj.variables = ss;
                                SendTelMSG(jj);
                                //var smspara = new SMSPara()
                                //{
                                //    mobile = "15523213827",
                                //    //mobile = sms.ProjectContact.HandlerTEL+","+ sms.ProjectContact.PrincipalTEL + "," + sms.ProjectContact.SiteLinkTEL,
                                //    tpl_id = int.Parse(System.Configuration.ConfigurationManager.AppSettings["smsw1id"]),
                                //    tpl_value = System.Web.HttpUtility.UrlEncode($"#projectname#={sms.ProjectInfo.ProjectName}&#pointname#={points}")
                                //};
                                ////var smsstr = Newtonsoft.Json.JsonConvert.SerializeObject(smspara);
                                ////para = new StringContent(smsstr, System.Text.Encoding.UTF8);
                                ////await http.PostAsync(url, para);
                                //await http.GetAsync(url + "?" + "mobile=" + smspara.mobile + "&tpl_id=" + smspara.tpl_id + "&tpl_value=" + smspara.tpl_value + "&key=" + smspara.key);
                            }
                            if (points2!="")
                            {
                                //发送2周
                                SMSPacket jj = new SMSPacket();
                                List<string> ss = new List<string>();
                                ss.Add(sms.ProjectInfo.ProjectName);
                                ss.Add(points);
                                jj.mobile = sms.ProjectContact.SitePrincipalTEL + "," + sms.ProjectContact.PrincipalTEL + "," + sms.ProjectContact.SiteLinkTEL + "," + sms.ProjectContact.LeaderTEL
                                + "," + sms.ProjectContact.DeptPrincipalTEL + "," + sms.ProjectContact.ComPrincipalTEL + "," + sms.ProjectContact.ComLeadTEL;
                                jj.templateno = System.Configuration.ConfigurationManager.AppSettings["smsmu2"];
                                jj.variables = ss;
                                SendTelMSG(jj);
                                //var smspara = new SMSPara()
                                //{
                                //    mobile= "15523213827",
                                //    //mobile = sms.ProjectContact.SitePrincipalTEL + "," + sms.ProjectContact.PrincipalTEL + "," + sms.ProjectContact.SiteLinkTEL + "," + sms.ProjectContact.LeaderTEL
                                //    //+ "," + sms.ProjectContact.DeptPrincipalTEL + "," + sms.ProjectContact.ComPrincipalTEL + "," + sms.ProjectContact.ComLeadTEL,
                                //    tpl_id = int.Parse(System.Configuration.ConfigurationManager.AppSettings["smsw2id"]),
                                //    tpl_value = System.Web.HttpUtility.UrlEncode($"#projectname#={sms.ProjectInfo.ProjectName}&#pointname#={points2}")
                                //};
                                ////var smsstr = Newtonsoft.Json.JsonConvert.SerializeObject(smspara);
                                ////para = new StringContent(smsstr, System.Text.Encoding.UTF8);
                                ////await http.PostAsync(url, para);
                                //await http.GetAsync(url + "?" + "mobile=" + smspara.mobile + "&tpl_id=" + smspara.tpl_id + "&tpl_value=" + smspara.tpl_value + "&key=" + smspara.key);
                            }
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                 LogHelper.WriteLog(typeof(SMSSend), ex);
            }

        }




        public static void SendTelMSG(SMSPacket pack)
        {

            try
            {
                string smsur = System.Configuration.ConfigurationManager.AppSettings["smsmulturl"];//短信地址
                string smsui = System.Configuration.ConfigurationManager.AppSettings["smsuid"];//短信平台用户名
                string smspwd = System.Configuration.ConfigurationManager.AppSettings["smspwd"];//短信平台密码



                //System.Net.Http.HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri(smsur);

                pack.uid = smsui;
                pack.userpwd = smspwd;

                
                var parastr = Newtonsoft.Json.JsonConvert.SerializeObject(pack);
                System.Net.WebRequest request = (System.Net.WebRequest)System.Net.HttpWebRequest.Create(smsur);
                request.Method = "POST";
                Byte[] postbytes = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(parastr);
                request.ContentType = "application/json";
                request.ContentLength = postbytes.Length;
                using (System.IO.Stream stream = request.GetRequestStream())
                {
                    stream.Write(postbytes, 0, postbytes.Length);
                    stream.Close();
                }
                string re = "";
                using (System.Net.WebResponse response = request.GetResponse())
                {
                    if (response == null)
                    {
                        throw new Exception("Response is not Created");
                    }
                    using (System.IO.Stream restream = response.GetResponseStream())
                    {
                        using (System.IO.StreamReader getrd = new System.IO.StreamReader(restream, System.Text.Encoding.UTF8))
                        {
                            re = getrd.ReadToEnd();
                            getrd.Close();
                        }
                        restream.Close();
                    }
                }
                //var para = new StringContent(parastr, System.Text.Encoding.UTF8);
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json");
                //client.DefaultRequestHeaders.Accept.Add(
                //    new MediaTypeWithQualityHeaderValue("application/json"));
                //var result = client.PostAsync("multisend", para).Result;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(SMSSend), ex);
            }

        }

    }
    /// <summary>
    /// 发送参数
    /// </summary>
    public class SMSPara
    {

        public string mobile { get; set; }

        public int tpl_id { get; set; }

        public string tpl_value { get; set; }
        public string key { get
            {
                return System.Configuration.ConfigurationManager.AppSettings["smskey"];
            } }

        public string dtype { get { return "json"; } }
    }




    public class SMSPacket
    {
        /// <summary>
        /// 用户名，不赋值
        /// </summary>
        public string uid { get; set; }
        /// <summary>
        /// 密码，不赋值
        /// </summary>
        public string userpwd { get; set; }
        /// <summary>
        /// 多号码用英文逗号
        /// </summary>
        public string mobile { get; set; }
        /// <summary>
        /// 签名可以不传
        /// </summary>
        public string signature { get; set; }
        /// <summary>
        /// 变理值集合，传入的值顺序与变理顺序对应
        /// </summary>
        public IEnumerable<string> variables { get; set; }
        /// <summary>
        /// 模板ID
        /// </summary>
        public string templateno { get; set; }
    }




}
