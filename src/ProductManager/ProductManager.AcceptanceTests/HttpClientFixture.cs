using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ductus.FluentDocker.Commands;

namespace ProductManager.AcceptanceTests
{
    public class HttpClientFixture : IDisposable
    {
        public HttpClient Client { get; set; }

        public HttpClientFixture()
        {
            Client = new HttpClient();
        }


        public void Dispose()
        {
            Client?.Dispose();
        }
    }
}
