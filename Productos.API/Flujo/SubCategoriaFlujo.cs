
using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Modelos;

namespace Flujo
{
    public class SubCategoriaFlujo : ISubCategoriaFlujo
    {
        private readonly ISubCategoriaDA _subCategoriaDA;

        public SubCategoriaFlujo(ISubCategoriaDA subCategoriaDA)
        {
            _subCategoriaDA = subCategoriaDA;
        }

        public async Task<IEnumerable<SubCategoriaResponse>> ObtenerPorCategoria(Guid IdCategoria)
        {
            return await _subCategoriaDA.ObtenerPorCategoria(IdCategoria);
        }
    }
}