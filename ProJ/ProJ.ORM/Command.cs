using ProJ.Model.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.ORM
{
    public class Command
    {
        /// <summary>
        /// 创建登陆Token
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateToken(int length)
        {
            //定义  
            string basestr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            string sb = "";
            for (int i = 0; i < length; i++)
            {
                int number = random.Next(basestr.Length);
                sb += basestr.Substring(number, 1);
            }
            return sb;
        }
        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="dataSource"></param>
        /// <returns></returns>
        public static string CreateExcel(DataTable dataSource, string filePaht)
        {
            var file = filePaht;// System.Web.Hosting.HostingEnvironment.MapPath(@"~/Template/");


            //操作excel
            NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
            NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("project");
            //标题行
            NPOI.SS.UserModel.IRow trow = sheet.CreateRow(0);

            foreach (DataColumn col in dataSource.Columns)
            {
                trow.CreateCell(col.Ordinal).SetCellValue(col.Caption);
            }
            //创建数据行
            int j = 1;//第一行是标题
            foreach (DataRow row in dataSource.Rows)
            {
                //创建行
                NPOI.SS.UserModel.IRow drow = sheet.CreateRow(j);
                foreach (DataColumn col in dataSource.Columns)
                {
                    var value = row[col.Ordinal];
                    var str = "";
                    if (!(value is DBNull))
                    {
                        str = value.ToString();
                    }
                    drow.CreateCell(col.Ordinal).SetCellValue(str);
                }

                j++;
            }
            string re = Guid.NewGuid().ToString() + ".xls";

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                book.Write(ms);
                using (System.IO.FileStream fs = new System.IO.FileStream(file + re, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
                book = null;
            }

            //删除 10分钟之前的数据
            foreach (var f in System.IO.Directory.GetFileSystemEntries(file, "*.xls"))
            {
                if (File.Exists(f))
                {
                    if ((DateTime.Now - File.GetCreationTime(f)).TotalMinutes > 10)//10分钟前的数据清掉
                    {

                        File.Delete(f);
                    }
                }
            }

            return re;

        }
        /// <summary>
        /// 创建excel
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string CreateExcel(IEnumerable<Report> source, string filePath)
        {
            try
            {

                var tb = new DataTable();

                var file = filePath;// System.Web.Hosting.HostingEnvironment.MapPath(@"~/Template/");
                                    //操作excel
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("project");
                //标题行
                NPOI.SS.UserModel.IRow trow = sheet.CreateRow(0);
                trow.CreateCell(0).SetCellValue("项目名称");
                trow.CreateCell(1).SetCellValue("计划/执行");
                trow.CreateCell(2).SetCellValue("年度计划");
                trow.CreateCell(3).SetCellValue("计划开工");
                trow.CreateCell(4).SetCellValue("工程可行性研究报告批复");
                trow.CreateCell(5).SetCellValue("建设用地规划许可证批复");
                trow.CreateCell(6).SetCellValue("地勘报告完成");
                trow.CreateCell(7).SetCellValue("初步设计及概算批复");
                trow.CreateCell(8).SetCellValue("施工图编制和审查");
                trow.CreateCell(9).SetCellValue("预算编制完成");
                trow.CreateCell(10).SetCellValue("财审控制价批复");
                trow.CreateCell(11).SetCellValue("施工监理招投标");
                trow.CreateCell(12).SetCellValue("项目开工");
                trow.CreateCell(13).SetCellValue("建设工程规划许可证批复");
                trow.CreateCell(14).SetCellValue("施工监理人员备案");
                trow.CreateCell(15).SetCellValue("施工许可证批复");
                trow.CreateCell(16).SetCellValue("规划选址及用地意见书批复");
                trow.CreateCell(17).SetCellValue("农转用手续及供地批复");
                trow.CreateCell(18).SetCellValue("土地出让合同");
                trow.CreateCell(19).SetCellValue("土地使用权证");
                trow.CreateCell(20).SetCellValue("项目总平设计方案批复");
                trow.CreateCell(21).SetCellValue("项目问题");
                trow.CreateCell(22).SetCellValue("第三季度完成投资");
                trow.CreateCell(23).SetCellValue("第三季度期末形象进度");
                trow.CreateCell(24).SetCellValue("第四季度完成投资");
                trow.CreateCell(25).SetCellValue("第四季度期末形象进度");
                trow.CreateCell(26).SetCellValue("业主单位");
                trow.CreateCell(27).SetCellValue("项目负责人");
                trow.CreateCell(28).SetCellValue("具体负责人");
                if (source == null)
                    throw new Exception("参数为空");
                if (source.Count() == 0)
                    throw new Exception("参数没有数据");

                System.Type t = source.FirstOrDefault().GetType();
                var sps = t.GetProperties();
                //int rowint = 0;
                //foreach (var p in sps)
                //{
                //    trow.CreateCell(rowint).SetCellValue(p.Name);
                //    rowint++;
                //}
                //创建数据行
                int j = 1;
                foreach (var obj in source)
                {
                    //创建行
                    NPOI.SS.UserModel.IRow drow = sheet.CreateRow(j);
                    NPOI.SS.UserModel.IRow drowsec = sheet.CreateRow(j + 1);
                    drow.CreateCell(0).SetCellValue(obj.ProjectInfo.ProjectName);
                    drowsec.CreateCell(0).SetCellValue(obj.ProjectInfo.ProjectName);
                    drow.CreateCell(1).SetCellValue("计划");
                    drowsec.CreateCell(1).SetCellValue("执行");
                    drow.CreateCell(2).SetCellValue(obj.ProjectInfo.InvestMoney.ToString());
                    drowsec.CreateCell(2).SetCellValue(obj.ProjectInfo.InvestMoney.ToString());
                    drow.CreateCell(3).SetCellValue(obj.ProjectInfo.ComemenceDate.ToString());
                    drowsec.CreateCell(3).SetCellValue(obj.ProjectInfo.ComemenceDate.ToString());
                    drow.CreateCell(4).SetCellValue(obj.Point_GCKXXYJBGPF.Plan);
                    drowsec.CreateCell(4).SetCellValue(obj.Point_GCKXXYJBGPF.Exec);
                    drow.CreateCell(5).SetCellValue(obj.Point_JSYDGHXKZPF.Plan);
                    drowsec.CreateCell(5).SetCellValue(obj.Point_JSYDGHXKZPF.Plan);
                    drow.CreateCell(6).SetCellValue(obj.Point_DKBGWC.Plan);
                    drowsec.CreateCell(6).SetCellValue(obj.Point_DKBGWC.Plan);
                    drow.CreateCell(7).SetCellValue(obj.Point_DKBGWC.Exec);
                    drowsec.CreateCell(7).SetCellValue(obj.Point_CBSJJGSPF.Plan);
                    drow.CreateCell(8).SetCellValue(obj.Point_CBSJJGSPF.Exec);
                    drowsec.CreateCell(8).SetCellValue(obj.Point_SGTBZHSC.Plan);
                    drow.CreateCell(9).SetCellValue(obj.Point_SGTBZHSC.Exec);
                    drowsec.CreateCell(9).SetCellValue(obj.Point_YSBZWC.Plan);
                    drow.CreateCell(10).SetCellValue(obj.Point_CSKZJPF.Plan);
                    drowsec.CreateCell(10).SetCellValue(obj.Point_CSKZJPF.Exec);
                    drow.CreateCell(11).SetCellValue(obj.Point_SGJLZTP.Plan);
                    drowsec.CreateCell(11).SetCellValue(obj.Point_SGJLZTP.Exec);
                    drow.CreateCell(12).SetCellValue(obj.Point_XMKG.Plan);
                    drowsec.CreateCell(12).SetCellValue(obj.Point_XMKG.Exec);
                    drow.CreateCell(13).SetCellValue(obj.Point_JSGCGHXKZPF.Plan);
                    drowsec.CreateCell(13).SetCellValue(obj.Point_JSGCGHXKZPF.Exec);
                    drow.CreateCell(14).SetCellValue(obj.Point_SGJLRYBA.Plan);
                    drowsec.CreateCell(42).SetCellValue(obj.Point_SGJLRYBA.Exec);
                    drow.CreateCell(15).SetCellValue(obj.Point_SGXKZPF.Plan);
                    drowsec.CreateCell(15).SetCellValue(obj.Point_SGXKZPF.Exec);
                    drow.CreateCell(16).SetCellValue(obj.Point_GHXZYDJYJSPF.Plan);
                    drowsec.CreateCell(16).SetCellValue(obj.Point_GHXZYDJYJSPF.Exec);
                    drow.CreateCell(17).SetCellValue(obj.Point_LZYSXJGDPF.Plan);
                    drowsec.CreateCell(17).SetCellValue(obj.Point_LZYSXJGDPF.Exec);
                    drow.CreateCell(18).SetCellValue(obj.Point_TDCRHT.Plan);
                    drowsec.CreateCell(18).SetCellValue(obj.Point_TDCRHT.Exec);
                    drow.CreateCell(19).SetCellValue(obj.Point_TDSYQZ.Plan);
                    drowsec.CreateCell(19).SetCellValue(obj.Point_TDSYQZ.Exec);
                    drow.CreateCell(20).SetCellValue(obj.Point_XMZPSJFAPF.Plan);
                    drowsec.CreateCell(20).SetCellValue(obj.Point_XMZPSJFAPF.Exec);
                    drow.CreateCell(21).SetCellValue(obj.IssuesStr);
                    drowsec.CreateCell(21).SetCellValue(obj.IssuesStr);
                    drow.CreateCell(22).SetCellValue(obj.ProjectInfo.Q3Invest.ToString());
                    drowsec.CreateCell(22).SetCellValue(obj.ProjectInfo.Q3Invest.ToString());
                    drow.CreateCell(23).SetCellValue(obj.ProjectInfo.Q3Memo);
                    drowsec.CreateCell(23).SetCellValue(obj.ProjectInfo.Q3Memo);
                    drow.CreateCell(24).SetCellValue(obj.ProjectInfo.Q4Invest.ToString());
                    drowsec.CreateCell(24).SetCellValue(obj.ProjectInfo.Q4Invest.ToString());
                    drow.CreateCell(25).SetCellValue(obj.ProjectInfo.Q4Memo);
                    drowsec.CreateCell(25).SetCellValue(obj.ProjectInfo.Q4Memo);
                    drow.CreateCell(26).SetCellValue(obj.ProjectOwner.OwnerName);
                    drowsec.CreateCell(26).SetCellValue(obj.ProjectOwner.OwnerName);
                    drow.CreateCell(27).SetCellValue(obj.ProjectOwner.Handler);
                    drowsec.CreateCell(27).SetCellValue(obj.ProjectOwner.Handler);
                    drow.CreateCell(28).SetCellValue(obj.ProjectOwner.Principal);
                    drowsec.CreateCell(28).SetCellValue(obj.ProjectOwner.Principal);
                    j += 2;
                }
                string re = Guid.NewGuid().ToString() + ".xls";

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    book.Write(ms);
                    using (System.IO.FileStream fs = new System.IO.FileStream(file + re, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    {
                        byte[] data = ms.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                    book = null;
                }

                //删除 10分钟之前的数据
                foreach (var f in System.IO.Directory.GetFileSystemEntries(file, "*.xls"))
                {
                    if (File.Exists(f))
                    {
                        if ((DateTime.Now - File.GetCreationTime(f)).TotalMinutes > 10)//10分钟前的数据清掉
                        {

                            File.Delete(f);
                        }
                    }
                }

                return "OutPutTemp/" + re;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /// <summary>
        /// 创建excel
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string CreateExcel(IEnumerable<ReporDynlist> source, IEnumerable<ReportColumn> list, string filePath)
        {
            try
            {
                var tb = new DataTable();

                var file = filePath;// System.Web.Hosting.HostingEnvironment.MapPath(@"~/Template/");
                                    //操作excel
                NPOI.HSSF.UserModel.HSSFWorkbook book = new NPOI.HSSF.UserModel.HSSFWorkbook();
                NPOI.SS.UserModel.ISheet sheet = book.CreateSheet("project");
                //标题行
                NPOI.SS.UserModel.IRow trow = sheet.CreateRow(0);
                trow.CreateCell(0).SetCellValue("项目名称");
                trow.CreateCell(1).SetCellValue("计划/执行");
                trow.CreateCell(2).SetCellValue("年度计划");
                trow.CreateCell(3).SetCellValue("计划开工");
                int t = 4;
                foreach (var item in list)
                {
                    trow.CreateCell(t).SetCellValue(item.Caption);
                    t += 1;
                }
                trow.CreateCell(t).SetCellValue("当前进展情况及存在问题");
                trow.CreateCell(t + 1).SetCellValue("下一周工作计划");
                trow.CreateCell(t + 2).SetCellValue("第一季度完成投资");
                trow.CreateCell(t + 3).SetCellValue("第一季度期末形象进度");
                trow.CreateCell(t + 4).SetCellValue("第二季度完成投资");
                trow.CreateCell(t + 5).SetCellValue("第二季度期末形象进度");
                trow.CreateCell(t + 6).SetCellValue("第三季度完成投资");
                trow.CreateCell(t + 7).SetCellValue("第三季度期末形象进度");
                trow.CreateCell(t + 8).SetCellValue("第四季度完成投资");
                trow.CreateCell(t + 9).SetCellValue("第四季度期末形象进度");
                trow.CreateCell(t + 10).SetCellValue("业主单位");
                trow.CreateCell(t + 11).SetCellValue("项目负责人");
                trow.CreateCell(t + 12).SetCellValue("具体负责人");
                if (source == null)
                    throw new Exception("参数为空");
                if (source.Count() == 0)
                    throw new Exception("参数没有数据");
                //创建数据行
                int j = 1;
                foreach (var obj in source)
                {
                    int x = 4;
                    int y = 4;
                    //创建行
                    NPOI.SS.UserModel.IRow drow = sheet.CreateRow(j);
                    NPOI.SS.UserModel.IRow drowsec = sheet.CreateRow(j + 1);
                    drow.CreateCell(0).SetCellValue(obj.ProjectInfo.ProjectName);
                    drowsec.CreateCell(0).SetCellValue(obj.ProjectInfo.ProjectName);
                    drow.CreateCell(1).SetCellValue("计划");
                    drowsec.CreateCell(1).SetCellValue("执行");
                    drow.CreateCell(2).SetCellValue(obj.ProjectInfo.InvestMoney.ToString());
                    drowsec.CreateCell(2).SetCellValue(obj.ProjectInfo.InvestMoney.ToString());
                    drow.CreateCell(3).SetCellValue(((DateTime)obj.ProjectInfo.ComemenceDate).ToString("yyyy.MM.dd"));
                    drowsec.CreateCell(3).SetCellValue(((DateTime)obj.ProjectInfo.ComemenceDate).ToString("yyyy.MM.dd"));
                    List<string> obj1 = new List<string>();
                    foreach (var item in obj.PointData)
                    {
                        if (item.Key.Contains("sch"))
                        {
                            drow.CreateCell(x).SetCellValue(item.Value.ToString());
                            //if (item.Value.ToString()== "/")
                            //{
                            //    drow.CreateCell(x).SetCellValue(item.Value.ToString());
                            //}
                            //else
                            //{
                            //    var date = Convert.ToDateTime(item.Value.ToString());
                            //    drow.CreateCell(x).SetCellValue(date.ToString("yyyy.MM.dd"));
                            //}
                            x++;
                        }
                        if (item.Key.Contains("exc"))
                        {
                            drowsec.CreateCell(y).SetCellValue(item.Value.ToString());
                            //DateTime dt;
                            //var ni = DateTime.TryParse(item.Value.ToString(), out dt);
                            //if (item.Value.ToString()== "/" ||ni==false )
                            //{
                            //    drowsec.CreateCell(y).SetCellValue(item.Value.ToString());
                            //}
                            //else
                            //{
                            //    var date = Convert.ToDateTime(item.Value.ToString());
                            //    drowsec.CreateCell(y).SetCellValue(date.ToString("yyyy.MM.dd"));
                            //}
                            y++;
                        }
                    };
                    drow.CreateCell(x).SetCellValue(obj.Issues==null?"": obj.Issues.IssueContent.ToString());
                    drowsec.CreateCell(x).SetCellValue(obj.Issues== null ? "" : obj.Issues.IssueContent.ToString());
                    drow.CreateCell(x + 1).SetCellValue(obj.ProjectInfo.NextPlan.ToString());
                    drowsec.CreateCell(x + 1).SetCellValue(obj.ProjectInfo.NextPlan.ToString());
                    drow.CreateCell(x + 2).SetCellValue(obj.ProjectInfo.Q1Invest.ToString());
                    drowsec.CreateCell(x + 2).SetCellValue(obj.ProjectInfo.Q1Invest.ToString());
                    drow.CreateCell(x + 3).SetCellValue(obj.ProjectInfo.Q1Memo);
                    drowsec.CreateCell(x + 3).SetCellValue(obj.ProjectInfo.Q1Memo);
                    drow.CreateCell(x + 4).SetCellValue(obj.ProjectInfo.Q2Invest.ToString());
                    drowsec.CreateCell(x + 4).SetCellValue(obj.ProjectInfo.Q2Invest.ToString());
                    drow.CreateCell(x + 5).SetCellValue(obj.ProjectInfo.Q2Memo);
                    drowsec.CreateCell(x + 5).SetCellValue(obj.ProjectInfo.Q2Memo);
                    drow.CreateCell(x + 6).SetCellValue(obj.ProjectInfo.Q3Invest.ToString());
                    drowsec.CreateCell(x + 6).SetCellValue(obj.ProjectInfo.Q3Invest.ToString());
                    drow.CreateCell(x + 7).SetCellValue(obj.ProjectInfo.Q3Memo);
                    drowsec.CreateCell(x + 7).SetCellValue(obj.ProjectInfo.Q3Memo);
                    drow.CreateCell(x + 8).SetCellValue(obj.ProjectInfo.Q4Invest.ToString());
                    drowsec.CreateCell(x + 8).SetCellValue(obj.ProjectInfo.Q4Invest.ToString());
                    drow.CreateCell(x + 9).SetCellValue(obj.ProjectInfo.Q4Memo);
                    drowsec.CreateCell(x + 9).SetCellValue(obj.ProjectInfo.Q4Memo);
                    drow.CreateCell(x + 10).SetCellValue(obj.ProjectOwner.OwnerName);
                    drowsec.CreateCell(x + 10).SetCellValue(obj.ProjectOwner.OwnerName);
                    drow.CreateCell(x + 11).SetCellValue(obj.ProjectOwner.Handler);
                    drowsec.CreateCell(x + 11).SetCellValue(obj.ProjectOwner.Handler);
                    drow.CreateCell(x + 12).SetCellValue(obj.ProjectOwner.Principal);
                    drowsec.CreateCell(x + 12).SetCellValue(obj.ProjectOwner.Principal);
                    j += 2;
                }
                string re = Guid.NewGuid().ToString() + ".xls";

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    book.Write(ms);
                    using (System.IO.FileStream fs = new System.IO.FileStream(file + re, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    {
                        byte[] data = ms.ToArray();
                        fs.Write(data, 0, data.Length);
                        fs.Flush();
                    }
                    book = null;
                }

                //删除 10分钟之前的数据
                foreach (var f in System.IO.Directory.GetFileSystemEntries(file, "*.xls"))
                {
                    if (File.Exists(f))
                    {
                        if ((DateTime.Now - File.GetCreationTime(f)).TotalMinutes > 10)//10分钟前的数据清掉
                        {

                            File.Delete(f);
                        }
                    }
                }

                return "OutPutTemp/" + re;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
    }
}
