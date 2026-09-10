using Microsoft.AspNetCore.Mvc;
using tp_escape.Models;

namespace tp_escape.Controllers;

public class EscapeController : Controller
{
    private readonly BD bd = new BD();
    private const string SESSION_KEY_USUARIO = "NombreUsuario";
    private const int TOTAL_SALAS = 5;
    private const int UMBRAL_ROSCO = 14; // aciertos necesarios (sobre 20) para desbloquear el código del Nivel 4


    private static readonly List<RoscoPregunta> RoscoPreguntas = new()
    {
        new() { Letra = "A", Pista = "Objeto de metal que se tira al agua para que el barco no se mueva", RespuestaCorrecta = "ancla" },
        new() { Letra = "B", Pista = "Instrumento que siempre señala el norte", RespuestaCorrecta = "brujula" },
        new() { Letra = "C", Pista = "Caja donde los piratas guardan su tesoro", RespuestaCorrecta = "cofre" },
        new() { Letra = "D", Pista = "Antigua moneda de oro española, muy buscada por los piratas", RespuestaCorrecta = "doblon" },
        new() { Letra = "E", Pista = "Animal marino con cinco brazos que se encuentra en la orilla", RespuestaCorrecta = "estrella" },
        new() { Letra = "F", Pista = "Torre con una luz que guía a los barcos de noche", RespuestaCorrecta = "faro" },
        new() { Letra = "G", Pista = "Ave blanca que sobrevuela la costa buscando comida", RespuestaCorrecta = "gaviota" },
        new() { Letra = "H", Pista = "Se cuelga entre dos palmeras para descansar frente al mar", RespuestaCorrecta = "hamaca" },
        new() { Letra = "I", Pista = "Porción de tierra rodeada de agua por todos lados", RespuestaCorrecta = "isla" },
        new() { Letra = "J", Pista = "Objeto valioso y brillante que los piratas atesoran", RespuestaCorrecta = "joya" },
        new() { Letra = "L", Pista = "Embarcación pequeña usada para llegar a la orilla", RespuestaCorrecta = "lancha" },
        new() { Letra = "M", Pista = "Dibujo antiguo que marca con una X dónde está el tesoro", RespuestaCorrecta = "mapa" },
        new() { Letra = "N", Pista = "Persona que sobrevive luego de que su barco se hunde", RespuestaCorrecta = "naufrago" },
        new() { Letra = "O", Pista = "Movimiento continuo de las olas en el mar", RespuestaCorrecta = "oleaje" },
        new() { Letra = "P", Pista = "Como se llaman los protagonistas isleños de Outer Banks", RespuestaCorrecta = "pogue" },
        new() { Letra = "Q", Pista = "Parte inferior y central del casco de un barco", RespuestaCorrecta = "quilla" },
        new() { Letra = "R", Pista = "Se usa de a pares para impulsar un bote a mano", RespuestaCorrecta = "remo" },
        new() { Letra = "S", Pista = "Criatura mitológica mitad mujer, mitad pez, que canta a los marineros", RespuestaCorrecta = "sirena" },
        new() { Letra = "T", Pista = "Conjunto de riquezas escondidas que todos están buscando", RespuestaCorrecta = "tesoro" },
        new() { Letra = "V", Pista = "Tela grande que impulsa un barco con el viento", RespuestaCorrecta = "vela" },
    };

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Tutorial() => View();

    public IActionResult Integrantes() => View();

    // Arma la view del nivel pedido, cargando en el ViewBag lo que haga falta
    // (por ejemplo, las preguntas del rosco cuando sala == 4).
    private IActionResult MostrarNivel(int sala, string nombreUsuario)
    {
        ViewBag.NombreUsuario = nombreUsuario;
        if (sala == 4)
        {
            ViewBag.Preguntas = RoscoPreguntas;
            ViewBag.Total = RoscoPreguntas.Count;
            ViewBag.Umbral = UMBRAL_ROSCO;
        }
        return View("Nivel" + sala);
    }

    private bool TryGetUsuario(out string nombreUsuario)
    {
        //lo que hace esto es 
        nombreUsuario = HttpContext.Session.GetString(SESSION_KEY_USUARIO) ?? "";
        return !string.IsNullOrEmpty(nombreUsuario);
    }

