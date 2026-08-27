using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tp_escape.Models;

namespace tp_escape.Controllers;

public class EscapeController : Controller
{
        public IActionResult Index()
    {
        return View();
    }
}