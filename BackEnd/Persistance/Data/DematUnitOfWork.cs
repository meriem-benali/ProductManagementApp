using Application.Interfaces;
using Domain.Interfaces;

namespace Persistance.Data
{
    public class DematUnitOfWork : UnitOfWork<CleanArchitectureContext>, IDematUnitOfWork
    {
        public DematUnitOfWork(IContext context) : base(context)
        {
        }
    }
}
