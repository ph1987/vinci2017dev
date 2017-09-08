using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vinci_novo.Models;

namespace vinci_novo.Controllers
{
    public class numbersController : Controller
    {
        vinci_novoEntities db = new vinci_novoEntities();
        public ActionResult Index(string page)
        {
            var consulta = db.numbers.Where(t => t.page == page).ToList();
            try
            {
                ViewBag.txtTitle = db.txts.Where(u => u.page == page).Select(u => u.txt1).FirstOrDefault();
            }
            catch
            {
                ViewBag.txtTitle = "";
            }
            return View(consulta);
        }

        public ActionResult Create()
        {
            var c = new numbers();
            return View(c);
        }


        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Create(numbers consulta, HttpPostedFileBase file, string page)
        {
            consulta.created = DateTime.Now;
            consulta.updated = DateTime.Now;
            consulta.bigNumber = Request.Form["bigNumber"];
            consulta.suffix = Request.Form["suffix"];
            consulta.txt = Request.Form["txt"];
            consulta.status = 1;
            consulta.page = page;
            consulta.bigNumberBold = Convert.ToInt32(Request.Form["bigNumberBold"]);
            consulta.suffixBold = Convert.ToInt32(Request.Form["suffixBold"]);
            consulta.txtBold = Convert.ToInt32(Request.Form["txtBold"]);

            var ultimoitem = new numbers();
            var consultaOrdem = (from ev in db.numbers orderby ev.position ascending select ev).ToList();
            if (consultaOrdem.Count() > 0)
            {
                ultimoitem = consultaOrdem[consultaOrdem.Count - 1];
                consulta.position = ultimoitem.position + 1;
            }
            else
                consulta.position = 1;

            //Cadastra a Categoria.
            if (ModelState.IsValid)
            {
                //Save Post                
                db.numbers.Add(consulta);
                db.SaveChanges();
                TempData["acao"] = "Dados inseridos com sucesso";
            }

            return RedirectToAction("Index", "numbers", new { page = page });
        }

        public ActionResult Edit(int id, string page)
        {
            var c = db.numbers.Single(a => a.id == id);
            return View(c);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(int id, HttpPostedFileBase file, string page)
        {
            var consulta = db.numbers.Single(a => a.id == id);
            consulta.updated = DateTime.Now;
            consulta.bigNumber = Request.Form["bigNumber"];
            consulta.suffix = Request.Form["suffix"];
            consulta.txt = Request.Form["txt"];
            consulta.page = page;
            consulta.bigNumberBold = Convert.ToInt32(Request.Form["bigNumberBold"]);
            consulta.suffixBold = Convert.ToInt32(Request.Form["suffixBold"]);
            consulta.txtBold = Convert.ToInt32(Request.Form["txtBold"]);
            if (TryUpdateModel(consulta))
            {
                db.SaveChanges();
                TempData["acao"] = "Dados alterados com sucesso";
            }
            else
            {
                return View(consulta);
            }
            return RedirectToAction("Index", "numbers", new { page = page });
        }

        public ActionResult Delete(int id, string page)
        {
            try
            {
                var consulta = db.numbers.Single(a => a.id == id);
                db.numbers.Remove(consulta);
                db.SaveChanges();
                TempData["acao"] = "Dados removidos com sucesso";
                return RedirectToAction("Index", "numbers", new { page = page });
            }
            catch
            {
                return RedirectToAction("Index", "numbers", new { page = page });
            }
        }

        public ActionResult editTitle(string page)
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
                return RedirectToAction("Index", "numbers", new { page = page });
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
                return RedirectToAction("Index", "numbers", new { page = page });
            }
        }
    }
}