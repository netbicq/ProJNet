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
    /// Service
    /// </summary>
    public class TeamService : ServiceBase, IBll.ITeam
    {
        private IUnitwork _work = null;

        private IRepository<Proiect_Teamwork> _team = null;
        private IRepository<Project_Enclosure> _enc = null;

        public TeamService(IUnitwork work)
        {
            _work = work;
            Unitwork = work;

            _team = work.Repository<Proiect_Teamwork>();
            _enc = work.Repository<Project_Enclosure>();

        }

        public ActionResult<bool> Addteam(TeamNew para)
        {
            var id = Guid.NewGuid();
            var tem = new Proiect_Teamwork();
            para.Clone(tem);
            tem.ID = id;
            _team.Add(tem);
            foreach (var item in para.Teams)
            {
                var en = new Project_Enclosure();
                en.TeamID = id;
                en.Enclosure = item;
                _enc.Add(en);
            }
            _work.Commit();

            return new ActionResult<bool>(true);
        }

        public ActionResult<bool> Delteam(Guid id)
        {
            var team = _team.Delete(q => q.ID == id);
            if (team>0)
            {
                var enc = _enc.Delete(q => q.TeamID == id);
            }
            return new ActionResult<bool>(team > 0);
        }

        public ActionResult<Pager<TemaView>> GetteamList(PagerQuery<TimeQuery> para)
        {
            var retmp = from ac in _team.GetList()
                        let enc=_enc.GetList(q=>q.TeamID==ac.ID).Select(s=>s.Enclosure)
                        select new Model.View.TemaView
                        {
                            Proiect_Teamwork = ac,
                            Encolos= enc
                        };
            var re = new Pager<TemaView>().GetCurrentPage(retmp, para.PageSize, para.PageIndex);
            return new ActionResult<Pager<TemaView>>(re);
        }
    }
    }
