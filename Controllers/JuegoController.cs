using Microsoft.AspNetCore.Mvc;
using tp_escape.Models;

namespace tp_escape.Controllers;

public class JuegoController : Controller
{
    private readonly BD _bd = new();

    [HttpGet]
    public IActionResult Index()
    {
        var partidaId = HttpContext.Session.GetInt32("PartidaId");
        if (partidaId.HasValue)
        {
            return RedirectToAction(nameof(Sala));
        }

        return View(new JuegoViewModel());
    }

    [HttpGet]
    public IActionResult Sala()
    {
        var partidaId = HttpContext.Session.GetInt32("PartidaId");
        if (!partidaId.HasValue)
        {
            return RedirectToAction(nameof(Index));
        }

        var partida = _bd.ObtenerPartida(HttpContext);
        var salaActual = _bd.ObtenerSalaActual(partidaId.Value);

        if (salaActual == null)
        {
            return View(new JuegoViewModel
            {
                PartidaId = partidaId.Value,
                NombreParticipante = HttpContext.Session.GetString("NombreParticipante") ?? partida?.NombreParticipante,
                SalaActual = 0
            });
        }

        HttpContext.Session.SetInt32("SalaActual", salaActual.ID);

        return View(new JuegoViewModel
        {
            PartidaId = partidaId.Value,
            NombreParticipante = HttpContext.Session.GetString("NombreParticipante") ?? partida?.NombreParticipante,
            SalaActual = salaActual.ID,
            Pista = $"Pista de la sala {salaActual.Codigo_Sala}: el código que buscas está oculto en la escena."
        });
    }

    [HttpPost]
    public IActionResult IngresarSala(string nombreParticipante, string respuesta)
    {
        var nombre = string.IsNullOrWhiteSpace(nombreParticipante)
            ? HttpContext.Session.GetString("NombreParticipante") ?? "Jugador"
            : nombreParticipante;

        var partidaId = HttpContext.Session.GetInt32("PartidaId");
        if (!partidaId.HasValue)
        {
            partidaId = _bd.CrearPartida(nombre);
            HttpContext.Session.SetInt32("PartidaId", partidaId.Value);
        }

        HttpContext.Session.SetString("NombreParticipante", nombre);

        var salaActual = _bd.ObtenerSalaActual(partidaId.Value);
        var salaActualId = HttpContext.Session.GetInt32("SalaActual") ?? (salaActual?.ID ?? 0);

        if (salaActual == null && salaActualId == 0)
        {
            return RedirectToAction(nameof(Index));
        }

        if (salaActualId > 0)
        {
            HttpContext.Session.SetInt32("SalaActual", salaActualId);
        }

        var correcta = _bd.ValidarRespuesta(salaActualId, respuesta ?? string.Empty);
        _bd.GuardarRespuesta(partidaId.Value, salaActualId, respuesta ?? string.Empty, correcta);

        if (correcta)
        {
            var siguienteSala = _bd.ObtenerSiguienteSala(partidaId.Value, (salaActual ?? _bd.ObtenerSalaPorId(salaActualId))?.OrdenSecuencial ?? 1);
            if (siguienteSala != null)
            {
                _bd.ActualizarSalaActual(partidaId.Value, siguienteSala.ID);
                HttpContext.Session.SetInt32("SalaActual", siguienteSala.ID);
            }
        }

        var vm = new JuegoViewModel
        {
            PartidaId = partidaId.Value,
            NombreParticipante = nombre,
            SalaActual = HttpContext.Session.GetInt32("SalaActual") ?? salaActualId,
            Respuesta = respuesta,
            EsCorrecta = correcta,
            Mensaje = correcta ? "¡Respuesta correcta!" : "Respuesta incorrecta. Inténtalo de nuevo.",
            Pista = "La pista corresponde a la sala activa; revisa la escena antes de responder."
        };

        return View("Sala", vm);
    }
}