    [HttpPost]
    public IActionResult FijarseNombreDeUsuario(string nombreUsuario)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario))
        {
            ViewBag.Error = "Ingresá un nombre de usuario para comenzar.";
            return View("Index");
        }

        nombreUsuario = nombreUsuario.Trim();
        // Se guarda en sesión para que Nivel1..Nivel5 sepan quién es el usuario
        // cuando se entra por GET (por ejemplo, refrescando la página).
        HttpContext.Session.SetString(SESSION_KEY_USUARIO, nombreUsuario);

        int salaActual = bd.ExisteUsuario(nombreUsuario)
            ? bd.ObtenerSalaActual(nombreUsuario)
            : CrearPartidaNueva(nombreUsuario);

        if (salaActual > TOTAL_SALAS)
        {
            ViewBag.NombreUsuario = nombreUsuario;
            return View("Final");
        }

        return MostrarNivel(salaActual, nombreUsuario);
    }

    private int CrearPartidaNueva(string nombreUsuario)
    {
        bd.CrearPartida(nombreUsuario, 1);
        return 1;
    }

    public IActionResult Nivel1()
    {
        if (!TryGetUsuario(out var nombreUsuario)) return View("Index");
        return MostrarNivel(1, nombreUsuario);
    }

    public IActionResult Nivel2()
    {
        if (!TryGetUsuario(out var nombreUsuario)) return View("Index");
        return MostrarNivel(2, nombreUsuario);
    }

    public IActionResult Nivel3()
    {
        if (!TryGetUsuario(out var nombreUsuario)) return View("Index");
        return MostrarNivel(3, nombreUsuario);
    }

    public IActionResult Nivel4()
    {
        if (!TryGetUsuario(out var nombreUsuario)) return View("Index");
        return MostrarNivel(4, nombreUsuario);
    }

    public IActionResult Nivel5()
    {
        if (!TryGetUsuario(out var nombreUsuario)) return View("Index");
        return MostrarNivel(5, nombreUsuario);
    }

    public IActionResult Final()
    {
        if (!TryGetUsuario(out var nombreUsuario)) return View("Index");
        ViewBag.NombreUsuario = nombreUsuario;
        return View();
    }

    // Válido para los 5 niveles: el código correcto siempre se compara contra
    // lo que está guardado en la tabla Salas (BD.VerificarRespuesta). Sin redirects: si está mal, se vuelve a mostrar el
    // mismo nivel con el error; si está bien, se muestra directamente el
    // siguiente (o Final).
    [HttpPost]
    public IActionResult FijarRespuesta(string respuesta, int sala, string nombreUsuario)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario))
        {
            nombreUsuario = HttpContext.Session.GetString(SESSION_KEY_USUARIO) ?? "";
        }
        if (string.IsNullOrEmpty(nombreUsuario))
        {
            return View("Index");
        }

        bool esCorrecta = bd.VerificarRespuesta(sala, respuesta);
        bd.GuardarRespuesta(nombreUsuario, sala, esCorrecta);

        if (!esCorrecta)
        {
            ViewBag.Error = "Código incorrecto. Intentá de nuevo.";
            return MostrarNivel(sala, nombreUsuario);
        }

        int siguienteSala = sala + 1;
        bd.ActualizarSala(nombreUsuario, siguienteSala);

        if (siguienteSala > TOTAL_SALAS)
        {
            ViewBag.NombreUsuario = nombreUsuario;
            return View("Final");
        }

        return MostrarNivel(siguienteSala, nombreUsuario);
    }


    [HttpPost]
    public IActionResult ResolverRosco(List<string> respuestas)
    {
        if (!TryGetUsuario(out var nombreUsuario)) return View("Index");

        int aciertos = 0;
        for (int i = 0; i < RoscoPreguntas.Count; i++)
        {
            var respuestaJugador = (respuestas != null && i < respuestas.Count) ? respuestas[i] : "";
            if (string.Equals(respuestaJugador.Trim(), RoscoPreguntas[i].RespuestaCorrecta, StringComparison.OrdinalIgnoreCase))
            {
                aciertos++;
            }
        }

        ViewBag.NombreUsuario = nombreUsuario;
        ViewBag.Preguntas = RoscoPreguntas;
        ViewBag.Total = RoscoPreguntas.Count;
        ViewBag.Umbral = UMBRAL_ROSCO;
        ViewBag.Aciertos = aciertos;

        if (aciertos >= UMBRAL_ROSCO)
        {
            ViewBag.RoscoSuperado = true;
            ViewBag.RoscoMensaje = $"¡Superaste el rosco con {aciertos} de {RoscoPreguntas.Count} aciertos! Ya podés ingresar el código de la sala para continuar.";
        }
        else
        {
            ViewBag.RoscoMensaje = $"Conseguiste {aciertos} de {RoscoPreguntas.Count} aciertos. Necesitás al menos {UMBRAL_ROSCO} para continuar. ¡Probá otra vez!";
        }

        return View("Nivel4");
    }
}