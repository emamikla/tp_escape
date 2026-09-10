namespace tp_escape.Models;

using Dapper;
using Microsoft.Data.SqlClient;

public class BD
{
    private readonly string _connectionString =
        "Server=localhost;Database=Escape;integrated security=true;TrustServerCertificate=True;";

    public bool ExisteUsuario(string nombreUsuario)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var query = "SELECT COUNT(*) FROM Partidas WHERE Nombre_Usuario = @NombreUsuario";
        int count = connection.ExecuteScalar<int>(query, new { NombreUsuario = nombreUsuario });
        return count > 0;
    }

    public int ObtenerSalaActual(string nombreUsuario)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var query = "SELECT Sala_Actual FROM Partidas WHERE Nombre_Usuario = @NombreUsuario";
        int salaActual = connection.ExecuteScalar<int>(query, new { NombreUsuario = nombreUsuario });
        return salaActual == 0 ? 1 : salaActual;
    }

    public void CrearPartida(string nombreUsuario, int idSala)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var query = @"INSERT INTO Partidas (Nombre_Usuario, Fecha, Hora, Estado, Puntuacion, Sala_Actual)
                      VALUES (@NombreUsuario, @Fecha, @Hora, @Estado, @Puntuacion, @SalaActual)";
        connection.Execute(query, new
        {
            NombreUsuario = nombreUsuario,
            Fecha = DateTime.Now.Date,
            Hora = DateTime.Now.TimeOfDay,
            Estado = "En curso",
            Puntuacion = 0m,
            SalaActual = idSala
        });
    }

    public void ActualizarSala(string nombreUsuario, int idSala)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var estado = idSala > 5 ? "Finalizado" : "En curso";
        var query = @"UPDATE Partidas SET Sala_Actual = @IdSala, Estado = @Estado
                      WHERE Nombre_Usuario = @NombreUsuario";
        connection.Execute(query, new { IdSala = idSala, Estado = estado, NombreUsuario = nombreUsuario });
    }

    public void GuardarRespuesta(string nombreUsuario, int idSala, bool valorRespuesta)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var query = @"INSERT INTO Respuestas (Nombre_Usuario, Id_Sala, ValorRespuesta, FechaHora)
                      VALUES (@NombreUsuario, @IdSala, @ValorRespuesta, @FechaHora)";
        connection.Execute(query, new
        {
            NombreUsuario = nombreUsuario,
            IdSala = idSala,
            ValorRespuesta = valorRespuesta,
            FechaHora = DateTime.Now
        });
    }

    public bool VerificarRespuesta(int sala, string respuesta)
    {
        var codigo = ObtenerCodigoSala(sala);
        if (string.IsNullOrWhiteSpace(codigo) || string.IsNullOrWhiteSpace(respuesta))
        {
            return false;
        }
        return string.Equals(codigo.Trim(), respuesta.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    public string ObtenerCodigoSala(int sala)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        var query = "SELECT TOP 1 Codigo_Sala FROM Salas WHERE OrdenSecuencial = @Sala";
        var codigo = connection.QuerySingleOrDefault<string>(query, new { Sala = sala });
        return codigo ?? string.Empty;
    }
}