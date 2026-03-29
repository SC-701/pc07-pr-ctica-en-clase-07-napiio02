
using System.ComponentModel.DataAnnotations;

namespace Abstracciones.Modelos
{
    public class CategoriaBase
    {
        [Required(ErrorMessage = "La propiedad nombre es requerida")]
        [StringLength(80, ErrorMessage = "La propiedad nombre debe ser mayor a 2 caracteres y menor a 80", MinimumLength = 2)]
        public string Nombre { get; set; }
    }

    public class CategoriaResponse : CategoriaBase
    {
        public Guid Id { get; set; }
    }
}
