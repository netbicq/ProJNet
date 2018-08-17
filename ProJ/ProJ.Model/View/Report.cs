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
}
