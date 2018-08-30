using ProJ.IBll;
using ProJ.Model;
using ProJ.Model.DB;
using ProJ.Model.Para;
using ProJ.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Bll
{
    public class Basic_PointService : ServiceBase, IBasic_Point
    {
        private IUnitwork _work = null;

        private IRepository<Model.DB.Basic_Point> _bap = null;

        public Basic_PointService(ORM.IUnitwork work)
        {
            _work = work;
            Unitwork = work;

            _bap = work.Repository<Model.DB.Basic_Point>();

        }
        //新建
        public ActionResult<bool> AddBap(BapNew Bap)
        {
            var dbbap = new Basic_Point();
            if (_bap.Any(q => q.PointName == Bap.PointName))
            {
                throw new Exception("不能有相同名称");
            }
            if (_bap.Any(q => q.ColName == Bap.ColName))
            {
                throw new Exception("不能有相同列名");
            }
            Bap.Clone(dbbap);
            dbbap.State = (int)PublicEnum.GenericState.Normal;
            dbbap.CreateMan = AppUser.CurrentUserInfo.UserProfile.CNName;
            _bap.Add(dbbap);
            _work.Commit();

            return new ActionResult<bool>(true);
        }
        //删除
        public ActionResult<bool> DelBap(Guid id)
        {
            var proj = _work.Repository<Project_Point>();
            if (proj.Any(q => q.PointID == id))
            {
                var mo = proj.Delete(q => q.PointID == id);
            }
            var bap = _bap.Delete(q => q.ID == id);
            return new ActionResult<bool>(bap > 0);
        }
        //修改
        public ActionResult<bool> EditBap(EidtBap updater)
        {
            var bap = _bap.GetModel(q => q.ID == updater.ID);
            if (bap == null)
            {
                throw new Exception("不存在此标准");
            }
            if (_bap.Any(q => q.PointName == updater.PointName && updater.PointName != bap.PointName))
            {
                throw new Exception("不能有相同名称");
            }
            if (_bap.Any(q => q.ColName == updater.ColName && updater.ColName != bap.ColName))
            {
                throw new Exception("不能有相同列名");
            }
            updater.Clone(bap);
            _bap.Update(bap);
            _work.Commit();
            return new ActionResult<bool>(true);
        }
        //列表
        public ActionResult<Pager<BapView>> GetBapList(PagerQuery<BapQuery> para)
        {
            var retmp = from ac in _bap.GetList(q =>
                       (q.PointName.Contains(para.KeyWord)
                       || string.IsNullOrEmpty(para.KeyWord)
                       ))
                        select new Model.Para.BapView
                        {
                             Basic_Point= ac,
                            StateStr = ac.State == (int)PublicEnum.GenericState.Normal ? "正常" : "未知"
                        };
            var re = new Pager<BapView>().GetCurrentPage(retmp, para.PageSize, para.PageIndex);
            return new ActionResult<Pager<BapView>>(re);
        }
    }
}
