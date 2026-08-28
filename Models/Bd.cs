namespace tp_escape.Models;

using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;

public class BD
{
    private readonly string _cs = "Server=(localdb)\\MSSQLLocalDB;Database=Escape;Integrated Security=True;TrustServerCertificate=True;";

    public Sala ObtenerSalaPorId(int id)
    {
        using var cn = new SqlConnection(_cs);
        return cn.QueryFirstOrDefault<Sala>(
            "SELECT * FROM Salas WHERE ID = @Id",
            new { Id = id });
    }

    public Sala ObtenerSalaPorCodigo(string codigo)
    {
        using var cn = new SqlConnection(_cs);
        return cn.QueryFirstOrDefault<Sala>(
            "SELECT * FROM Salas WHERE Codigo_Sala = @Codigo",
            new { Codigo = codigo });
    }

    public int CrearPartida(string nombreParticipante)
    {
        using var cn = new SqlConnection(_cs);

        var ahora = DateTime.Now;
        var partidaId = cn.QuerySingle<int>(
            @"INSERT INTO Partidas (Fecha, Hora, ID_Jugador, Estado, Puntuacion)
              VALUES (@Fecha, @Hora, @IdJugador, @Estado, @Puntuacion);
              SELECT CAST(SCOPE_IDENTITY() AS int);",
            new
            {
                Fecha = ahora.Date,
                Hora = ahora.TimeOfDay,
                IdJugador = 0,
                Estado = "Activa",
                Puntuacion = 0m
            });

        var salasIniciales = new[]
        {
            new { Codigo_Sala = 101, OrdenSecuencial = 1, PartidaId = partidaId },
            new { Codigo_Sala = 202, OrdenSecuencial = 2, PartidaId = partidaId },
            new { Codigo_Sala = 303, OrdenSecuencial = 3, PartidaId = partidaId }
        };

        cn.Execute(
            @"INSERT INTO Salas (Codigo_Sala, OrdenSecuencial, PartidaId)
              VALUES (@Codigo_Sala, @OrdenSecuencial, @PartidaId)",
            salasIniciales);

        return partidaId;
    }

    public Partida ObtenerPartida(HttpContext http)
    {
        var partidaId = http.Session.GetInt32("PartidaId");
        if (partidaId == null) return null;

        using var cn = new SqlConnection(_cs);

        var partida = cn.QueryFirstOrDefault<Partida>(
            @"SELECT ID as Id, Fecha, Hora, ID_Jugador, Estado, Puntuacion
              FROM Partidas
              WHERE ID = @Id",
            new { Id = partidaId.Value });

        if (partida != null)
        {
            partida.NombreParticipante = http.Session.GetString("NombreParticipante");
        }

        return partida;
    }

    public Sala? ObtenerSalaActual(int partidaId)
    {
        using var cn = new SqlConnection(_cs);

        return cn.QueryFirstOrDefault<Sala>(
            @"SELECT *
              FROM Salas
              WHERE PartidaId = @PartidaId
              ORDER BY OrdenSecuencial ASC",
            new { PartidaId = partidaId });
    }

    public Sala? ObtenerSiguienteSala(int partidaId, int ordenActual)
    {
        using var cn = new SqlConnection(_cs);

        return cn.QueryFirstOrDefault<Sala>(
            @"SELECT TOP 1 *
              FROM Salas
              WHERE PartidaId = @PartidaId
                AND OrdenSecuencial > @OrdenActual
              ORDER BY OrdenSecuencial ASC",
            new { PartidaId = partidaId, OrdenActual = ordenActual });
    }

    public void GuardarRespuesta(int partidaId, int salaId, string respuesta, bool correcta)
    {
        using var cn = new SqlConnection(_cs);

        cn.Execute(
            @"INSERT INTO Respuestas (ID_Partida, Id_Sala, ValorRespuesta, FechaHora)
              VALUES (@PartidaId, @SalaId, @ValorRespuesta, @FechaHora)",
            new
            {
                PartidaId = partidaId,
                SalaId = salaId,
                ValorRespuesta = correcta,
                FechaHora = DateTime.Now
            });
    }

    public void ActualizarSalaActual(int partidaId, int nuevaSalaId)
    {
        using var cn = new SqlConnection(_cs);

        var sala = cn.QueryFirstOrDefault<Sala>(
            "SELECT * FROM Salas WHERE ID = @Id",
            new { Id = nuevaSalaId });

        if (sala == null)
        {
            return;
        }

        cn.Execute(
            @"UPDATE Partidas
              SET Estado = @Estado
              WHERE ID = @Id",
            new { Id = partidaId, Estado = $"Sala {sala.Codigo_Sala}" });
    }

    public bool ValidarRespuesta(int salaId, string respuesta)
    {
        var sala = ObtenerSalaPorId(salaId);
        if (sala == null)
        {
            return false;
        }

        return sala.Codigo_Sala.ToString() == (respuesta ?? string.Empty).Trim();
    }
}