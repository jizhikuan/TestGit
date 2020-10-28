using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using Autofac;
namespace t6
{
    public class Class11
    {
        public static void Index(Test1 db)
        {

            // AutofacRegister.
            //Test1 t = new Test1();
            /*
            
            t.Table_1.Add( new Table_1() { name="sss", address="sdfsdf", sex = true, type=1 });
            await t.SaveChangesAsync();
            */
              var db11= AutofacRegister.Container.Resolve<Test1>();
            var data = db.Table_1.Where(i => i.id == 1).FirstOrDefault();
            /**/
            var is1 = db11 == db; //object.Equals(db1, db);
          
        }
    }
}