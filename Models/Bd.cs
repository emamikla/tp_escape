namespace tp_escape.Models;
using Microsoft.Data.SqlClient;

public class BD
{
    DataBase = Escape; Integrated Security=True; TrustServerCertificate=True;";

   public Sala ObtenerSalaPorId(int id)
    {
    using var cn = new SqlConnection(_cs);
    return cn.QueryFirstOrDefault<Sala>(
        "SELECT * FROM Salas WHERE Id = @Id", new { Id = id });
    }

    public Sala ObtenerSalaPorCodigo(string codigo)
    {
    using var cn = new SqlConnection(_cs);
    return cn.QueryFirstOrDefault<Sala>(
        "SELECT * FROM Salas WHERE CodigoSala = @Codigo", new { Codigo = codigo });
    }

public void GuardarRespuesta ((int partidaId, int salaId, string respuesta, bool correcta))
{
    

}

public Partida ObtenerPartida(HttpContext http)
{
    var partidaId = http.Session.GetInt32("PartidaId");
    if (partidaId == null) return null;

    using var cn = new SqlConnection(_conn);
    return cn.QueryFirstOrDefault<Partida>(
        "SELECT Id, SalaActual, NombreParticipante FROM Partidas WHERE Id = @Id",
        new { Id = partidaId });
}




}