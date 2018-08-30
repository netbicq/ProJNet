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
        /// 标题
        /// </summary>
        public string PointName { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public string ColName { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int PointOrderIndex { get; set; }
    }
}
