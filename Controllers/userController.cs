using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vinci_novo.Models;

namespace vinci_novo.Controllers
{
    public class userController : Controller
    {
        vinci_novoEntities db = new vinci_novoEntities();
        public ActionResult Index()
        {
            var c = db.users.ToList();
            return View(c);
        }

        public ActionResult Create()
        {
            var c = new users();
            return View(c);
        }


        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Create(users consulta, HttpPostedFileBase file)
        {
            consulta.created = DateTime.Now;
            consulta.updated = DateTime.Now;
            consulta.type = Request.Form["type"];
            consulta.status = 1;
            string pwCrypto = System.Web.Helpers.Crypto.HashPassword(Request.Form["password"].ToString());
            consulta.password = pwCrypto;
            if (ModelState.IsValid)
            {
                //Save Post                
                db.users.Add(consulta);
                db.SaveChanges();
                TempData["acao"] = "Dados inseridos com sucesso";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var c = db.users.Single(a => a.id == id);
            return View(c);
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateInput(false)]
        public ActionResult Edit(int id, HttpPostedFileBase file)
        {
            var c = db.users.Single(a => a.id == id);
            c.updated = DateTime.Now;
            c.type = Request.Form["type"];
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
                var consulta = db.users.Single(a => a.id == id);
                db.users.Remove(consulta);
                db.SaveChanges();
                TempData["acao"] = "Dados removidos com sucesso";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult EditPw(FormCollection collection)
        {
            int id = Convert.ToInt32(Request.Form["iduser"]);
            var ct = db.users.Find(id);
            ct.updated = DateTime.Now;
            string pwCrypto = System.Web.Helpers.Crypto.HashPassword(Request.Form["PasswordEdit"].ToString());
            ct.password = pwCrypto;
            if (TryUpdateModel(ct))
            {
                db.SaveChanges();
            }
            else
            {
                return View(ct);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Login(users consulta)
        {
            //Crypto.VerifyHashedPassword(us.senha, consulta.senha)
            consulta.email = Request.Form["email"];
            consulta.password = Request.Form["password"];
            try
            {
                var consultauser = (from us in db.users where us.email == consulta.email where us.status == 1 select us).Single();
                if (consultauser != null)
                {
                    if (System.Web.Helpers.Crypto.VerifyHashedPassword(consultauser.password, consulta.password))
                    {
                        HttpCookie cookie = new HttpCookie("admVinci");
                        cookie.Value = consultauser.id.ToString();
                        DateTime dtNow = DateTime.Now;
                        TimeSpan tsMinute = new TimeSpan(30, 0, 0, 0);
                        cookie.Expires = dtNow + tsMinute;
                        Response.Cookies.Add(cookie);
                        return RedirectToAction("Index", "user");
                    }
                    else
                    {
                        ViewBag.Erro = "Erro ao fazer login";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Erro = "Erro ao fazer login";
                    return View();
                }
            }
            catch
            {
                ViewBag.Erro = "Erro ao fazer login";
                return View();
            }
        }

        public ActionResult Logout()
        {
            HttpCookie myCookie = new HttpCookie("admVinci");
            myCookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(myCookie);
            return RedirectToAction("Index", "meueditor");
        }

    }
}