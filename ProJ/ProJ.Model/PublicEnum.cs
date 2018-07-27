using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model
{
    public class PublicEnum
    {
        /// <summary>
        /// 计划执行
        /// </summary>
        public enum PlanEnd
        {
            /// <summary>
            /// 工程可行性研究报告批复
            /// </summary>
            Point_GCKXXYJBGPF = 1,
            /// <summary>
            /// 建设用地规划许可证批复
            /// </summary>
            Point_JSYDGHXKZPF = 2,
            /// <summary>
            /// 地勘报告完成
            /// </summary>
            Point_DKBGWC = 3,
            /// <summary>
            /// 初步设计及概算批复
            /// </summary>
            Point_CBSJJGSPF = 4,
            /// <summary>
            /// 施工图编制和审查
            /// </summary>
            Point_SGTBZHSC = 5,
            /// <summary>
            /// 预算编制完成
            /// </summary>
            Point_YSBZWC = 6,
            /// <summary>
            /// 财审控制价批复
            /// </summary>
            Point_CSKZJPF = 7,
            /// <summary>
            /// 施工监理招投标
            /// </summary>
            Point_SGJLZTP = 8,
            /// <summary>
            /// 项目开工
            /// </summary>
            Point_XMKG = 9,
            /// <summary>
            /// 建设工程规划许可证批复
            /// </summary>
            Point_JSGCGHXKZPF = 10,
            /// <summary>
            /// 施工监理人员备案
            /// </summary>
            Point_SGJLRYBA = 11,
            /// <summary>
            /// 施工许可证批复
            /// </summary>
            Point_SGXKZPF = 12,
            /// <summary>
            /// 《规划选址及用地意见书》批复
            /// </summary>
            Point_GHXZYDJYJSPF = 13,
            /// <summary>
            /// 农转用手续及供地批复
            /// </summary>
            Point_LZYSXJGDPF = 14,
            /// <summary>
            /// 土地出让合同
            /// </summary>
            Point_TDCRHT = 15,
            /// <summary>
            /// 土地使用权证
            /// </summary>
            Point_TDSYQZ = 16,
            /// <summary>
            /// 项目总平设计方案批复
            /// </summary>
            Point_XMZPSJFAPF = 17
        }
        /// <summary>
        /// 计划类型
        /// </summary>
        public enum PlanType
        {
            /// <summary>
            /// 计划
            /// </summary>
            Plan = 1,
            /// <summary>
            /// 执行
            /// </summary>
            Ement = 2
        }
        /// <summary>
        /// 词典类型
        /// </summary>
        public enum DictType
        {
            /// <summary>
            /// 项目级别
            /// </summary>
            ProLevel = 1,
            /// <summary>
            /// 项目行业
            /// </summary>
            ProIndustry = 2
        }
        /// <summary>
        /// 一般状态枚举
        /// </summary>
        public enum GenericState
        {
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 1,
            /// <summary>
            /// 取消，作废
            /// </summary>
            Cancel = 2
        }
        /// <summary>
        /// 工程项目状态枚举
        /// </summary>
        public enum ProjState
        {
            /// <summary>
            /// 正常
            /// </summary>
            Normal = 1,
            /// <summary>
            /// 申请
            /// </summary>
            Apply = 2,
            /// <summary>
            /// 待修改
            /// </summary>
            Modified = 3,
            /// <summary>
            /// 开工
            /// </summary>
            Start = 4
        }
    }
}
