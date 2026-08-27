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

    public IEnumerable<Sala> ListarSalasDePartida(int partidaId)
{
    using var cn = new SqlConnection(_cs);
    return cn.Query<Sala>(
        @"SELECT * FROM Salas 
          WHERE PartidaId = @PartidaId 
          ORDER BY OrdenSecuencial", 
        new { PartidaId = partidaId });
}
}