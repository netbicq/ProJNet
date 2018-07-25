using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProJ.Model
{
    /// <summary>
    /// 分页器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pager<T>
    {

        /// <summary>
        /// 数据集合
        /// </summary>
        public IEnumerable<T> Data { get; set; }
        /// <summary>
        /// 可用总页数
        /// </summary>
        public int Pages { get; set; }
        /// <summary>
        /// 记录总条数
        /// </summary>
        public int Items { get; set; }
        /// <summary>
        /// Excel导出结果
        /// </summary>
        public string ExcelResult { get; set; }

        /// <summary>
        /// 获取当前页数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pagesize"></param>
        /// <param name="pageindex"></param>
        /// <returns></returns>
        public Pager<T> GetCurrentPage(IEnumerable<T> source, int pagesize, int pageindex)
        {
            if (pagesize == 0)
            {
                return new Pager<T>()
                {
                    Data = source,
                    Items = 0,
                    Pages = 0
                };
            }
            else
            {
                return new Pager<T>()
                {
                    Data = source.Skip(pagesize * pageindex).Take(pagesize),
                    Items = source.Count(),
                    Pages = (int)Math.Ceiling(((decimal)Items / pagesize)) > 1 ? (int)Math.Ceiling(((decimal)Items)) : 1
                };
            }

        }

    }

    public class PagerQuery<T>
    {
        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 页面索引
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord { get; set; }
        /// <summary>
        /// 参数类
        /// </summary>
        public T Query { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string OrderString { get; set; }

        /// <summary>
        /// 导出excel
        /// </summary>
        public bool ToExcel { get; set; }

    }
}
