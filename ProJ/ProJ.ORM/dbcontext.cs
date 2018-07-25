using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProJ.Model.DB;

namespace ProJ.ORM
{
    public class dbcontext:DbContext
    {

        public dbcontext() : base("dbconn")
        {

        }

        public virtual DbSet<Auth_Key> Auth_Key { get; set; }

        public virtual DbSet<Auth_KeyDetail> Auth_KeyDetail { get; set; }

        public virtual DbSet<Auth_Role> Auth_Role { get; set; }

        public virtual DbSet<Auth_RoleAuthScope> Auth_RoleAuthScope { get; set; }

        public virtual DbSet<Auth_User> Auth_User { get; set; }

        public virtual DbSet<Auth_UserRole> Auth_UserRole { get; set; }

        public virtual DbSet<Basic_Dict> Basic_Dict { get; set; }

        public virtual DbSet<Basic_Owner> Basic_Owner { get; set; }

        public virtual DbSet<Auth_UserProfile> Auth_UserProfile { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }
    }
}
