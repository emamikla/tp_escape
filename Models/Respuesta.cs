namespace tp_escape.Models;

public class Respuesta
{
    public int ID { get; set; }
    public string Nombre_Usuario { get; set; } = "";
    public int ID_Sala { get; set; }
    public bool ValorRespuesta { get; set; }
    public DateTime FechaHora { get; set; }
}