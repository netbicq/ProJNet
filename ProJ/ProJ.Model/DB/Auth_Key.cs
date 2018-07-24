using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.DB
{
    /// <summary>
    /// 权限key
    /// </summary>
    public class Auth_Key : ModelBase
    {
        /// <summary>
        /// 权限KEY
        /// </summary>
        public string AuthKey { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        public string FunctionName { get; set; }
        /// <summary>
        /// web路由url
        /// </summary>
        public string RoutUrl { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderIndex { get; set; }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string IMGUrl { get; set; }
    }
}
