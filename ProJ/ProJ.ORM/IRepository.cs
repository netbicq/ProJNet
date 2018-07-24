using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ProJ.Model.DB;

namespace ProJ.ORM
{
    public interface IRepository<T> where T:ModelBase
    {


        /// <summary>
        /// 批量新建
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        IEnumerable<T> Add(IEnumerable<T> entitys);
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Add(T entity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQueryable<T> Queryable(Expression<Func<T, bool>> predicate = null);
        /// <summary>
        /// 批量删除指定的实体集合
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        int Delete(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Delete(T entity);

        /// <summary>
        /// 修改指定的实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Update(T entity);

        /// <summary>
        /// 修改条件内数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="updater"></param>
        /// <returns></returns>

        int Update(Expression<Func<T, bool>> predicate, Expression<Func<T, T>> updater);
        /// <summary>
        /// 获取指定ID的实体
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        T GetModel(Guid key);
        /// <summary>
        /// 获取条件内实体，如果多条则返回第一条
        /// </summar
        /// <param name="predicate"></param>
        /// <returns></returns>
        T GetModel(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 获取和件内实体集合，条件为空则返回所有
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<T> GetList(Expression<Func<T, bool>> predicate = null);
        /// <summary>
        /// 条件内是否存在数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>

        bool Any(Expression<Func<T, bool>> predicate = null);
        /// <summary>
        /// 指定ID是否存在实体
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Any(Guid key);

    }
}
