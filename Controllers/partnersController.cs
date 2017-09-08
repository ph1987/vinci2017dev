using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vinci_novo.Models;

namespace vinci_novo.Controllers
{
    public class partnersController : Controller
    {
        vinci_novoEntities db = new vinci_novoEntities();
        // GET: partners
        public ActionResult Index(string page)
        {
            var consulta = db.partners.Where(t=>t.page == page).ToList();
            return View(consulta);
        }

        public ActionResult Create()
        {
            var c = new partners();
            return View(c);
        }


        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Create(partners consulta, HttpPostedFileBase file, string page)
        {
            consulta.created = DateTime.Now;
            consulta.updated = DateTime.Now;
            consulta.status = 1;
            consulta.page = page;

            var ultimoitem = new partners();
            var consultaOrdem = (from ev in db.partners where ev.page == page orderby ev.position ascending select ev).ToList();
            if (consultaOrdem.Count() > 0)
            {
                ultimoitem = consultaOrdem[consultaOrdem.Count - 1];
                consulta.position = ultimoitem.position + 1;
            }
            else
                consulta.position = 1;

            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var extensao = Path.GetExtension(fileName);
                Random randNum = new Random();
                var random = randNum.Next().ToString();
                string dir = (Server.MapPath("~/uploads/"));
                if (System.IO.Directory.Exists(dir) == false)
                {
                    System.IO.Directory.CreateDirectory(dir);
                }

                var path = Path.Combine(Server.MapPath("~/uploads/"), random + extensao);
                file.SaveAs(path);
                consulta.img = random + extensao;
            }

            //Cadastra a Categoria.
            if (ModelState.IsValid)
            {
                //Save Post                
                db.partners.Add(consulta);
                db.SaveChanges();
                TempData["acao"] = "Dados inseridos com sucesso";
            }

            return RedirectToAction("Index", "partners", new { page = page });
        }

        public ActionResult Edit(int id, string page)
        {
            var c = db.partners.Single(a => a.id == id);
            return View(c);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(int id, HttpPostedFileBase file, string page)
        {
            var consulta = db.partners.Single(a => a.id == id);
            consulta.updated = DateTime.Now;
            consulta.page = page;

            if (file != null)
            {
                var fileName = Path.GetFileName(file.FileName);
                var extensao = Path.GetExtension(fileName);
                Random randNum = new Random();
                var random = randNum.Next().ToString();
                string dir = (Server.MapPath("~/uploads/"));
                if (System.IO.Directory.Exists(dir) == false)
                {
                    System.IO.Directory.CreateDirectory(dir);
                }

                var path = Path.Combine(Server.MapPath("~/uploads/"), random + extensao);
                file.SaveAs(path);
                consulta.img = random + extensao;
            }

            if (TryUpdateModel(consulta))
            {
                db.SaveChanges();
                TempData["acao"] = "Dados alterados com sucesso";
            }
            else
            {
                TempData["acao"] = "Erro ao alterar o conteudo";
                return View(consulta);
            }
            return RedirectToAction("Index", "partners", new { page = page });
        }

        public ActionResult Delete(int id, string page)
        {
            try
            {
                var consulta = db.partners.Single(a => a.id == id);
                db.partners.Remove(consulta);
                db.SaveChanges();
                TempData["acao"] = "Dados removidos com sucesso";
                return RedirectToAction("Index", "partners", new { page = page });
            }
            catch
            {
                TempData["acao"] = "Não foi possível remover o conteudo";
                return RedirectToAction("Index", "partners", new { page = page });
            }
        }

        public ActionResult DeleteImg(int id, string page)
        {
            var consulta = db.partners.Single(a => a.id == id);
            consulta.img = null;
            if (TryUpdateModel(consulta))
            {
                db.SaveChanges();
                TempData["acao"] = "Imagem removida com sucesso";
            }
            return RedirectToAction("Edit", "partners", new { id = id, page = page });
        }
    }
}