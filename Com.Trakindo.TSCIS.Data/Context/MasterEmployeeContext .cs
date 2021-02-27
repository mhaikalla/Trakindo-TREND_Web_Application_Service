namespace Com.Trakindo.TSICS.Data.Context
{
    using Com.Trakindo.TSICS.Data.Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public partial class MasterEmployeeContext : DbContext
    {
        public MasterEmployeeContext()
            : base("name=dbEmployeeMaster ")
        {
        }

        public virtual DbSet<EmployeeMaster> EmployeeMaster { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Customer>()
            //    .Property(e => e.PAR)
            //    .IsUnicode(false);
        }
    }
}
