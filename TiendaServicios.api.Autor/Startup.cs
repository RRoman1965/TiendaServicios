using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TiendaServicios.api.Autor.Aplicacion;
using TiendaServicios.api.Autor.ManejadorRabbit;
using TiendaServicios.api.Autor.Persistencia;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Implement;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Interface;
using TiendaServiciosRabbitMQ.Bus.BusRabbit;
using TiendaServiciosRabbitMQ.Bus.EventoQueue;
using TiendaServiciosRabbitMQ.Bus.Implement;

namespace TiendaServicios.api.Autor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IRabbitEventBus, RabbitEventBus>( sp => {
                var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                return new RabbitEventBus(sp.GetService<IMediator>(), scopeFactory);
            });

            services.AddSingleton<ISendGridEnviar, SendGridEnviar>();
            services.AddTransient<EmailEventoManejador>();

            services.AddTransient<IEventoManejador<EmailEventoQueue>, EmailEventoManejador>();
            services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());
            services.AddDbContext<ContextoAutor>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("ConexionDatabase"));
            });

            services.AddMediatR(typeof(Nuevo.Manejador).Assembly);
            services.AddAutoMapper(typeof(Consulta.Manejador));    
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var eventBus = app.ApplicationServices.GetRequiredService<IRabbitEventBus>();
            eventBus.Subscribe<EmailEventoQueue, EmailEventoManejador>();
        }
    }
}
