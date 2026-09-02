using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tp_escape.Models;

namespace tp_escape.Controllers;

public class EscapeController : Controller
{
    public BD bd = new BD();
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult FijarseNombreDeUsuario (string nombreUsuario)
    {
        if (bd.ExisteUsuario(nombreUsuario) == true)
        {
            int salaActual = bd.ObtenerSalaActual(nombreUsuario);
            return RedirectToAction("Sala" + salaActual, new { nombreUsuario = nombreUsuario });
        }
        else
        {
            bd.CrearPartida(nombreUsuario, 1);
            return RedirectToAction("Sala1", new { nombreUsuario = nombreUsuario });
        }

    }

    
}

