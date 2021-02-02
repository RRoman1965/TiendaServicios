using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TiendaServiciosRabbitMQ_Bus.Eventos;

namespace TiendaServiciosRabbitMQ_Bus.BusRabbit
{
    public interface IEventoManejador<in TEvent> : IEventoManejador where TEvent : Evento
    {
        Task Handle(TEvent @event);
    }

    public interface IEventoManejador
    {

    }
}
