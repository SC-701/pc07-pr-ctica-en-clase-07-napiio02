
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Abstracciones.Models
{
    public class ProductoBase
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "El nombre debe tener entre 5 y 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(500, ErrorMessage = "La descripción puede tener máximo 500 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Se requiere un precio válido")]
        [Range(0.01, 1000000, ErrorMessage = "El precio debe ser mayor a 0")] // Valida que el precio sea mayor a 0 y no se pase de 1,000,000
        [JsonPropertyOrder(2)]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La propiedad stock es requerida")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "El stock solo puede tener números positivos")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "El código de barras es obligatorio")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El código de barras debe tener exactamente 13 caracteres")]
        [RegularExpression("^[0-9]{13}$", ErrorMessage = "El código de barras debe contener solo números")]
        public string CodigoBarras { get; set; }
    }

    public class ProductoRequest : ProductoBase
    {
        [Required(ErrorMessage = "El IdSubCategoria es obligatorio")]
        public Guid IdSubCategoria { get; set; }
    }

    public class ProductoResponse : ProductoBase
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Se requiere una subcategoría obligatoria")]
        [StringLength(100, ErrorMessage = "La subcategoría no puede pasarse de los 100 caracteres")]
        public string SubCategoria { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria")]
        [StringLength(100, ErrorMessage = "La categoría no puede pasarse de los 100 caracteres")]
        public string Categoria { get; set; }
    }

    public class ProductoDetalle : ProductoResponse
    {
        [JsonPropertyOrder(3)]
        public decimal PrecioUSD { get; set; }
    }
}