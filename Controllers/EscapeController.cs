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

    public IActionResult FijarseNombreDeUsuario (string nombreUsuario)
    {
        if (bd.ExisteUsuario(nombreUsuario) == true)
        {
            int salaActual = bd.ObtenerSalaActual(nombreUsuario);
            return RedirectToAction("Nivel" + salaActual, new { nombreUsuario = nombreUsuario });
        }
        else
        {
            bd.CrearPartida(nombreUsuario, 1);
            return RedirectToAction("Nivel1", new { nombreUsuario = nombreUsuario });
        }

    }

    public IActionResult FijarRespuesta(string nombreUsuario, string respuesta, int sala)
    {
        // Obtener el código esperado para la sala
        var codigo = bd.ObtenerCodigoSala(sala);
        bool correcto = string.Equals((respuesta ?? string.Empty).Trim(), codigo, StringComparison.OrdinalIgnoreCase);

        // Guardar la respuesta en la BD
        bd.GuardarRespuesta(nombreUsuario, sala, correcto);

        if (correcto)
        {
            // Avanzar a la siguiente sala
            bd.ActualizarSalaActual(sala + 1, nombreUsuario);
            return RedirectToAction("Nivel" + (sala + 1), new { nombreUsuario = nombreUsuario });
        }
        else
        {
            // Volver a la misma sala si la respuesta es incorrecta
            return RedirectToAction("Nivel" + sala, new { nombreUsuario = nombreUsuario });
        }
    }

}

