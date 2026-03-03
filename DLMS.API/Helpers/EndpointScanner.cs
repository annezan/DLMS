using Microsoft.AspNetCore.Mvc.Controllers;

namespace DLMS.API.Helpers
{
    public static class EndpointScanner
    {
        public static List<(string action, string controller, string method, string label)> GetAllEndpoints(IServiceProvider serviceProvider)
        {
            var endpointDataSource = serviceProvider.GetRequiredService<EndpointDataSource>();

            Console.WriteLine("🔎 Détection des endpoints...");

            var routeEndpoints = endpointDataSource
                .Endpoints
                .OfType<RouteEndpoint>()
                .Select(e =>
                {
                    var httpMethod = string.Join(',', e.Metadata.OfType<HttpMethodMetadata>().FirstOrDefault()?.HttpMethods ?? new List<string>());
                    var route = e.RoutePattern.RawText;
                    var controllerActionDescriptor = e.Metadata.OfType<ControllerActionDescriptor>().FirstOrDefault();

                    var controllerName = controllerActionDescriptor?.ControllerName ?? "Unknown";
                    var actionName = controllerActionDescriptor?.ActionName ?? "Unknown";

                    // Récupérer l'attribut PermissionLabel s'il existe
                    var permissionLabel = controllerActionDescriptor?.MethodInfo
                        .GetCustomAttributes(typeof(PermissionLabelAttribute), false)
                        .FirstOrDefault() as PermissionLabelAttribute;

                    var label = permissionLabel?.Label ?? $"{controllerName} - {actionName}"; // Par défaut, met Controller + Action

                    Console.WriteLine($"✅ Trouvé : {httpMethod}:{route} ({controllerName}.{actionName}) - Label : {label}");

                    return (action: $"{httpMethod}:{route}", controller: controllerName, method: actionName, label: label);
                })
                .ToList();

            Console.WriteLine($"🔎 {routeEndpoints.Count} endpoints trouvés.");

            return routeEndpoints;
        }

    }
}

