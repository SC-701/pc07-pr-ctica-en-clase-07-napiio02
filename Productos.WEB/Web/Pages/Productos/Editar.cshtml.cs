
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Models;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class EditarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public EditarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public ProductoResponse productoResponse { get; set; } = new ProductoResponse();

        [BindProperty]
        public List<SelectListItem> categorias { get; set; } = new();

        [BindProperty]
        public List<SelectListItem> subcategorias { get; set; } = new();

        [BindProperty]
        public Guid categoriaseleccionada { get; set; }

        [BindProperty]
        public Guid subcategoriaseleccionada { get; set; }

        public async Task<IActionResult> OnGet(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty)
                return NotFound();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProducto");
            using var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, id.Value));

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            if (respuesta.StatusCode == HttpStatusCode.OK)
            {
                await ObtenerCategorias();

                var resultado = await respuesta.Content.ReadAsStringAsync();
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                productoResponse = JsonSerializer.Deserialize<ProductoResponse>(resultado, opciones) ?? new ProductoResponse();

                // Usa productoResponse.Categoria (campo del modelo)
                var categoriaItem = categorias.FirstOrDefault(c => c.Text == productoResponse.Categoria);
                if (categoriaItem != null && Guid.TryParse(categoriaItem.Value, out Guid idCategoria))
                {
                    categoriaseleccionada = idCategoria;

                    // Usa productoResponse.SubCategoria (campo del modelo)
                    subcategorias = (await ObtenerSubcategorias(categoriaseleccionada)).Select(s =>
                        new SelectListItem
                        {
                            Value = s.Id.ToString(),
                            Text = s.Nombre,
                            Selected = s.Nombre == productoResponse.SubCategoria
                        }
                    ).ToList();

                    var subcategoriaItem = subcategorias.FirstOrDefault(s => s.Text == productoResponse.SubCategoria);
                    if (subcategoriaItem != null && Guid.TryParse(subcategoriaItem.Value, out Guid idSubcategoria))
                        subcategoriaseleccionada = idSubcategoria;
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(Guid? id)
        {
            if (!id.HasValue || id.Value == Guid.Empty)
                return NotFound();

            var request = new ProductoRequest
            {
                Nombre = productoResponse.Nombre,
                Descripcion = productoResponse.Descripcion,
                Precio = productoResponse.Precio,
                Stock = productoResponse.Stock,
                CodigoBarras = productoResponse.CodigoBarras,
                IdSubCategoria = subcategoriaseleccionada   // nombre exacto del modelo
            };

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EditarProducto");
            var cliente = new HttpClient();

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var solicitud = new HttpRequestMessage(HttpMethod.Put, string.Format(endpoint, id.Value))
            {
                Content = content
            };

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Index");
        }

        private async Task ObtenerCategorias()
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerCategorias");
            var cliente = new HttpClient();
            var solicitud = new HttpRequestMessage(HttpMethod.Get, endpoint);

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            // CategoriaResponse viene de Abstracciones.Modelos
            var resultadoDeserializado = JsonSerializer.Deserialize<List<CategoriaResponse>>(resultado, opciones) ?? new();

            categorias = resultadoDeserializado
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Nombre
                })
                .ToList();
        }

        private async Task<List<SubCategoriaResponse>> ObtenerSubcategorias(Guid categoriaID)
        {
            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerSubCategoriasPorCategoria");
            var cliente = new HttpClient();

            var solicitud = new HttpRequestMessage(HttpMethod.Get, string.Format(endpoint, categoriaID));

            var respuesta = await cliente.SendAsync(solicitud);
            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();
            var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<List<SubCategoriaResponse>>(resultado, opciones) ?? new();
        }
    }
}