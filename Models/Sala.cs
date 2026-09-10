namespace tp_escape.Models;

public class Sala
{
    public int ID { get; set; }
    public string? Codigo_Sala { get; set; }
    public int OrdenSecuencial { get; set; }
    public int PartidaId { get; set; }
}