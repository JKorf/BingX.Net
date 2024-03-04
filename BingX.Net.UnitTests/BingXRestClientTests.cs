using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using BingX.Net.Clients;

namespace BingX.Net.UnitTests
{
    [TestFixture()]
    public class BingXRestClientTests
    {
        [Test]
        public void CheckRestInterfaces()
        {
            var assembly = Assembly.GetAssembly(typeof(BingXRestClient));
            var ignore = new string[] { };
            var clientInterfaces = assembly.GetTypes().Where(t => t.Name.StartsWith("IBingXRestClient") && !ignore.Contains(t.Name));

            foreach (var clientInterface in clientInterfaces)
            {
                var implementation = assembly.GetTypes().Single(t => t.IsAssignableTo(clientInterface) && t != clientInterface);
                int methods = 0;
                foreach (var method in implementation.GetMethods().Where(m => m.ReturnType.IsAssignableTo(typeof(Task))))
                {
                    var interfaceMethod = clientInterface.GetMethod(method.Name, method.GetParameters().Select(p => p.ParameterType).ToArray());
                    Assert.NotNull(interfaceMethod, $"Missing interface for method {method.Name} in {implementation.Name} implementing interface {clientInterface.Name}");
                    methods++;
                }
                Debug.WriteLine($"{clientInterface.Name} {methods} methods validated");
            }
        }

    }
}
