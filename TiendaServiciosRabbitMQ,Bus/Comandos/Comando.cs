using System;
using System.Collections.Generic;
using System.Text;
using TiendaServiciosRabbitMQ.Bus.Eventos;

namespace TiendaServiciosRabbitMQ.Bus.Comandos
{
    public abstract class Comando : Message
    {
        public DateTime Timestamp { get; protected set; }

        protected Comando()
        {
            Timestamp = DateTime.Now;
        }
    }
}
