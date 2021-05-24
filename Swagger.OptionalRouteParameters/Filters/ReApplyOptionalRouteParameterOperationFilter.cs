using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Swagger.OptionalRouteParameters.Filters
{
	public class ReApplyOptionalRouteParameterOperationFilter : IOperationFilter
	{
		const string captureName = "routeParameter";

		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			var routeInfo = context.ApiDescription.ActionDescriptor.AttributeRouteInfo;

			string allParamsRegex = $"{{(?<{captureName}>\\w+)\\??}}";
			string regex = $"{{(?<{captureName}>\\w+)\\?}}";

			var matches = System.Text.RegularExpressions.Regex.Matches(routeInfo.Template, regex);
			var allParamMatches = System.Text.RegularExpressions.Regex.Matches(routeInfo.Template, allParamsRegex);

			foreach (System.Text.RegularExpressions.Match match in matches)
			{
				var name = match.Groups[captureName].Value;

				var parameter = operation.Parameters.FirstOrDefault(p => p.In == ParameterLocation.Path && p.Name == name);
				if (parameter != null)
				{
					parameter.AllowEmptyValue = true;
					parameter.Description = "Must check \"Send empty value\" or Swagger passes a comma for empty values otherwise";
					parameter.Required = false;
					//parameter.Schema.Default = new OpenApiString(string.Empty);
					parameter.Schema.Nullable = true;
				}
			}

			// any parameter represented in the operation that isn't represented in the template, remove it
			for (int paramIndex = operation.Parameters.Count-1; paramIndex >= 0; paramIndex--) //foreach (var parameter in operation.Parameters)
			{
				var parameter = operation.Parameters[paramIndex];
				var match = allParamMatches.FirstOrDefault(m => m.Groups[captureName].Value == parameter.Name);
				// if the route doesn't have this parameter, just remove it...
				if (match == null)
					operation.Parameters.Remove(parameter);
			}
		}
	}
}
