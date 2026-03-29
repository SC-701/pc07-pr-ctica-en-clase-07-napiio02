
using Abstracciones.Interfaces.Reglas;
using Abstracciones.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Web.Pages.Productos
{
    public class EliminarModel : PageModel
    {
        private readonly IConfiguracion _configuracion;

        public EliminarModel(IConfiguracion configuracion)
        {
            _configuracion = configuracion;
        }

        public ProductoResponse producto { get; set; } = default!;

        public async Task<IActionResult> OnGet(Guid? id)
        {
            if (id == null)
                return NotFound();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "ObtenerProducto");

            var cliente = new HttpClient();
            var respuesta = await cliente.GetAsync(string.Format(endpoint, id));

            respuesta.EnsureSuccessStatusCode();

            var resultado = await respuesta.Content.ReadAsStringAsync();

            var opciones = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            producto = JsonSerializer.Deserialize<ProductoResponse>(resultado, opciones)!;

            return Page();
        }

        public async Task<IActionResult> OnPost(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return NotFound();

            string endpoint = _configuracion.ObtenerMetodo("ApiEndPoints", "EliminarProducto");

            var cliente = new HttpClient();
            var respuesta = await cliente.DeleteAsync(string.Format(endpoint, id));

            respuesta.EnsureSuccessStatusCode();

            return RedirectToPage("./Index");
        }
    }
}