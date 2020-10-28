using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity;
using Autofac;
using Autofac.Integration.Mvc;

namespace t6.Controllers
{
    public class HomeController : Controller
    {
 
        public Test1 db;
        public HomeController(Test1 db1)
        {
            db = db1;
        }
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public async Task<ActionResult> Index(Test1 db2)
        {

            // AutofacRegister.
            //Test1 t = new Test1();
            /*
            
            t.Table_1.Add( new Table_1() { name="sss", address="sdfsdf", sex = true, type=1 });
            await t.SaveChangesAsync();
            */

            var db11 = AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<Test1>(); //AutofacRegister.Container.Resolve<Test1>();//AutofacDependencyResolver.Current.RequestLifetimeScope.Resolve<Test1>(); //;
            var is1 = db == db2;
            var is2 = db == db11;
            var item = from i in db.Table_1 where i.name == "sss" select i.name;
            var list = item.ToList();
            return View(); 

        }

        public ActionResult About(Test1 db)
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}