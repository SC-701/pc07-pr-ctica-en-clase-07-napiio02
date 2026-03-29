
using Abstracciones.Modelos;

namespace Abstracciones.Interfaces.Flujo
{
    public interface ISubCategoriaFlujo
    {
        Task<IEnumerable<SubCategoriaResponse>> ObtenerPorCategoria(Guid IdCategoria);
    }
}