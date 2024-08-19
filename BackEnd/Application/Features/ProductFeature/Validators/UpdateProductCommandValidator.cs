using Application.Common.Constants;
using Application.Common.Validator;
using Application.Features.ProductFeature.Commands;
using FluentValidation;

namespace Application.Features.ProductFeature.Validators
{
    public class UpdateProductCommandValidator : ValidatorBase<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {

            RuleFor(v => v.Name).NotEmpty()
               .WithMessage(ValidationConstants.NameMustHasValue);

            RuleFor(v => v.Description).NotEmpty()
                .WithMessage(ValidationConstants.DescriptionMustHasValue);

            RuleFor(v => v.Price).NotEmpty();

            RuleFor(v => v.Price).NotEmpty();
        }

    }
    
}
