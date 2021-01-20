using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TiendaServicios.Api.Libro.Test
{
    public class AsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        public AsyncEnumerator(IEnumerator<T> enumerator) => this.enumerator = enumerator ?? throw new ArgumentException(); 

        private readonly IEnumerator<T> enumerator;
        public T Current => enumerator.Current;

        public async ValueTask DisposeAsync()
        {
            await Task.CompletedTask;
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            return await Task.FromResult(enumerator.MoveNext());
        }
    }
}
