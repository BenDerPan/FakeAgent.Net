using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace FakeAgent.Net.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public async Task LoadFromFile()
        {
            var isOk = await FakeAgent.TryLoadSource(useLocal: false);
            Assert.AreEqual(true, isOk);
            for (int i = 0; i < 100; i++)
            {
                var agent = FakeAgent.RandomAgent;
                Console.WriteLine($"[{i + 1}] {agent}");
            }
            
        }
    }
}
