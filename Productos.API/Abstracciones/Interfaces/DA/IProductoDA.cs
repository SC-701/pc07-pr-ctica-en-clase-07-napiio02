

using Abstracciones.Models;

namespace Abstracciones.Interfaces.DA
{
    public interface IProductoDA
    {
        Task<IEnumerable<ProductoResponse>> Obtener();
        Task<ProductoResponse> Obtener(Guid Id);
        Task<Guid> Agregar(ProductoRequest vehiculo);
        Task<Guid> Editar(Guid Id, ProductoRequest vehiculo);
        Task<Guid> Eliminar(Guid Id);

    }
}
