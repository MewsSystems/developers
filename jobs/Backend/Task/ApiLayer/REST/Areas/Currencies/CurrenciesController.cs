using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationLayer.Queries.Currencies.GetAllCurrencies;
using ApplicationLayer.Queries.Currencies.GetCurrencyByCode;
using ApplicationLayer.Queries.Currencies.GetCurrencyById;
using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.Currencies.DeleteCurrency;
using REST.Response.Models.Common;
using REST.Response.Models.Areas.Currencies;
using REST.Response.Converters;
using REST.Request.Models.Areas.Currencies;

namespace REST.Areas.Currencies;

/// <summary>
/// API endpoints for currency management.
/// </summary>
[ApiController]
[Area("Currencies")]
[Route("api/currencies")]
[Authorize(Roles = "Consumer,Admin")]
public class CurrenciesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CurrenciesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all currencies.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CurrencyResponse>>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetAllCurrenciesQuery(PageNumber: 1, PageSize: 100, IncludePagination: false);
        var pagedResult = await _mediator.Send(query);

        var response = ApiResponse<IEnumerable<CurrencyResponse>>.Ok(
            pagedResult.Items.Select(c => c.ToResponse()),
            "Currencies retrieved successfully"
        );

        return Ok(response);
    }

    /// <summary>
    /// Get a specific currency by ID.
    /// </summary>
    /// <param name="id">Currency ID</param>
    [HttpGet("id/{id}")]
    [ProducesResponseType(typeof(ApiResponse<CurrencyResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetCurrencyByIdQuery(id);
        var currencyDto = await _mediator.Send(query);

        if (currencyDto != null)
        {
            var response = ApiResponse<CurrencyResponse>.Ok(
                currencyDto.ToResponse(),
                "Currency retrieved successfully"
            );
            return Ok(response);
        }

        return NotFound(ApiResponse<CurrencyResponse>.NotFound($"Currency with ID {id} not found"));
    }

    /// <summary>
    /// Get a specific currency by code.
    /// </summary>
    /// <param name="code">Currency code (ISO 4217, e.g., USD, EUR)</param>
    [HttpGet("{code}")]
    [ProducesResponseType(typeof(ApiResponse<CurrencyResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> GetByCode(string code)
    {
        var query = new GetCurrencyByCodeQuery(code);
        var currencyDto = await _mediator.Send(query);

        if (currencyDto != null)
        {
            var response = ApiResponse<CurrencyResponse>.Ok(
                currencyDto.ToResponse(),
                $"Currency {code} retrieved successfully"
            );
            return Ok(response);
        }

        return NotFound(ApiResponse<CurrencyResponse>.NotFound($"Currency with code '{code}' not found"));
    }

    /// <summary>
    /// Create a new currency.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<CurrencyResponse>), 201)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> Create([FromBody] CreateCurrencyRequest request)
    {
        var command = new CreateCurrencyCommand(request.Code);
        var result = await _mediator.Send(command);

        if (result.IsSuccess && result.Value > 0)
        {
            // Get the created currency to return it
            var getQuery = new GetCurrencyByCodeQuery(request.Code);
            var currencyDto = await _mediator.Send(getQuery);

            if (currencyDto != null)
            {
                var response = ApiResponse<CurrencyResponse>.Ok(
                    currencyDto.ToResponse(),
                    $"Currency {request.Code} created successfully"
                );
                return CreatedAtAction(nameof(GetByCode), new { code = request.Code }, response);
            }
        }

        return BadRequest(ApiResponse<CurrencyResponse>.BadRequest(
            result.Error ?? "Failed to create currency"
        ));
    }

    /// <summary>
    /// Delete a currency by code.
    /// </summary>
    /// <param name="code">Currency code to delete</param>
    [HttpDelete("{code}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> Delete(string code)
    {
        // First get currency ID from code
        var getQuery = new GetCurrencyByCodeQuery(code);
        var currencyDto = await _mediator.Send(getQuery);

        if (currencyDto == null)
        {
            return NotFound(ApiResponse.Fail($"Currency with code '{code}' not found", 404));
        }

        var command = new DeleteCurrencyCommand(currencyDto.Id);
        var result = await _mediator.Send(command);

        return Ok(result.ToApiResponse($"Currency {code} deleted successfully"));
    }
}
