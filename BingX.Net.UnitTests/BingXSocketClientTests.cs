using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Objects;
using System.Diagnostics;
using System.Reflection;
using BingX.Net.Clients.SpotApi;
using NUnit.Framework.Legacy;

namespace BingX.Net.UnitTests
{
    [TestFixture()]
    public class BingXSocketClientTests
    {
        [Test]
        public void CheckSocketInterfaces()
        {
            var assembly = Assembly.GetAssembly(typeof(BingXSocketClientSpotApi));
            var clientInterfaces = assembly.GetTypes().Where(t => t.Name.StartsWith("IBingXSocketClient"));

            foreach (var clientInterface in clientInterfaces)
            {
                var implementation = assembly.GetTypes().Single(t => t.IsAssignableTo(clientInterface) && t != clientInterface);
                int methods = 0;
                foreach (var method in implementation.GetMethods().Where(m => m.ReturnType.IsAssignableTo(typeof(Task<CallResult<UpdateSubscription>>))))
                {
                    var interfaceMethod = clientInterface.GetMethod(method.Name, method.GetParameters().Select(p => p.ParameterType).ToArray());
                    ClassicAssert.NotNull(interfaceMethod, $"Missing interface for method {method.Name} in {implementation.Name} implementing interface {clientInterface.GetType().Name}");
                    methods++;
                }
                Debug.WriteLine($"{clientInterface.Name} {methods} methods validated");
            }
        }
    }
}
