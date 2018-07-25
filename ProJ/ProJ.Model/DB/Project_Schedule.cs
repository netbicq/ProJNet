using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.DB
{
    /// <summary>
    /// 项目日程
    /// </summary>
    public class Project_Schedule:ModelBase
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public Guid ProjectID { get; set; }
        /// <summary>
        /// 日程类型 来自词典
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
}
