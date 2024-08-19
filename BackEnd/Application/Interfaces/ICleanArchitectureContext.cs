using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface ICleanArchitectureContext : IContext
    {

        public DbSet<Product> Products { get; set; }


    }
}
