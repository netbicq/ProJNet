using ProJ.Model;
using ProJ.Model.DB;
using ProJ.Model.Para;
using ProJ.Model.View;
using ProJ.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Bll
{
    /// <summary>
    /// 词典Service
    /// </summary>
    public class DictService : ServiceBase, IBll.IDict
    {
        private IUnitwork _work = null;

        private IRepository<Basic_Dict> _dict = null;

        public DictService(IUnitwork work)
        {
            _work = work;
            Unitwork = work;

            _dict = work.Repository<Basic_Dict>();

        }
        /// <summary>
        /// 新建词典，同一词典类型不能有想同词典名称
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public ActionResult<bool> AddDict(DictNew dict)
        {
            var dbdevice = new Basic_Dict();
            if (_dict.Any(q => q.DictName == dict.DictName && q.DictType == (int)dict.DictType))
            {
                throw new Exception("同一词典类型不能有相同词典名称");
            }
            dict.Clone(dbdevice);
            dbdevice.State = (int)PublicEnum.GenericState.Normal;
            dbdevice.CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName;
            _dict.Add(dbdevice);
            _work.Commit();

            return new ActionResult<bool>(true);
        }

        /// <summary>
        /// 删除词典，在删除词典前，检查其他地方 里是否存在被删除词典ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult<bool> DelDict(Guid id)
        {
            //if (_emp.Any(q => q.EmployeeType == id))
            //{
            //    throw new Exception("以存在被删除词典");
            //}
            var dict = _dict.Delete(q => q.ID == id);
            return new ActionResult<bool>(dict > 0);
        }
        /// <summary>
        /// 获取指定词典类型的词典列表
        /// </summary>
        /// <param name="dicttype"></param>
        /// <returns></returns>
        public ActionResult<IEnumerable<Basic_Dict>> DictSelector(PublicEnum.DictType dicttype)
        {
            var re = _dict.Queryable(q => q.DictType == (int)dicttype);

            return new ActionResult<IEnumerable<Basic_Dict>>(re);

        }

        /// <summary>
        /// 修改词典 同一词典类型不能有相同词典名称
        /// </summary>
        /// <param name="updater"></param>
        /// <returns></returns>
        public ActionResult<bool> EditDict(EidtDict updater)
        {
            var dict = _dict.GetModel(q => q.ID == updater.ID);
            if (dict == null)
            {
                throw new Exception("词典不存在");
            }
            if (_dict.Any(q => q.DictName == updater.DictName && q.DictType == (int)updater.DictType) && updater.DictName != dict.DictName)
            {
                throw new Exception("同一词典类型不能有相同词典名称");
            }
            updater.Clone(dict);
            _dict.Update(dict);
            _work.Commit();
            return new ActionResult<bool>(true);
        }
        /// <summary>
        /// 获取词典列表
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public ActionResult<Pager<DictView>> GetDictList(PagerQuery<DictQuery> para)
        {
            var retmp = from ac in _dict.GetList(q =>
                        (q.DictType == (int)para.Query.DictType
                        || (int)para.Query.DictType == 0)
                        && (q.DictName.Contains(para.KeyWord)
                        || string.IsNullOrEmpty(para.KeyWord)
                        ))
                        select new Model.View.DictView
                        {
                            DictInfo = ac,
                            StateStr = ac.State == (int)PublicEnum.GenericState.Normal ? "正常" : "未知"
                        };
            var re = new Pager<DictView>().GetCurrentPage(retmp, para.PageSize, para.PageIndex);
            return new ActionResult<Pager<DictView>>(re);
        }
    }
}
