﻿using Microsoft.AspNetCore.Mvc;

namespace Accounts.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}