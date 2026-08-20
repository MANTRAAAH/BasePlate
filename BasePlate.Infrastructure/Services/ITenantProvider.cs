using Microsoft.AspNetCore.Http;

namespace BasePlate.Infrastructure.Services;

public interface ITenantProvider
{
    string GetTenantId();
}

public class HttpContextTenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetTenantId()
    {
        // Intercetta l'header X-Tenant-Id (utile per i test su Postman/Swagger)
        var tenantId = _httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-Id"].FirstOrDefault();

        if (string.IsNullOrEmpty(tenantId))
        {
            throw new UnauthorizedAccessException("TenantId mancante. Accesso al Multi-Tenant negato.");
        }

        return tenantId;
    }
}