using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Interface;
using TiendaServicios.Mensajeria.Email.SendGridLibreria.Modelo;

namespace TiendaServicios.Mensajeria.Email.SendGridLibreria.Implement
{
    public class SendGridEnviar : ISendGridEnviar
    {
        public async Task<(bool resultado, string errorMessage)> EnviarEmail(SendGridData data)
        {
            try
            {
                var sendGridCliente = new SendGridClient(data.SendGridAPIKey);
                var destinatario = new EmailAddress(data.EmailDestinatario, data.NombreDestinatario);
                var tituloEmail = data.Titulo;
                var sender = new EmailAddress("grobertomovil@gmail.com", "Roberto Roman");
                var contenido = data.Contenido;

                var objMensaje = MailHelper.CreateSingleEmail(sender, destinatario, tituloEmail, contenido, contenido);

                await sendGridCliente.SendEmailAsync(objMensaje);

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }            
        }
    }
}
