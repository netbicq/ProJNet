using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.ORM
{
    public class RepositoryEF<T> : IRepository<T> where T : Model.DB.ModelBase
    {

        internal DbContext _dbcontext = null;
        internal readonly DbSet<T> _dbset = null;
        private string errmessage = string.Empty;

        public RepositoryEF(DbContext db)
        {
            _dbcontext = db;
            _dbset = db.Set<T>();
        }
        /// <summary>
        /// 批量新建
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public IEnumerable<T> Add(IEnumerable<T> entitys)
        {
            return _dbset.AddRange(entitys);
        }
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Add(T entity)
        {
            return _dbset.Add(entity);
        }
        /// <summary>
        /// 是否存在条件内数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Any(Expression<Func<T, bool>> predicate = null)
        {
            return _dbset.Any(predicate);
        }

        /// <summary>
        /// 是否存在Key值数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Any(Guid key)
        {
            return _dbset.Any(q => q.ID == key);
        }
        /// <summary>
        /// 删除条件内数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Delete(Expression<Func<T, bool>> predicate)
        {
            return _dbset.Where(predicate).Delete();
        }
        /// <summary>
        ///删除数据模型
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Delete(T entity)
        {
            return  _dbset.Remove(entity);
        }
        /// <summary>
        /// 返回条件内集合
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<T> GetList(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate == null)
            {
                return _dbset.AsEnumerable();
            }
            else
            {
                return _dbset.Where(predicate);
            }
        }
        /// <summary>
        /// 获取指定ID的模型
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetModel(Guid key)
        {
            return _dbset.FirstOrDefault(q => q.ID == key);
        }
        /// <summary>
        /// 指定条件内的模型
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T GetModel(Expression<Func<T, bool>> predicate)
        {
            return _dbset.Where(predicate).FirstOrDefault();
        }
        /// <summary>
        /// 获取Queryable
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> Queryable(Expression<Func<T, bool>> predicate = null)
        {

            if (predicate != null)
            {
                return _dbset.Where(predicate);
            }
            else
            {
                return _dbset.AsQueryable();
            }
        }

        public void Update(T entity)
        {
            try
            {
                var entry = _dbcontext.Entry(entity);
                _dbset.Attach(entity);
                entry.State = EntityState.Modified;

            }
            catch (DbEntityValidationException dbEx)
            {

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errmessage += string.Format("Property: {0} Error: {1}",
                        validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                    }
                }
                throw new Exception(errmessage, dbEx);
            }
        }

        public int Update(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updater)
        {
            return _dbset.Where(predicate).Update(updater);
        }
    }
}
