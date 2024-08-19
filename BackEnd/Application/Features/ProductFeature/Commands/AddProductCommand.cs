using Application.Features.ProductFeature.Dtos;
using Application.Interfaces;
using Application.Setting;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ProductFeature.Commands
{
    public record AddProductCommand(
        string Name,
        string Description,
       decimal Price,
       int Stock)
         : IRequest<ResponseHttp>
    {
        public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ResponseHttp>
        {
            private readonly IProductRepository ProductRepository;
            private readonly IMapper _mapper;

            public AddProductCommandHandler(IProductRepository productRepository, IMapper mapper)
            {
                ProductRepository = productRepository;
                _mapper = mapper;
            }

            public async Task<ResponseHttp> Handle(AddProductCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var Product = _mapper.Map<Product>(request);

                    Product = await ProductRepository.Post(Product);
                    await ProductRepository.SaveChange(cancellationToken);

                    return new ResponseHttp()
                    {
                        Resultat = _mapper.Map<ProductDTO>(Product),
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
