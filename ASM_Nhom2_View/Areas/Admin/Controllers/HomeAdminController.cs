﻿using ASM_Nhom2_View.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components;

namespace ASM_Nhom2_View.Areas.Admin.Controllers
{
    //[Area("Admin")]
    public class HomeAdminController : Controller
    {
        //[Microsoft.AspNetCore.Mvc.Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
