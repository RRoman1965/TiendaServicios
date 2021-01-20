using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiendaServicios.Api.CarritoCompra.Modelo;

namespace TiendaServicios.Api.CarritoCompra.Aplicacion
{
    public class Nuevo
    {
        public class Ejecuta : IRequest
        {
            public DateTime? FechaCreacionSesion { get; set; }
            public List<string> ProductoLista { get; set; }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly CarritoContexto _contexto;
            public Manejador(CarritoContexto contexto)
            {
                _contexto = contexto;
            }
            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                Unit valueReturn;

                var carritoSesion = new CarritoSesion
                {
                    FechaCreacion = request.FechaCreacionSesion
                };
                _contexto.CarritoSesion.Add(carritoSesion);
                var value = await _contexto.SaveChangesAsync();
                if (value == 0)
                {
                    throw new Exception("Error en la insercion del carrito");
                }

                int id = carritoSesion.CarritoSesionId;

                foreach (var producto in request.ProductoLista)
                {
                    var detalleSesion = new CarritoSesionDetalle
                    {
                        FechaCreacion = DateTime.Now,
                        CarritoSesionId = id,
                        ProductoSeleccionado = producto
                    };

                    _contexto.CarritoSesionDetalle.Add(detalleSesion);
                    value = await _contexto.SaveChangesAsync();
                    if (value == 0)
                    {
                        throw new Exception("No se puedo insertar el detalle del carrito");
                    }
                    valueReturn = Unit.Value;
                }
                return valueReturn;
            }
        }
    }
}
