using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.Para
{

    public class ProjectAdd
    {
        /// <summary>
        /// 项目信息
        /// </summary>
        public ProjectNew ProjectInfo { get; set; }
        /// <summary>
        /// 项目计划，弃用
        /// </summary>
        public ProjectScheduleNew Schedules { get; set; }
        /// <summary>
        /// 项目计划节点
        /// </summary>
        public IEnumerable <ProjectPointScheduleNew> PointSchedules { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public ProjectContactNew Contacts { get; set; }
    }
    public class ProjectNew
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
        /// 下一周工作计划
        /// </summary>
        public string NextPlan { get; set; }

    }

    public class ProjectScheduleNew
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid ProjectID { get; set; }
        /// <summary>
        /// 日程类型 
        /// </summary>
        public int ScheduleType { get; set; }
        /// <summary>
        /// 工程可行性研究报告批复
        /// </summary>
        public DateTime? Point_GCKXXYJBGPF { get; set; }
        /// <summary>
        /// 建设用地规划许可证批复
        /// </summary>
        public DateTime? Point_JSYDGHXKZPF { get; set; }
        /// <summary>
        /// 地勘报告完成
        /// </summary>
        public DateTime? Point_DKBGWC { get; set; }
        /// <summary>
        /// 初步设计及概算批复
        /// </summary>
        public DateTime? Point_CBSJJGSPF { get; set; }
        /// <summary>
        /// 施工图编制和审查
        /// </summary>
        public DateTime? Point_SGTBZHSC { get; set; }
        /// <summary>
        /// 预算编制完成
        /// </summary>
        public DateTime? Point_YSBZWC { get; set; }
        /// <summary>
        /// 财审控制价批复
        /// </summary>
        public DateTime? Point_CSKZJPF { get; set; }
        /// <summary>
        /// 施工监理招投标
        /// </summary>
        public DateTime? Point_SGJLZTP { get; set; }
        /// <summary>
        /// 项目开工
        /// </summary>
        public DateTime? Point_XMKG { get; set; }
        /// <summary>
        /// 建设工程规划许可证批复
        /// </summary>
        public DateTime? Point_JSGCGHXKZPF { get; set; }
        /// <summary>
        /// 施工监理人员备案
        /// </summary>
        public DateTime? Point_SGJLRYBA { get; set; }
        /// <summary>
        /// 施工许可证批复
        /// </summary>
        public DateTime? Point_SGXKZPF { get; set; }
        /// <summary>
        /// 《规划选址及用地意见书》批复
        /// </summary>
        public DateTime? Point_GHXZYDJYJSPF { get; set; }
        /// <summary>
        /// 农转用手续及供地批复
        /// </summary>
        public DateTime? Point_LZYSXJGDPF { get; set; }
        /// <summary>
        /// 土地出让合同
        /// </summary>
        public DateTime? Point_TDCRHT { get; set; }
        /// <summary>
        /// 土地使用权证
        /// </summary>
        public DateTime? Point_TDSYQZ { get; set; }
        /// <summary>
        /// 项目总平设计方案批复
        /// </summary>
        public DateTime? Point_XMZPSJFAPF { get; set; }
        /// <summary>
        /// 《规划选址及用地意见书》备注
        /// </summary>
        public string Point_GHXZJYDYJSMemo { get; set; }
        /// <summary>
        /// 农转用手续及供地批复备注
        /// </summary>
        public string Point_LZYSXJGDMemo { get; set; }
        /// <summary>
        /// 土地出让合同（招投标项目填
        /// </summary>
        public string Point_TDCRHTMemo { get; set; }
        /// <summary>
        /// 土地使用权证
        /// </summary>
        public string Point_TDSYQZMemo { get; set; }
        /// <summary>
        /// 项目总平设计方案批复
        /// </summary>
        public string Point_XMZPSJFAMemo { get; set; }
        /// <summary>
        /// 《工程可行性研究报告+》批复
        /// </summary>
        public string Point_GCKXXYJBGMemo { get; set; }
        /// <summary>
        /// 《建设用地规划许可证》批复
        /// </summary>
        public string Point_JSYDGHXKZMemo { get; set; }
        /// <summary>
        /// 地勘报告完成
        /// </summary>
        public string Point_DKBGWCMemo { get; set; }
        /// <summary>
        /// 初步设计及概算批复
        /// </summary>
        public string Point_CBSJJGSMemo { get; set; }
        /// <summary>
        /// 施工图编制和审查
        /// </summary>
        public string Point_SGTBZHSCMemo { get; set; }
        /// <summary>
        /// 预算编制完成
        /// </summary>
        public string Point_YSBZWCMemo { get; set; }
        /// <summary>
        /// 《财审控制价》批复
        /// </summary>
        public string Point_CSKZJMemo { get; set; }
        /// <summary>
        /// 施工监理招投标（含预公告）
        /// </summary>
        public string Point_SGJLZTPMemo { get; set; }
        /// <summary>
        /// 项目开工
        /// </summary>
        public string Point_XMKGMemo { get; set; }
        /// <summary>
        /// 《建设工程规划许可证》批复
        /// </summary>
        public string Point_JSGSGHXKZMemo { get; set; }
        /// <summary>
        /// 施工监理人员备案
        /// </summary>
        public string Point_SGJLRYBAMemo { get; set; }
        /// <summary>
        /// 《施工许可证》批复
        /// </summary>
        public string Point_SGXKZMemo { get; set; }
    }
    /// <summary>
    /// 项目节点基类
    /// </summary>
    public class ProjectPointBase
    { 
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid ProjectID { get; set; }
        /// <summary>
        /// 节点ID
        /// </summary>
        public Guid PointID { get; set; }
    }
    /// <summary>
    /// 项目节点计划
    /// </summary>
    public class ProjectPointScheduleNew:ProjectPointBase
    {
        /// <summary>
        /// 计划时间
        /// </summary>
        public DateTime? Schedule { get; set; }
    }
    /// <summary>
    /// 修改项目节点计划
    /// </summary>
    public class ProjetPointScheduleEdit: ProjectPointScheduleNew
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }
    }
    /// <summary>
    /// 项目执行修改
    /// </summary>
    public class ProjectPointExecEdit : ProjectPointExecNew
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get; set; }
    } 
    /// <summary>
    /// 项目节点执行
    /// </summary>
    public class ProjectPointExecNew : ProjectPointBase
    {
        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime? Exec { get; set; }
        /// <summary>
        /// 执行备注
        /// </summary>
        public string ExecMem { get; set; }
    }

    public class ProjectContactNew
    {
        public Guid ProjectID { get; set; }
        /// <summary>
        /// 片区负责人
        /// </summary>
        public string SitePrincipal { get; set; }
        /// <summary>
        /// 片区负责人电话
        /// </summary>
        public string SitePrincipalTEL { get; set; }
        /// <summary>
        /// 片区联系人
        /// </summary>
        public string SiteLink { get; set; }
        /// <summary>
        /// 片区联系人电话
        /// </summary>
        public string SiteLinkTEL { get; set; }
        /// <summary>
        /// 经办人
        /// </summary>
        public string Handler { get; set; }
        /// <summary>
        /// 经办人电话
        /// </summary>
        public string HandlerTEL { get; set; }
        /// <summary>
        /// 分管领导
        /// </summary>
        public string Principal { get; set; }
        /// <summary>
        /// 分管领导电话
        /// </summary>
        public string PrincipalTEL { get; set; }
        /// <summary>
        /// 主要领导
        /// </summary>
        public string Leader { get; set; }
        /// <summary>
        /// 主要领导电话
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
    }
    /// <summary>
    /// 问题发布
    /// </summary>
    public class IssueNew
    {
        public Guid ProjectID { get; set; }
        public string IssueContent { get; set; }
    }
    /// <summary>
    /// 联系人修改
    /// </summary>
    public class ProJCon: ProjectContactNew
    {
       public Guid ID { get; set; }
    }
    /// <summary>
    /// 进度计划修改
    /// </summary>
    public class ProJSch : ProjectScheduleNew
    {
        public Guid ID { get; set; }
    }
    /// <summary>
    /// 问题修改
    /// </summary>
    public class IssueEdit
    {
        public Guid ID { get; set; }
        public Guid ProjectID { get; set; }
        public string IssueContent { get; set; }
    }
    /// <summary>
    /// 后续计划修改
    /// </summary>
    public class AfterEdit
    {
        public Guid ID { get; set; }
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
        ///// <summary>
        ///// 季度
        ///// </summary>
        //public PublicEnum.QuarterState Quarter { get; set; }
        ///// <summary>
        ///// 季度投资
        ///// </summary>
        //public decimal? QInvest { get; set; }
        ///// <summary>
        ///// 季度末形象进度
        ///// </summary>
        //public string QMemo { get; set; }
    }
    /// <summary>
    /// 计划执行保存
    /// </summary>
    public class ScheduleEdit
    {
        public Guid ID { get; set; }
        /// <summary>
        /// 工程项目ID
        /// </summary>
        public Guid ProjectID { get; set; }
        /// <summary>
        /// 枚举
        /// </summary>
        public PublicEnum.PlanEnd Quarter { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public DateTime? Value { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
    }
    /// <summary>
    /// 基本信息修改
    /// </summary>
    public class ProjEdit
    {
        public Guid ID { get; set; }
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
    }
}
