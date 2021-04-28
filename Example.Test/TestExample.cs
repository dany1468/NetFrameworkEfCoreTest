using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using TestData.Data.Entities;

namespace ExampleTest
{
    public class TestExample
    {
        private const string ConnStr = "Server=host.docker.internal,14331;Database=main;User Id=SA;Password=P@55w0rd;";

        private static MainDbContext CreateDbContext() =>
            new MainDbContext(
                new DbContextOptionsBuilder<MainDbContext>().UseSqlServer(ConnStr).Options);

        private MainDbContext Db { get; set; }

        [SetUp]
        public void Setup() => Db = CreateDbContext();

        [TearDown]
        public async Task TearDown() => await Db.Database.CloseConnectionAsync();

        [Test]
        public async Task Test()
        {
            var beforeCount = await Db.Example.CountAsync();

            Db.Example.Add(new Example
            {
                ExampleInt = 1,
                ExampleBit = false
            });

            await Db.SaveChangesAsync();

            var afterCount = await Db.Example.CountAsync();

            Assert.That(afterCount, Is.EqualTo(beforeCount + 1));
        }
    }
}
