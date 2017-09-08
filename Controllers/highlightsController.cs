using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vinci_novo.Models;

namespace vinci_novo.Controllers
{
    public class highlightsController : Controller
    {

        vinci_novoEntities db = new vinci_novoEntities();
        // GET: highlights
        public ActionResult Index(int order, string page)
        {
            var consulta = db.highlights.Single(a => a.position == order && a.page == page);
            ViewBag.imgs = db.files.Where(t => t.content_id == consulta.id).ToList();
            return View(consulta);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Index(int id, int order, string page, IEnumerable<HttpPostedFileBase> files)
        {
            var consulta = db.highlights.Single(a => a.id == id);
            consulta.txt1 = Request.Form["txt1"];
            consulta.txt2 = Request.Form["txt2"];
            consulta.txt3 = Request.Form["txt3"];
            consulta.link = Request.Form["link"];
            consulta.opt = Request.Form["customRadio"];
            consulta.updated = DateTime.Now;
            consulta.status = 1;
            //Cadastra a Categoria.
            if (TryUpdateModel(consulta))
            {
                db.SaveChanges();
                TempData["acao"] = "Dados alterados com sucesso";
            }

            if (files.Count() > 0)
                uploadImagem(consulta, files, page);

            return RedirectToAction("Index", "highlights", new { order = order, page = page });
        }

        private void uploadImagem(highlights consulta, IEnumerable<HttpPostedFileBase> files, string page)
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

        public ActionResult DeleteImg(int id, int order, string page)
        {
            var consulta = db.files.Single(a => a.id == id);
            db.files.Remove(consulta);
            db.SaveChanges();
            TempData["acao"] = "Imagem removida com sucesso";
            return RedirectToAction("Index", "highlights", new { order = order, page = page });
        }
    }
}