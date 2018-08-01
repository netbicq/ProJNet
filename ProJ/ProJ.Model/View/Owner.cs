using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.View
{
    public class OwnerView
    {
        /// <summary>
        /// 业主信息
        /// </summary>
        public Model.DB.Basic_Owner OwnerInfo { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string StateStr { get; set; }
    }
}
