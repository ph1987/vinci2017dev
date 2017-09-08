using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vinci_novo.Models;
namespace vinci_novo.Controllers
{
    public class categoriesController : Controller
    {
        vinci_novoEntities db = new vinci_novoEntities();
        // GET: categories
        public ActionResult Index()
        {
            var c = db.categories.ToList();
            return View(c);
        }

        public ActionResult Create()
        {
            var c = new categories();
            return View(c);
        }


        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Create(categories consulta, HttpPostedFileBase file)
        {
            consulta.created = DateTime.Now;
            consulta.updated = DateTime.Now;
            //consulta.status = Convert.ToInt32(Request.Form["status"]);

            //Preenche o campo Ordem da tabela de Categorias.
            var ultimoitem = new categories();
            var consultaOrdem = (from ev in db.categories orderby ev.ordem ascending select ev).ToList();
            if (consultaOrdem.Count() > 0)
            {
                ultimoitem = consultaOrdem[consultaOrdem.Count - 1];
                consulta.ordem = ultimoitem.ordem + 1;
            }
            else
                consulta.ordem = 1;

            //Cadastra a Categoria.
            if (ModelState.IsValid)
            {
                //Save Post                
                db.categories.Add(consulta);
                db.SaveChanges();
                TempData["acao"] = "Dados inseridos com sucesso";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var c = db.categories.Single(a => a.id == id);
            return View(c);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(int id, HttpPostedFileBase file)
        {
            var c = db.categories.Single(a => a.id == id);
            c.updated = DateTime.Now;
            if (TryUpdateModel(c))
            {
                db.SaveChanges();
                TempData["acao"] = "Dados alterados com sucesso";
            }
            else
            {
                return View(c);
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var consulta = db.categories.Single(a => a.id == id);
                db.categories.Remove(consulta);
                db.SaveChanges();
                TempData["acao"] = "Dados removidos com sucesso";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}