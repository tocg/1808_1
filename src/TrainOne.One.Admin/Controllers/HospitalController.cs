using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainOne.One.Admin.Controllers
{
    public class HospitalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
