using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model.DB
{
    /// <summary>
    /// 模型基类
    /// </summary>
    public class ModelBase
    {
        private Guid _id = Guid.NewGuid();
        /// <summary>
        /// ID
        /// </summary>
        public Guid ID { get { return _id; } set { _id = value; } }
    }
    /// <summary>
    /// 模型扩展基类
    /// </summary>
    public class ModelBaseEx:ModelBase
    {
        private string _createman = "";
        private DateTime _createdate = DateTime.Now;
        private int _state = 0;
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateMan { get { return _createman; } set { _createman = value; } }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get { return _createdate; } set { _createdate = value; } }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get { return _state; } set { _state = value; } }
    }
}
