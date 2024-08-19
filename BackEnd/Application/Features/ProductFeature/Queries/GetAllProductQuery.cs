using Application.Features.ProductFeature.Dtos;
using Application.Interfaces;
using Application.Setting;
using AutoMapper;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ProductFeature.Queries
{
    public record class GetAllProductQuery(int? pageNumber, int? pageSize) : IRequest<ResponseHttp>
    {
        public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, ResponseHttp>
        {
            private readonly IProductRepository ProductRepository;
            private readonly IMapper _mapper;

            public GetAllProductQueryHandler(IProductRepository ProductRepository, IMapper mapper)
            {
                this.ProductRepository = ProductRepository;
                _mapper = mapper;
            }

            public async Task<ResponseHttp> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
            {
                var Product = await ProductRepository.GetAllWithTypesAsync(request.pageNumber, request.pageSize, cancellationToken);

                if (Product == null)
                    return new ResponseHttp
                    {
                        Fail_Messages = "No Product found !",
                        Status = StatusCodes.Status400BadRequest,
                    };

                var customersToReturn = _mapper.Map<PagedList<ProductDTO>>(Product);
                return new ResponseHttp
                {
                    Status = 200,
                    Resultat = customersToReturn
                };
            }
        }
    }
}

