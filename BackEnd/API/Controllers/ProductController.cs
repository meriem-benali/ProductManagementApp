using Application.Features.ProductFeature.Commands;
using Application.Features.ProductFeature.Queries;
using Application.Features.ProductFeature.Validators;
using Application.Setting;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Add(AddProductCommand cmd)
        {
            try
            {
                ResponseHttp AddCustomerResult;
                AddProductCommandValidator validator = new();

                AddCustomerResult = validator.Validate(new ValidationContext<AddProductCommand>(cmd));

                if (AddCustomerResult.Status == StatusCodes.Status400BadRequest)
                {
                    return BadRequest(AddCustomerResult);
                }

                AddCustomerResult = await _mediator.Send(cmd);

                return Ok(AddCustomerResult);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateProductCommand cmd)
        {
            try
            {
                // Validate the command
                var validator = new UpdateProductCommandValidator();
                var validationResult = validator.Validate(cmd);

                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }

                // Process the command
                var updateResult = await _mediator.Send(cmd);

                if (updateResult.Status == StatusCodes.Status400BadRequest)
                {
                    return BadRequest(updateResult.Fail_Messages);
                }

                return Ok(updateResult.Resultat);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Update error: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(Guid id)
        {
            GetProductByIdQuery qr = new(id);
            var result = await _mediator.Send(qr);

            return Ok(result);
        }
        [HttpGet("")]
        public async Task<ActionResult> Get(int? pageNumber, int? pageSize)
        {
            var result = await _mediator.Send(new GetAllProductQuery(pageNumber, pageSize));

            return Ok(result);
        }
    }
}
