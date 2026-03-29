

using Abstracciones.Models;
using Microsoft.AspNetCore.Mvc;

namespace Abstracciones.Interfaces.API
{
    public interface IProductoController
    {
        Task<ActionResult> Obtener();
        Task<ActionResult> Obtener(Guid Id);
        Task<ActionResult> Agregar(ProductoRequest vehiculo);
        Task<ActionResult> Editar(Guid Id, ProductoRequest vehiculo);
        Task<ActionResult> Eliminar(Guid Id);
    }
}
