using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vinci_novo.Models;

namespace vinci_novo.Controllers
{
    public class admTxtController : Controller
    {
        vinci_novoEntities db = new vinci_novoEntities();
        public ActionResult Index(string page, string title = "")
        {
            try
            {
                ViewBag.txt1 = db.txts.Where(u => u.page == page).Select(u => u.txt1).FirstOrDefault();
            }
            catch
            {
                ViewBag.txt1 = "";
            }
            return View();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Index(string page, IEnumerable<HttpPostedFileBase> files, string title = "")
        {
            try
            {
                var consulta = db.txts.Single(a => a.page == page);
                consulta.txt1 = Request.Form["txt1"];
                if (TryUpdateModel(consulta))
                {
                    db.SaveChanges();
                    TempData["acaotitle"] = "Dados alterados com sucesso";
                }
                else
                {
                    return View(consulta);
                }
                return RedirectToAction("Index", "admTxt", new { page = page, title = title });
            }
            catch
            {
                var c = new txts();
                c.txt1 = Request.Form["txt1"];
                c.page = page;
                c.status = 1;
                if (ModelState.IsValid)
                {
                    //Save Post                
                    db.txts.Add(c);
                    db.SaveChanges();
                    TempData["acaotitle"] = "Dados alterados com sucesso";
                }
                return RedirectToAction("Index", "admTxt", new { page = page, title = title });
            }
        }
    }
}