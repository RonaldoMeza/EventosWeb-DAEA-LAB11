using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventosWeb.Models;

public class Evento
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Titulo { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    public DateTime Fecha { get; set; }

    [Required]
    [StringLength(100)]
    public string Lugar { get; set; } = string.Empty;

    [Range(1, 5000)]
    public int Aforo { get; set; }

    [Range(typeof(decimal), "0", "99999999.99")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Precio { get; set; }

    [Required]
    [StringLength(20)]
    [RegularExpression("^(Presencial|Virtual)$", ErrorMessage = "Seleccione una modalidad válida.")]
    public string Modalidad { get; set; } = "Presencial";

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría.")]
    public int CategoriaId { get; set; }

    public bool Activo { get; set; } = true;

    [ForeignKey(nameof(CategoriaId))]
    public Categoria Categoria { get; set; } = null!;

    public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
}
