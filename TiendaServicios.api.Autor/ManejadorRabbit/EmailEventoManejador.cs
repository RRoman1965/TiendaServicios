﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Interface;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Modelo;
using TiendaServiciosRabbitMQ_Bus.BusRabbit;
using TiendaServiciosRabbitMQ_Bus.EventoQueue;

namespace TiendaServicios.api.Autor.ManejadorRabbit
{
    public class EmailEventoManejador : IEventoManejador<EmailEventoQueue>
    {
        private readonly ILogger<EmailEventoManejador> _logger;
        private readonly ISendGridEnviar _sendGridEnviar;
        private readonly Microsoft.Extensions.Configuration.IConfiguration _configuration;

        public EmailEventoManejador() { }
        public EmailEventoManejador(ILogger<EmailEventoManejador> logger, ISendGridEnviar sendGridEnviar, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            _logger = logger;
            _sendGridEnviar = sendGridEnviar;
            _configuration = configuration;
        }
        public async Task Handle(EmailEventoQueue @event)
        {
            _logger.LogInformation(@event.Titulo);

            var objData = new SendGridData();
            objData.Contenido = @event.Contenido;
            objData.EmailDestinatario = @event.Destinatario;
            objData.NombreDestinatario = @event.Destinatario;
            objData.Titulo = @event.Titulo;
            objData.SendGridAPIKey = _configuration["SendGrid:ApiKey"];

            var resultado = await _sendGridEnviar.EnviarEmail(objData);

            if (resultado.resultado)
            {
                await Task.CompletedTask;
                return;
            }
        }
    }
}
