namespace t6
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Reflection;

    public partial class Test1 : DbContext
    {
        public Test1()
            : base("name=Test1")
        {
        }

        public virtual DbSet<Table_1> Table_1 { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            //modelBuilder.Configurations.Add(new Table1EntityType());
            //modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            // modelBuilder.Configurations.Add<Table1Type>();
        }
    }
}
