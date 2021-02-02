using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.Gateway.InterfaceRemote;
using TiendaServicios.Api.Gateway.LibroRemote;

namespace TiendaServicios.Api.Gateway.MessageHandler
{
    public class LibroHandler : DelegatingHandler
    {
        private readonly IAutorRemote _autorRemote;
        private readonly ILogger<LibroHandler> _logger;

        public LibroHandler(IAutorRemote autorRemote, ILogger<LibroHandler> logger)
        {
            _autorRemote = autorRemote;
            _logger = logger;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tiempo = Stopwatch.StartNew();
            _logger.LogInformation("Inicia request");
            var response = await base.SendAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var contenido = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions{ PropertyNameCaseInsensitive = true};
                var resultado = JsonSerializer.Deserialize<LibroModeloRemote>(contenido, options);
                var responseAutor = await _autorRemote.GetAutor(resultado.AutorLibro?? Guid.Empty);
                if (responseAutor.resultado)
                {
                    var objetoAutor = responseAutor.autor;
                    resultado.AutorData = objetoAutor;
                    var resultadoString = JsonSerializer.Serialize(resultado);
                    response.Content = new StringContent(resultadoString, System.Text.Encoding.UTF8, "application/json");
                }
            }
            _logger.LogInformation($"Este proceso se hizo en {tiempo.ElapsedMilliseconds} ms");

            return response;
        }
    }
}
