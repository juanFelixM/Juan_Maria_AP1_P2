using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Juan_Maria_AP1_P2.Models
{
    public class EntradasDetalle
    {
        [Key]
        public int DetallesId { get; set; }

        public int EntradasId { get; set; }
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; } = null!;

        public int Cantidad { get; set; }
    }
} 