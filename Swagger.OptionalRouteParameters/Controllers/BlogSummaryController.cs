using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Swagger.OptionalRouteParameters.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogSummaryController : ControllerBase
    {
        [HttpGet("{year}/{month}/{day?}")]
        // commented out since I have it applied globally
        //[SwaggerOperationFilter(typeof(Filters.ReApplyOptionalRouteParameterOperationFilter))]
        public async Task<IActionResult> Get([FromRoute]int year, [FromRoute]int month, [FromRoute]int? day = null)
        {
            await Task.CompletedTask;

            return Ok();
        }
    }
}