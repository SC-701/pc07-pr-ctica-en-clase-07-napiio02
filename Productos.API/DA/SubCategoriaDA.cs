
using Abstracciones.Interfaces.DA;
using Abstracciones.Modelos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace DA
{
    public class SubCategoriaDA : ISubCategoriaDA
    {
        private IRepositorioDapper _repositorioDapper;
        private SqlConnection _sqlConnection;

        public SubCategoriaDA(IRepositorioDapper repositorioDapper)
        {
            _repositorioDapper = repositorioDapper;
            _sqlConnection = _repositorioDapper.ObtenerRepositorio();
        }

        public async Task<IEnumerable<SubCategoriaResponse>> ObtenerPorCategoria(Guid IdCategoria)
        {
            string query = @"ObtenerSubCategoriasPorCategoria";

            var resultadoConsulta = await _sqlConnection.QueryAsync<SubCategoriaResponse>(
                query,
                new { IdCategoria = IdCategoria }
            );

            return resultadoConsulta;
        }
    }
}
