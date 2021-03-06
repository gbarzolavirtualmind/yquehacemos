﻿using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Controllers
{
    public class HomeController : Controller
    {
        private Service service;

        public HomeController()
            : this(new Service(new GoogleRecommendationEngine(), new PlaceRepositoryHarcoded()))
        {

        }

        public HomeController(Service service)
        {
            this.service = service;
        }

        [HttpPost]
        public JsonResult GetPlace(bool[] answers)
        {
            //return Json(1);
            return Json(service.GetPlace(answers));
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }
    }
}

