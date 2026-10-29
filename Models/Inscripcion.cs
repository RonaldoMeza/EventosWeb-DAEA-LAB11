using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventosWeb.Models;

public class Inscripcion
{
    [Key]
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un evento.")]
    public int EventoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un asistente.")]
    public int AsistenteId { get; set; }

    public DateTime FechaInscripcion { get; set; }

    [Required]
    [RegularExpression("^(Confirmada|Cancelada)$")]
    public string Estado { get; set; } = "Confirmada";

    [ForeignKey(nameof(EventoId))]
    public Evento Evento { get; set; } = null!;

    [ForeignKey(nameof(AsistenteId))]
    public Asistente Asistente { get; set; } = null!;
}
