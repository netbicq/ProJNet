using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProJ.Model.DB;

namespace ProJ.Bll
{
    /// <summary>
    /// 服务基类
    /// </summary>
    public class ServiceBase
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        public AppServiceUser AppUser { get; set; }
        /// <summary>
        /// Unitwork
        /// </summary>
        public ORM.IUnitwork Unitwork { get; set; }
    }
    /// <summary>
    /// 当前用户
    /// </summary>
    public class AppServiceUser
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public Auth_User UserInfo { get; set; }

        /// <summary>
        /// 上传文件路径
        /// </summary>
        public string UploadPath { get; set; }
        /// <summary>
        /// 导出文件路径
        /// </summary>
        public string OutPutPaht { get; set; }
    }
}
