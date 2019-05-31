using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
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
                                ss.Add("7");
                                //jj.mobiles = sms.ProjectContact.SitePrincipalTEL + "," + sms.ProjectContact.SiteLinkTEL + "," + sms.ProjectContact.HandlerTEL + "," + sms.ProjectContact.OwnerTEL + "," + sms.ProjectContact.ComLeadTEL;
                                jj.mobiles = (string.IsNullOrEmpty(sms.ProjectContact.SitePrincipalTEL) ? "" : (sms.ProjectContact.SitePrincipalTEL + ","))
                                          + (string.IsNullOrEmpty(sms.ProjectContact.SiteLinkTEL) ? "" : (sms.ProjectContact.SiteLinkTEL + ","))
                                          + (string.IsNullOrEmpty(sms.ProjectContact.HandlerTEL) ? "" : (sms.ProjectContact.HandlerTEL + ","))
                                          + (string.IsNullOrEmpty(sms.ProjectContact.OwnerTEL) ? "" : (sms.ProjectContact.OwnerTEL + ","))
                                          + (string.IsNullOrEmpty(sms.ProjectContact.ComLeadTEL) ? "" : (sms.ProjectContact.ComLeadTEL + ","));
                                jj.templateId = System.Configuration.ConfigurationManager.AppSettings["templateId"];
                                jj.Params= ss;
                                SendTelMSG(jj);
                                LogHelper.WriteLog(typeof(ProJ.SMSClient.SMSSend),Newtonsoft.Json.JsonConvert.SerializeObject(jj));

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
                                ss.Add(points2);
                                ss.Add("14");
                                //jj.mobiles = sms.ProjectContact.SitePrincipalTEL + "," + sms.ProjectContact.SiteLinkTEL + "," + sms.ProjectContact.HandlerTEL + "," + sms.ProjectContact.OwnerTEL + "," + sms.ProjectContact.ComLeadTEL + "," + sms.ProjectContact.ComPrincipalTEL + "," + sms.ProjectContact.LeaderTEL;
                                jj.mobiles = (string.IsNullOrEmpty(sms.ProjectContact.SitePrincipalTEL) ? "" : (sms.ProjectContact.SitePrincipalTEL + ","))
                                           + (string.IsNullOrEmpty(sms.ProjectContact.SiteLinkTEL) ? "" : (sms.ProjectContact.SiteLinkTEL + ","))
                                           + (string.IsNullOrEmpty(sms.ProjectContact.HandlerTEL) ? "" : (sms.ProjectContact.HandlerTEL + ","))
                                           + (string.IsNullOrEmpty(sms.ProjectContact.OwnerTEL) ? "" : (sms.ProjectContact.OwnerTEL + ","))
                                           + (string.IsNullOrEmpty(sms.ProjectContact.ComLeadTEL) ? "" : (sms.ProjectContact.ComLeadTEL + ","))
                                           + (string.IsNullOrEmpty(sms.ProjectContact.ComPrincipalTEL) ? "" : (sms.ProjectContact.ComPrincipalTEL + ","))
                                           + (string.IsNullOrEmpty(sms.ProjectContact.LeaderTEL) ? "" : (sms.ProjectContact.LeaderTEL + ","));
                                jj.templateId = System.Configuration.ConfigurationManager.AppSettings["templateId"];
                                jj.Params = ss;
                                SendTelMSG(jj);
                                LogHelper.WriteLog(typeof(ProJ.SMSClient.SMSSend), Newtonsoft.Json.JsonConvert.SerializeObject(jj));
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
                string smsur = System.Configuration.ConfigurationManager.AppSettings["smsurl"];//短信地址
                string smsui = System.Configuration.ConfigurationManager.AppSettings["apId"];//短信平台用户名
                string smspwd = System.Configuration.ConfigurationManager.AppSettings["secretKey"];//短信平台密码
                string ecname= System.Configuration.ConfigurationManager.AppSettings["ecName"];//企业名
                string sign= System.Configuration.ConfigurationManager.AppSettings["sign"];//企业名

                pack.apId = smsui;
                pack.secretKey = smspwd;
                pack.ecName = ecname;
                pack.sign = sign;
                JObject obj = new JObject();
                obj.Add("ecName", new JValue(pack.ecName));
                obj.Add("apId", new JValue(pack.apId));
                obj.Add("secretKey", new JValue(pack.secretKey));
                obj.Add("mobiles", new JValue(pack.mobiles));
                obj.Add("params", new JValue(JsonConvert.SerializeObject(pack.Params)));
                obj.Add("templateId", new JValue(pack.templateId));
                obj.Add("sign", new JValue(pack.sign));
                obj.Add("addSerial", new JValue(pack.addSerial));
                var mac = pack.ecName + pack.apId + pack.secretKey + pack.templateId + pack.mobiles + JsonConvert.SerializeObject(pack.Params) + pack.sign + pack.addSerial;
                var mac1 = UserMd5(mac);//要进行32位MD5加密
                var length = mac1.Length;
                obj.Add("mac", new JValue(mac1));
                string paras = obj.ToString();
                var jiami = Base64Code(paras);//传参数前要进行64位加密


                var parastr = Newtonsoft.Json.JsonConvert.SerializeObject(pack);
                System.Net.WebRequest request = (System.Net.WebRequest)System.Net.HttpWebRequest.Create(smsur);
                request.Method = "POST";
                Byte[] postbytes = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(jiami);
                request.ContentType = "application/json";
                request.ContentLength = postbytes.Length;
                using (System.IO.Stream stream = request.GetRequestStream())
                {
                    stream.Write(postbytes, 0, postbytes.Length);
                    stream.Close();

                }
                LogHelper.WriteLog(typeof(SMSSend), "发送短信");
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
                    LogHelper.WriteLog(typeof(SMSSend), "发送结果：" + re);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(typeof(SMSSend), ex);
            }

        }


        /// <summary>
        /// Base64加密 
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public static string Base64Code(string Message)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Message);//这里要注意不是Default 因为Default默认GB2312
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Md5 加密
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UserMd5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像
                                   // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("x2");
            }
            return pwd;
        }
    }

    //public class SMSPacket
    //{
    //    /// <summary>
    //    /// 用户名，不赋值
    //    /// </summary>
    //    public string uid { get; set; }
    //    /// <summary>
    //    /// 密码，不赋值
    //    /// </summary>
    //    public string userpwd { get; set; }
    //    /// <summary>
    //    /// 多号码用英文逗号
    //    /// </summary>
    //    public string mobile { get; set; }
    //    /// <summary>
    //    /// 签名可以不传
    //    /// </summary>
    //    public string signature { get; set; }
    //    /// <summary>
    //    /// 变理值集合，传入的值顺序与变理顺序对应
    //    /// </summary>
    //    public IEnumerable<string> variables { get; set; }
    //    /// <summary>
    //    /// 模板ID
    //    /// </summary>
    //    public string templateno { get; set; }
    //}

    public class SMSPacket
    {
        /// <summary>
        /// 企业名
        /// </summary>
        public string ecName { get; set; }
        /// <summary>
        /// 接口配置的用户名
        /// </summary>
        public string apId { get; set; }
        /// <summary>
        /// 接口配置的密码
        /// </summary>
        public string secretKey { get; set; }
        /// <summary>
        /// 电话以逗号隔开
        /// </summary>
        public string mobiles { get; set; }
        /// <summary>
        /// 模板号
        /// </summary>
        public string templateId { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 可不填
        /// </summary>
        public string addSerial { get; set; } = "";
        /// <summary>
        /// 参数
        /// </summary>
        public List<string> Params { get; set; }

    }




}
