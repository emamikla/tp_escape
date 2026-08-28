namespace tp_escape.Models;

public class JuegoViewModel
{
    public int PartidaId { get; set; }
    public string? NombreParticipante { get; set; }
    public int SalaActual { get; set; }
    public string? Respuesta { get; set; }
    public bool EsCorrecta { get; set; }
    public string? Mensaje { get; set; }
    public string? Pista { get; set; }
}
