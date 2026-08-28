namespace tp_escape.Models;

public class Partida
{
    public int Id { get; set; }
    public string NombreParticipante { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime HoraInicio { get; set; }
    public string Estado { get; set; }
    

    public Partida(int id, string nombreParticipante, DateTime fechaInicio, DateTime horaInicio, string estado)
    {
        this.Id = id;
        this.NombreParticipante = nombreParticipante;
        this.FechaInicio = fechaInicio;
        this.HoraInicio = horaInicio;
        this.Estado = estado;
    }
}