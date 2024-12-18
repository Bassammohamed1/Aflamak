using Aflamak.Data;
using Microsoft.EntityFrameworkCore;

namespace AflamakTests
{
    internal class InMemoryDB : AppDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        }
        public override void Dispose()
        {
            Database.EnsureDeleted();
            base.Dispose();
        }
    }
}
