using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vinci_novo.Models;

namespace vinci_novo.Controllers
{
    public class admNewsController : Controller
    {
        vinci_novoEntities db = new vinci_novoEntities();
        public ActionResult Index()
        {
            var c = db.news.ToList();
            return View(c);
        }

        public ActionResult Create()
        {
            var c = new news();
            ViewBag.categories = db.categories.OrderBy(t => t.titulo).ToList();
            return View(c);
        }


        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Create(news consulta, IEnumerable<HttpPostedFileBase> files1, IEnumerable<HttpPostedFileBase> files2)
        {
            consulta.created = DateTime.Now;
            consulta.updated = DateTime.Now;

            //Preenche o campo Ordem da tabela de Categorias.
            var ultimoitem = new news();
            var consultaOrdem = (from ev in db.news orderby ev.position ascending select ev).ToList();
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
                db.news.Add(consulta);
                db.SaveChanges();
                TempData["acao"] = "Dados inseridos com sucesso";
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (files1.Count() > 0)
                uploadImagem(consulta, files1, "news-img");

            if (files2.Count() > 0)
                uploadImagem(consulta, files2, "news-pdf");

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var c = db.news.Single(a => a.id == id);
            ViewBag.categories = db.categories.OrderBy(t => t.titulo).ToList();
            ViewBag.imgs = db.files.Where(t => t.reference == "news-img" && t.content_id == id).ToList();
            ViewBag.pdfs = db.files.Where(t => t.reference == "news-pdf" && t.content_id == id).ToList();
            return View(c);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(int id, IEnumerable<HttpPostedFileBase> files1, IEnumerable<HttpPostedFileBase> files2)
        {
            var c = db.news.Single(a => a.id == id);
            ViewBag.categories = db.categories.OrderBy(t => t.titulo).ToList();
            c.updated = DateTime.Now;
            var x = Request.Form["highlight"];
            if (x == null)
                c.highlight = null;
            //if (x == "on")
            //{
            //    c.highlight = 1;
            //}
            //else
            //{
            //    c.highlight = 0;
            //}

            if (TryUpdateModel(c))
            {
                db.SaveChanges();
                TempData["acao"] = "Dados alterados com sucesso";
            }
            else
            {
                return View(c);
            }

            if (files1.Count() > 0)
                uploadImagem(c, files1, "news-pdf");

            if (files2.Count() > 0)
                uploadImagem(c, files2, "news-img");

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            try
            {
                var consulta = db.news.Single(a => a.id == id);
                db.news.Remove(consulta);
                db.SaveChanges();
                TempData["acao"] = "Dados removidos com sucesso";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private void uploadImagem(news consulta, IEnumerable<HttpPostedFileBase> files, string page)
        {
            var ultimoitem2 = new files();
            var consulta2 = new files();
            foreach (var file in files)
            {
                if (file != null)
                {
                    var consultaOrdem2 = (from ev in db.files where ev.content_id == consulta.id && ev.reference == page orderby ev.position ascending select ev).ToList();

                    if (consultaOrdem2.Count() > 0)
                    {
                        ultimoitem2 = consultaOrdem2[(consultaOrdem2.Count()) - 1];
                        consulta2.position = ultimoitem2.position + 1;
                    }
                    else
                        consulta2.position = 1;

                    int id = Convert.ToInt32(consulta.id);
                    //nome do arquivo original
                    var fileName = Path.GetFileName(file.FileName);

                    //atribuicao da extensão do arquivo (.jpg / .gif) na variavel
                    var extensao = Path.GetExtension(fileName);

                    //geracao de numeros randômicos que serao o novo nome do arquivo
                    Random randNum = new Random();
                    var random = randNum.Next().ToString();

                    //mapeamento do caminho absoluto onde ficarão os arquivos
                    string dir = (Server.MapPath("~/uploads/"));

                    //verifica se o diretorio referente ao determinado conteudo existe. caso não exista, cria o diretorio.
                    if (System.IO.Directory.Exists(dir) == false)
                    {
                        System.IO.Directory.CreateDirectory(dir);
                    }

                    var path = Path.Combine(Server.MapPath("~/uploads/"), random + extensao);
                    file.SaveAs(path);
                    consulta2.filename = random + extensao;
                    consulta2.status = 1;
                    consulta2.content_id = consulta.id;
                    consulta2.updated = DateTime.Now;
                    consulta2.created = DateTime.Now;
                    consulta2.reference = page;
                    //consulta2.tipo = "imagem";
                    try
                    {
                        db.files.Add(consulta2);
                        db.SaveChanges();
                    }
                    catch
                    {

                    }
                }
            }
        }

        public ActionResult DeleteImg(int id)
        {
            var consulta = db.files.Single(a => a.id == id);
            db.files.Remove(consulta);
            db.SaveChanges();
            TempData["acao"] = "Arquivo removido com sucesso";
            return RedirectToAction("Edit", "admNews", new { id = consulta.content_id });
        }
    }
}