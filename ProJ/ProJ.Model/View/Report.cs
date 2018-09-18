using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.View
{
    public class Report
    {
        /// <summary>
        /// 有一个超期项目名变红
        /// </summary>
        public bool ProJBool { get; set; }
        /// <summary>
        /// 业主单位
        /// </summary>
        public DB.Basic_Owner ProjectOwner { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public DB.Project_Contacts Project_Contacts { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public DB.Project_Info ProjectInfo { get; set; }
        /// <summary>
        /// 计划执行
        /// </summary>
        public PointBase ScheduleTitle { get; set; }

        /// <summary>
        /// 工程可行性研究报告批复
        /// </summary>
        public PointBase Point_GCKXXYJBGPF { get; set; }
        /// <summary>
        /// 建设用地规划许可证批复
        /// </summary>
        public PointBase Point_JSYDGHXKZPF { get; set; }
        /// <summary>
        /// 地勘报告完成
        /// </summary>
        public PointBase Point_DKBGWC { get; set; }
        /// <summary>
        /// 初步设计及概算批复
        /// </summary>
        public PointBase Point_CBSJJGSPF { get; set; }
        /// <summary>
        /// 施工图编制和审查
        /// </summary>
        public PointBase Point_SGTBZHSC { get; set; }
        /// <summary>
        /// 预算编制完成
        /// </summary>
        public PointBase Point_YSBZWC { get; set; }
        /// <summary>
        /// 财审控制价批复
        /// </summary>
        public PointBase Point_CSKZJPF { get; set; }
        /// <summary>
        /// 施工监理招投标
        /// </summary>
        public PointBase Point_SGJLZTP { get; set; }
        /// <summary>
        /// 项目开工
        /// </summary>
        public PointBase Point_XMKG { get; set; }
        /// <summary>
        /// 建设工程规划许可证批复
        /// </summary>
        public PointBase Point_JSGCGHXKZPF { get; set; }
        /// <summary>
        /// 施工监理人员备案
        /// </summary>
        public PointBase Point_SGJLRYBA { get; set; }
        /// <summary>
        /// 施工许可证批复
        /// </summary>
        public PointBase Point_SGXKZPF { get; set; }
        /// <summary>
        /// 《规划选址及用地意见书》批复
        /// </summary>
        public PointBase Point_GHXZYDJYJSPF { get; set; }
        /// <summary>
        /// 农转用手续及供地批复
        /// </summary>
        public PointBase Point_LZYSXJGDPF { get; set; }
        /// <summary>
        /// 土地出让合同
        /// </summary>
        public PointBase Point_TDCRHT { get; set; }
        /// <summary>
        /// 土地使用权证
        /// </summary>
        public PointBase Point_TDSYQZ { get; set; }
        /// <summary>
        /// 项目总平设计方案批复
        /// </summary>
        public PointBase Point_XMZPSJFAPF { get; set; }
        /// <summary>
        /// 问题列表
        /// </summary>
        //public IEnumerable<Model.DB.Project_Issue> Issues { get; set; }
        public Model.DB.Project_Issue Issues { get; set; }
        /// <summary>
        /// 问题Str
        /// </summary>
        public string IssuesStr { get; set; }

    }
    /// <summary>
    /// 节点基类
    /// </summary>
    public class PointBase
    {
        /// <summary>
        /// 计划
        /// </summary>
        public string Plan { get; set; }
        /// <summary>
        /// 执行
        /// </summary>
        public string Exec { get; set; }
        /// <summary>
        /// 是否超期
        /// </summary>
        public bool Execeed { get; set; }
    }
    /// <summary>
    /// 动态报表模型
    /// </summary>
    public class ReporDynlist
    {
        /// <summary>
        /// true
        /// </summary>
        public bool isssum { get; set; }
        /// <summary>
        /// 有一个超期项目名变红
        /// </summary>
        public bool ProJBool { get; set; }
        /// <summary>
        /// 问题列表
        /// </summary>
        public Model.DB.Project_Issue Issues { get; set; }
        /// <summary>
        /// 业主单位
        /// </summary>
        public DB.Basic_Owner ProjectOwner { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public ProjContacts Project_Contacts { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public Projinfo ProjectInfo { get; set; }
        /// <summary>
        /// 动态数据,每个节点会提供3个属性
        /// sch_+ colname 计划
        /// exc_+ colname 执行
        /// tot_+ colnmae 逾期 大于0表示逾期  否则不逾期
        /// </summary>
        public dynamic PointData { get; set; }
    }
    public class Projinfo
    {
        public DateTime? test { get; set; }
        public Guid ID { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateMan { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }
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
        public string ComemenceDate { get; set; }
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
        /// 下一周工作计划
        /// </summary>
        public string NextPlan { get; set; }
        /// <summary>
        /// 责任管理部门名称
        /// </summary>
        public string Department { get; set; }
    }
    /// <summary>
    /// 项目联系人
    /// </summary>
    public class ProjContacts
    {
        public Guid ID { get; set; }
        public Guid ProjectID { get; set; }
        /// <summary>
        /// 片区负责人（责任管理部门项目负责人）
        /// </summary>
        public string SitePrincipal { get; set; }
        /// <summary>
        /// 片区负责人电话（责任管理部门项目负责人电话）
        /// </summary>
        public string SitePrincipalTEL { get; set; }
        /// <summary>
        /// 片区联系人（责任管理部门具体负责人）
        /// </summary>
        public string SiteLink { get; set; }
        /// <summary>
        /// 片区联系人电话（责任管理部门具体负责人电话）
        /// </summary>
        public string SiteLinkTEL { get; set; }
        /// <summary>
        /// 经办人（业主单位项目负责人）
        /// </summary>
        public string Handler { get; set; }
        /// <summary>
        /// 经办人电话（业主单位项目负责人电话）
        /// </summary>
        public string HandlerTEL { get; set; }
        /// <summary>
        /// 分管领导（业主分管领导）
        /// </summary>
        public string Principal { get; set; }
        /// <summary>
        /// 分管领导电话（业主分管领导电话）
        /// </summary>
        public string PrincipalTEL { get; set; }
        /// <summary>
        /// 主要领导（业主主要领导）
        /// </summary>
        public string Leader { get; set; }
        /// <summary>
        /// 主要领导电话（业主主要领导电话）
        /// </summary>
        public string LeaderTEL { get; set; }
        /// <summary>
        /// 集团公司分管领导
        /// </summary>
        public string ComLead { get; set; }
        /// <summary>
        /// 集团公司分管领导电话
        /// </summary>
        public string ComLeadTEL { get; set; }
        /// <summary>
        /// 集团公司主要负责人
        /// </summary>
        public string ComPrincipal { get; set; }
        /// <summary>
        /// 集团公司主要负责人电话
        /// </summary>
        public string ComPrincipalTEL { get; set; }
        /// <summary>
        /// 责任管理部门责任领导
        /// </summary>
        public string DeptPrincipal { get; set; }
        /// <summary>
        /// 责任管理部门责任领导
        /// </summary>
        public string DeptPrincipalTEL { get; set; }
        /// <summary>
        /// 业主具体责任人
        /// </summary>
        public string OwnerPrinci { get; set; }
        /// <summary>
        /// 业主具体责任人电话
        /// </summary>
        public string OwnerTEL { get; set; }
    }
    public class ReportDyn
    {
        /// <summary>
        /// 动态列集合，返回结果集要按OrderIndex排序
        /// </summary>
        public IEnumerable<ReportColumn> ReportCols { get; set; }
        public Pager<Model.View.ReporDynlist> ReporDynlist { get; set; }
        public Pank Pank { get; set; }
    }
    public class Pank
    {
        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsOv { get; set; }
        /// <summary>
        /// xx月份
        /// </summary>
        public DateTime Moth { get; set; }
        /// <summary>
        /// xx个前期项目
        /// </summary>
        public string Prophase { get; set; }
        /// <summary>
        /// xx个项目正常推进
        /// </summary>
        public string Normal { get; set; }
        /// <summary>
        /// xx个项目未按序时推进
        /// </summary>
        public string Exec { get; set; }
        /// <summary>
        /// xx个项目滞后1个月
        /// </summary>
        public string POne { get; set; }
        /// <summary>
        /// xx个项目滞后2个月
        /// </summary>
        public string PTwo { get; set; }
        /// <summary>
        /// xx个项目滞后3个月以上
        /// </summary>
        public string PThree { get; set; }
    }
     
    /// <summary>
    /// 动态列
    /// </summary>
    public class ReportColumn
    {

        public ReportColumn()
        {
            Isbool = false;
            IsColumn = true;
            IsPoint = true;
            IsClass = true;
            MultiColumn = false;
            ShowModal = false;
            Children = new List<ReportColumn>();
        }
        /// <summary>
        /// 判断计划
        /// </summary>
        public bool Isbool { get; set; }
        /// <summary>
        /// 是否显示列
        /// </summary>
        public bool IsColumn { get; set; }
        /// <summary>
        /// 列名，来自Basic_Point的ColName
        /// </summary>
        public string ColName { get; set; }
        /// <summary>
        /// 列标题，来自Basic_Point的PointName
        /// </summary>
        public string Caption { get; set; } 
        /// <summary>
        /// 排序值
        /// </summary>
        public int OrderIndex { get; set; }
        /// <summary>
        /// 是否多列表头
        /// </summary>
        public bool MultiColumn { get; set; }
        /// <summary>
        /// 是否class样式
        /// </summary>
        public bool IsClass { get; set; }
        /// <summary>
        /// 是否节点
        /// </summary>
        public bool IsPoint { get; set; }
        /// <summary>
        /// 列固定
        /// </summary>
        public bool ColumnFixed { get; set; }
        /// <summary>
        /// 是否点击弹出Modal
        /// </summary>
        public bool ShowModal { get; set; }
        /// <summary>
        /// 多列表头明细
        /// </summary>
        public IEnumerable<ReportColumn> Children { get; set; }
    }
}
