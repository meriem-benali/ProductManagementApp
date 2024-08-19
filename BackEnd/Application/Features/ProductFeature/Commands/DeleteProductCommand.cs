using Application.Interfaces;
using Application.Setting;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ProductFeature.Commands
{
    public record DeleteProductCommand(
        Guid ProductId
        )
        : IRequest<ResponseHttp>
    {
        public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ResponseHttp>
        {
            private readonly IProductRepository ProductRepository;
            public DeleteProductCommandHandler(IProductRepository ProductRepository)
            {
                this.ProductRepository = ProductRepository;
            }

            public async Task<ResponseHttp> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                var product = await ProductRepository.GetById(request.ProductId);

                if (product == null)
                {
                    return new ResponseHttp
                    {
                        Fail_Messages = "No Product found",
                        Status = StatusCodes.Status400BadRequest,
                    };
                }

                await ProductRepository.SoftDelete(request.ProductId);
                await ProductRepository.SaveChange(cancellationToken);

                return new ResponseHttp
                {
                    Status = StatusCodes.Status200OK,
                };
            }
        }
    }
}
