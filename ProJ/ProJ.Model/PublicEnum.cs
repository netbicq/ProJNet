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
        /// 词典类型
        /// </summary>
        public enum DictType
        {
            /// <summary>
            /// 项目级别
            /// </summary>
            ProLevel=1,
            /// <summary>
            /// 项目行业
            /// </summary>
            ProIndustry=2
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
    }
}
