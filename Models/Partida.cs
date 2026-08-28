namespace tp_escape.Models;

public class Partida
{
    public int Id { get; set; }
    public int? ID_Jugador { get; set; }
    public string? NombreParticipante { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan? Hora { get; set; }
    public string? Estado { get; set; }
    public decimal Puntuacion { get; set; }
}