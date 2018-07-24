using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ProJ.Model.DB;

namespace ProJ.ORM
{
    public class Unitwork : IUnitwork
    {

        private Dictionary<string, object> repositorys = null;

        private string errmsg = null;

        private string connstr = null;
        private DbContext _dbcontext = null;


        public Unitwork(DbContext context)
        {
            if (context != null)
            {
                _dbcontext = context;// new dbContext(connstr);
            }


        }

        public int Commit()
        {
            try
            {

                return _dbcontext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        errmsg += string.Format("Property: {0} Error: {1}",
                           validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                    }
                }
                throw new Exception(errmsg, dbEx);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {

                if (_dbcontext != null)
                {
                    _dbcontext.Dispose();
                }

            }
        }
        public int ExcuteSQLNoQuery(string sql, SqlParameter[] Parameters = null)
        {
            SqlConnection conn = _dbcontext.Database.Connection as SqlConnection;

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {

                cmd.CommandType = CommandType.Text;

                if (Parameters != null)
                {
                    cmd.Parameters.AddRange(Parameters);
                }
                return cmd.ExecuteNonQuery();

            }
        }

        public dynamic ExecProcedre(string ProcedreName, object parameter = null)
        {
            SqlConnection coonn = _dbcontext.Database.Connection as SqlConnection;

            return coonn.QueryMultiple(ProcedreName, parameter);
        }

        public object ExecProcedreResult(string sql, object parameters = null)
        {
            SqlConnection conn = _dbcontext.Database.Connection as SqlConnection;

            return conn.QueryMultiple(sql, parameters);
        }

        public DataSet ExecuteSQL(string sql, SqlParameter[] Parameters = null)
        {
            DataSet re = new DataSet();
            SqlConnection conn = _dbcontext.Database.Connection as SqlConnection;
            SqlDataAdapter dap = new SqlDataAdapter();


            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {

                cmd.CommandType = CommandType.Text;

                if (Parameters != null)
                {
                    cmd.Parameters.AddRange(Parameters);
                }
                dap.SelectCommand = cmd;
                dap.Fill(re);
                return re;
            }
        }

        public IRepository<T> Repository<T>() where T : ModelBase
        {
            if (repositorys == null)
            {
                repositorys = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;
            if (!repositorys.ContainsKey(type))
            {
                var repositoryType = typeof(RepositoryEF<>);
                var repository = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _dbcontext);
                repositorys.Add(type, repository);
            }
            return (RepositoryEF<T>)repositorys[type];
        }
    }
}
