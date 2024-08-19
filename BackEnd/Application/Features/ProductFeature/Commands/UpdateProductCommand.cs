using Application.Features.ProductFeature.Dtos;
using Application.Interfaces;
using Application.Setting;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.ProductFeature.Commands
{
    public record class UpdateProductCommand(
       Guid ProductId,
       string Name,
       string Description,
       decimal Price,
       int Stock)
        : IRequest<ResponseHttp>
    {
        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ResponseHttp>
        {
            private readonly IProductRepository ProductRepository;
            private readonly IMapper _mapper;

            public UpdateProductCommandHandler(IProductRepository ProductRepository, IMapper mapper)
            {
                this.ProductRepository = ProductRepository;
                _mapper = mapper;
            }

            public async Task<ResponseHttp> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                Product? product = await ProductRepository.GetById(request.ProductId);

                if (product == null)
                {
                    return new ResponseHttp
                    {
                        Resultat = this._mapper.Map<ProductDTO>(product),
                        Fail_Messages = "Product with this Id not found.",
                        Status = StatusCodes.Status400BadRequest,
                    };
                }
                else
                {
                    product.Id = request.ProductId;
                    product.Name = request.Name;
                    product.description = request.Description;
                    product.price = request.Price;
                    product.stock = request.Stock;
                    await ProductRepository.Update(product);
                    await ProductRepository.SaveChange(cancellationToken);

                    var customerToReturn = _mapper.Map<ProductDTO>(product);
                    return new ResponseHttp
                    {
                        Resultat = customerToReturn,
                        Status = StatusCodes.Status200OK,
                    };

                }

            }
        }
    }
}


