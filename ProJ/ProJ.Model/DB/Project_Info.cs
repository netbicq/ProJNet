using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.DB
{
    /// <summary>
    /// 项目信息
    /// </summary>
    public class Project_Info:ModelBaseEx
    {
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 业主ID
        /// </summary>
        public Guid OwnerID { get; set; }
        /// <summary>
        /// 项目级别ID
        /// </summary>
        public Guid LevelID { get; set; }
        /// <summary>
        /// 项目行业ID
        /// </summary>
        public Guid IndustryID { get; set; }
        /// <summary>
        /// 项目投资额
        /// </summary>
        public decimal? InvestMoney { get; set; }
        /// <summary>
        /// 开工日期
        /// </summary>
        public DateTime? ComemenceDate { get; set; }
        /// <summary>
        /// 第一季度投资
        /// </summary>
        public decimal? Q1Invest { get; set; }
        /// <summary>
        /// 第二季度投资
        /// </summary>
        public decimal? Q2Invest { get; set; }
        /// <summary>
        /// 第三季度投资
        /// </summary>
        public decimal? Q3Invest { get; set; }
        /// <summary>
        /// 第四季度投资
        /// </summary>
        public decimal? Q4Invest { get; set; }
        /// <summary>
        /// 第一季度末形象进度
        /// </summary>
        public string Q1Memo { get; set; }
        /// <summary>
        /// 第二季度末形象进度
        /// </summary>
        public string Q2Memo { get; set; }
        /// <summary>
        /// 第三季度末形象进度
        /// </summary>
        public string Q3Memo { get; set; }
        /// <summary>
        /// 第四季度末形象进度
        /// </summary>
        public string Q4Memo { get; set; }
        /// <summary>
        /// 超期第一周短信发送时间
        /// </summary>
        public DateTime? W1SMS { get; set; }
        /// <summary>
        /// 超期第二周短信发送时间
        /// </summary>
        public DateTime? W2SMS { get; set; }
        /// <summary>
        /// 超期第三周短信发送时间
        /// </summary>
        public DateTime? W3SMS { get; set; }
    }
}
