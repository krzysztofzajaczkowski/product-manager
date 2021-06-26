using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProductManager.AcceptanceTests
{
    [CollectionDefinition("DockerTests")]
    public class DockerTestsCollection : ICollectionFixture<HttpClientFixture>
    {
    }
}
