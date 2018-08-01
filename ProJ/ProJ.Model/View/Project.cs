using ProJ.Model.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.View
{
    public class ProjectView
    {
        public Project_Info Project_Info { get; set; }
        /// <summary>
        /// 问题
        /// </summary>
        public IEnumerable<Project_Issue> Project_Issue { get; set; }
        public IEnumerable<Project_Log> Project_Log { get; set; }
        /// <summary>
        /// 计划
        /// </summary>
        public IEnumerable<Project_Schedule> Project_Schedule { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public Project_Contacts Project_Contacts { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string StateStr { get; set; }
        /// <summary>
        /// 项目级别
        /// </summary>
        public string ProJLeveStr { get; set; }
        /// <summary>
        /// 项目行业
        /// </summary>
        public string ProjStr { get; set; }
        /// <summary>
        /// 业主单位
        /// </summary>
        public string OwnerStr { get; set; }
        /// <summary>
        /// 是否可修改
        /// </summary>
        public Schss EditTable { get; set; }
        /// <summary>
        /// 变红
        /// </summary>
        public Schss Color { get; set; }
    }
    public class Schss
    {
        /// <summary>
        /// 工程可行性研究报告批复
        /// </summary>
        public bool Point_GCKXXYJBGPF { get; set; }
        /// <summary>
        /// 建设用地规划许可证批复
        /// </summary>
        public bool Point_JSYDGHXKZPF { get; set; }
        /// <summary>
        /// 地勘报告完成
        /// </summary>
        public bool Point_DKBGWC { get; set; }
        /// <summary>
        /// 初步设计及概算批复
        /// </summary>
        public bool Point_CBSJJGSPF { get; set; }
        /// <summary>
        /// 施工图编制和审查
        /// </summary>
        public bool Point_SGTBZHSC { get; set; }
        /// <summary>
        /// 预算编制完成
        /// </summary>
        public bool Point_YSBZWC { get; set; }
        /// <summary>
        /// 财审控制价批复
        /// </summary>
        public bool Point_CSKZJPF { get; set; }
        /// <summary>
        /// 施工监理招投标
        /// </summary>
        public bool Point_SGJLZTP { get; set; }
        /// <summary>
        /// 项目开工
        /// </summary>
        public bool Point_XMKG { get; set; }
        /// <summary>
        /// 建设工程规划许可证批复
        /// </summary>
        public bool Point_JSGCGHXKZPF { get; set; }
        /// <summary>
        /// 施工监理人员备案
        /// </summary>
        public bool Point_SGJLRYBA { get; set; }
        /// <summary>
        /// 施工许可证批复
        /// </summary>
        public bool Point_SGXKZPF { get; set; }
        /// <summary>
        /// 《规划选址及用地意见书》批复
        /// </summary>
        public bool Point_GHXZYDJYJSPF { get; set; }
        /// <summary>
        /// 农转用手续及供地批复
        /// </summary>
        public bool Point_LZYSXJGDPF { get; set; }
        /// <summary>
        /// 土地出让合同
        /// </summary>
        public bool Point_TDCRHT { get; set; }
        /// <summary>
        /// 土地使用权证
        /// </summary>
        public bool Point_TDSYQZ { get; set; }
        /// <summary>
        /// 项目总平设计方案批复
        /// </summary>
        public bool Point_XMZPSJFAPF { get; set; }
    }
}
