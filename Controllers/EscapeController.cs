using System.Diagnostics;
using System;
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
    public IActionResult Tutorial()
    {
        return View();
    }
    public IActionResult Integrantes()
    {
        return View();
    }

    public IActionResult FijarseNombreDeUsuario (string nombreUsuario)
    {
        if (bd.ExisteUsuario(nombreUsuario) == true)
        {
            int salaActual = bd.ObtenerSalaActual(nombreUsuario);
            ViewBag.NombreUsuario = nombreUsuario;
            return RedirectToAction("Nivel" + salaActual);
        }
        else
        {
            bd.CrearPartida(nombreUsuario, 1);
            ViewBag.NombreUsuario = nombreUsuario;
            return RedirectToAction("Nivel1");
        }

    }

    public IActionResult FijarRespuesta(string respuesta , int sala , string nombreUsuario)
    {
        if (bd.VerificarRespuesta(sala, respuesta) == true)
        {
            bd.ActualizarSalaActual(sala + 1, nombreUsuario);
            ViewBag.NombreUsuario = nombreUsuario;
            return RedirectToAction("Nivel" + (sala + 1));
        }
        else
        {
            ViewBag.NombreUsuario = nombreUsuario;
            ViewBag.Error = "Respuesta incorrecta. Intenta de nuevo.";
            return RedirectToAction("Nivel" + sala);
        }
    }

    
    


}

