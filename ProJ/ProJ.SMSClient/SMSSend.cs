using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
                                var smspara = new SMSPara()
                                {
                                    mobile = "15523213827",
                                    //mobile = sms.ProjectContact.HandlerTEL+","+ sms.ProjectContact.PrincipalTEL + "," + sms.ProjectContact.SiteLinkTEL,
                                    tpl_id = int.Parse(System.Configuration.ConfigurationManager.AppSettings["smsw1id"]),
                                    tpl_value = System.Web.HttpUtility.UrlEncode($"#projectname#={sms.ProjectInfo.ProjectName}&#pointname#={points}")
                                };
                                //var smsstr = Newtonsoft.Json.JsonConvert.SerializeObject(smspara);
                                //para = new StringContent(smsstr, System.Text.Encoding.UTF8);
                                //await http.PostAsync(url, para);
                                await http.GetAsync(url + "?" + "mobile=" + smspara.mobile + "&tpl_id=" + smspara.tpl_id + "&tpl_value=" + smspara.tpl_value + "&key=" + smspara.key);
                            }
                            if (points2!="")
                            {
                                //发送2周
                                var smspara = new SMSPara()
                                {
                                    mobile= "15523213827",
                                    //mobile = sms.ProjectContact.SitePrincipalTEL + "," + sms.ProjectContact.PrincipalTEL + "," + sms.ProjectContact.SiteLinkTEL + "," + sms.ProjectContact.LeaderTEL
                                    //+ "," + sms.ProjectContact.DeptPrincipalTEL + "," + sms.ProjectContact.ComPrincipalTEL + "," + sms.ProjectContact.ComLeadTEL,
                                    tpl_id = int.Parse(System.Configuration.ConfigurationManager.AppSettings["smsw2id"]),
                                    tpl_value = System.Web.HttpUtility.UrlEncode($"#projectname#={sms.ProjectInfo.ProjectName}&#pointname#={points2}")
                                };
                                //var smsstr = Newtonsoft.Json.JsonConvert.SerializeObject(smspara);
                                //para = new StringContent(smsstr, System.Text.Encoding.UTF8);
                                //await http.PostAsync(url, para);
                                await http.GetAsync(url + "?" + "mobile=" + smspara.mobile + "&tpl_id=" + smspara.tpl_id + "&tpl_value=" + smspara.tpl_value + "&key=" + smspara.key);
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

   
}
