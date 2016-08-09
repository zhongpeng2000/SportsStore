using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        //// GET: Nav
        //public ActionResult Index()
        //{
        //    return View();
        //}
        private IProductRepository repository;

        public NavController(IProductRepository repo)
        {
            repository = repo;
        }



        public PartialViewResult Menu(string category=null)
        {
            // can pass multiple model by ViewBag
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x);
            return PartialView(categories);
        }
    }
}