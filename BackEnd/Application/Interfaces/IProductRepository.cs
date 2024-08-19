using Domain.Common;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IProductRepository:IGenericRepository<Product>
    {
            Task<PagedList<Product>> GetAllWithTypesAsync(int? pageNumber, int? pageSize, CancellationToken cancellationToken);
        }
    }


