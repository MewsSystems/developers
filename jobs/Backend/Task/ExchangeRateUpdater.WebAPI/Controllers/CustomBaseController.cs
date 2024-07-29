using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.WebAPI.Controllers
{
    /// <summary>
    /// Custom Base Controller is used as the base class for all other Web API controllers
    /// This handles routing and adding the ApiController decorator in a single location.
    /// Also allows for any additional functionality that is shared across all controllers
    /// </summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
    }
}
