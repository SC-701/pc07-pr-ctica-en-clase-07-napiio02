
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Models;
using Abstracciones.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class AgregarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public AgregarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        [BindProperty]
        public ProductoRequest producto { get; set; } = new ProductoRequest();

        [BindProperty]
        public List<SelectListItem> categorias { get; set; } = new();

        [BindProperty]
        public List<SelectListItem> subcategorias { get; set; } = new();

        [BindProperty]
        public Guid categoriaSeleccionada { get; set; }

        public async Task<ActionResult> OnGet()
        {
            await ObtenerCategorias();
            return Page();
        }

        public async Task<ActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "AgregarProducto");
            var cliente = new HttpClient();

            var respuesta = await cliente.PostAsJsonAsync(endpoint, producto);
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

        public async Task<JsonResult> OnGetObtenerSubcategorias(Guid categoriaID)
        {
            var subcats = await ObtenerSubcategorias(categoriaID);
            return new JsonResult(subcats);
        }
    }
}