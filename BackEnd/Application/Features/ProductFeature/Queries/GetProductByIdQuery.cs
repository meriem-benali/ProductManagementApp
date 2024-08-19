using Application.Features.ProductFeature.Dtos;
using Application.Interfaces;
using Application.Setting;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ProductFeature.Queries
{
    public record class GetProductByIdQuery(
        Guid ProductId
        ) : IRequest<ResponseHttp>
    {
        public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ResponseHttp>
        {
            private readonly IProductRepository ProductRepository;
            private readonly IMapper _mapper;

            public GetProductByIdQueryHandler(IProductRepository ProductRepository, IMapper mapper)
            {
                this.ProductRepository = ProductRepository;
                _mapper = mapper;
            }

            public async Task<ResponseHttp> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var product = await ProductRepository.GetByIdAsync(request.ProductId, cancellationToken);

                    if (product == null)
                        return new ResponseHttp()
                        {
                            Status = 404,
                            Fail_Messages = "Product not found !"
                        };

                    return new ResponseHttp()
                    {

                        Resultat = _mapper.Map<ProductDTO>(product),
                        Status = 200
                    };
                }
                catch (Exception ex)
                {
                    return new ResponseHttp
                    {
                        Fail_Messages = ex.Message,
                        Status = StatusCodes.Status400BadRequest,
                    };

                }

            }
        }
    }
}
