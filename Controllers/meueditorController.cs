using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using vinci_novo.Models;

namespace vinci_novo.Controllers
{
    public class meueditorController : Controller
    {

        vinci_novoEntities db = new vinci_novoEntities();
        // GET: meueditor
        public ActionResult Index()
        {
            HttpCookie admVinciCookie = Request.Cookies["admVinci"];
            if (admVinciCookie != null)
            {
                Response.Redirect(Url.Action("Index", "user"));
            }
            else
            {
                return View();
            }
            return View();
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
                        return RedirectToAction("Index", "meueditor");
                    }
                }
                else
                {
                    ViewBag.Erro = "Erro ao fazer login";
                    return RedirectToAction("Index", "meueditor");
                }
            }
            catch
            {
                ViewBag.Erro = "Erro ao fazer login";
                return RedirectToAction("Index", "meueditor");
            }
        }
    }
}