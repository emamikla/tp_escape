namespace tp_escape.Models;

using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;

public class BD
{
    private string _connectionString = "Server=localhost;Database=Escape;integrated security=true;TrustServerCertificate=True;";

    public void ActualizarSalaActual(int idSala , string nombreUsuario)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = "UPDATE Partidas SET Sala_Actual = @IdSala WHERE Nombre_Usuario = @NombreUsuario";
            connection.Execute(query, new { IdSala = idSala, NombreUsuario = nombreUsuario });
        }
    }
    public bool ExisteUsuario(string nombreUsuario)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            bool existe = false;
            connection.Open();
            var query = "SELECT COUNT(*) FROM Partidas WHERE Nombre_Usuario = @NombreUsuario";
            int count = connection.ExecuteScalar<int>(query, new { NombreUsuario = nombreUsuario });
            if (count > 0)
            {
                existe = true;
            }
            return existe; 
        }
    }
    public int ObtenerSalaActual(string nombreUsuario)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = "SELECT Sala_Actual FROM Partidas WHERE Nombre_Usuario = @NombreUsuario";
            int salaActual = connection.ExecuteScalar<int>(query, new { NombreUsuario = nombreUsuario });
            return salaActual;
        }
    }

    public void GuardarRespuesta(string nombreUsuario, int idSala, bool valorRespuesta)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = "INSERT INTO Respuestas (Nombre_Usuario, Id_Sala, ValorRespuesta, FechaHora) VALUES (@NombreUsuario, @IdSala, @ValorRespuesta, @FechaHora)";
            connection.Execute(query, new { NombreUsuario = nombreUsuario, IdSala = idSala, ValorRespuesta = valorRespuesta, FechaHora = DateTime.Now });
        }
    }

    public void CrearPartida(string nombreUsuario , int idSala)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = "INSERT INTO Partidas (Nombre_Usuario, Sala_Actual) VALUES (@NombreUsuario, @SalaActual)";
            connection.Execute(query, new { NombreUsuario = nombreUsuario, SalaActual = idSala });
        }
    }

    public string ObtenerCodigoSala(int idSala)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var query = "SELECT Codigo FROM Salas WHERE Id = @IdSala";
            var codigo = connection.QuerySingleOrDefault<string>(query, new { IdSala = idSala });
            return codigo;
        }
    }

}