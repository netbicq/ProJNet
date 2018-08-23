using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.DB
{
    public class Basic_Point : ModelBaseEx
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string PointName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int PointOrderIndex { get; set; }
    }
}
