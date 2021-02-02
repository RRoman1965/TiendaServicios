using System;
using System.Collections.Generic;
using System.Text;

namespace TiendaServiciosRabbitMQ_Bus.Eventos
{
    public abstract class Evento
    {
        public DateTime Timestamp { get; protected set; }

        protected Evento()
        {
            Timestamp = DateTime.Now;
        }
    }
}
