using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Juan_Maria_AP1_P2.Models
{
    public class Entradas
    {
        [Key]
        public int EntradasId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El concepto es obligatorio")]
        public string? Concepto { get; set; }

        public virtual ICollection<EntradasDetalle> EntradasDetalle { get; set; } = new List<EntradasDetalle>();
    }
} 