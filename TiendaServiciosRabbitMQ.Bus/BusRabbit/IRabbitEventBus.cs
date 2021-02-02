using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TiendaServiciosRabbitMQ.Bus.Comandos;
using TiendaServiciosRabbitMQ.Bus.Eventos;

namespace TiendaServiciosRabbitMQ.Bus.BusRabbit
{
    public interface IRabbitEventBus
    {
        Task EnviarComando<T>(T comando) where T : Comando;

        void Publish<T>(T @evento) where T : Evento;

        void Subscribe<T, TH>() where T : Evento 
                                where TH : IEventoManejador<T>;
    }
}
