
using Abstracciones.Interfaces.API;
using Abstracciones.Interfaces.DA;
using Abstracciones.Interfaces.Flujo;
using Abstracciones.Interfaces.Reglas;
using DA;
using Flujo;
using Reglas;
using RepositoryDA;
using Servicios;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IProductoFlujo, ProductoFlujo>();
            builder.Services.AddScoped<IProductoDA, ProductoDA>();
            builder.Services.AddScoped<IRepositorioDapper, RepositorioDapper>();

            //Para API de cambios de moneda

            builder.Services.AddHttpClient<ITipoCambioServicio, TipoCambioServicio>();
            builder.Services.AddScoped<ITipoCambioReglas, TipoCambioReglas>();
            builder.Services.AddScoped<IConfiguracion, Configuracion>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
