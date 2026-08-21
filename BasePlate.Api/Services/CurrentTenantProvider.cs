using BasePlate.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BasePlate.Api.Services; // Metti il tuo namespace corretto

public class CurrentTenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentTenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetTenantId()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return Guid.Empty;

        // 👑 1. PRIORITÀ ASSOLUTA: God Mode / Impersonation (Header)
        if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var headerValue))
        {
            if (Guid.TryParse(headerValue, out var tenantId))
            {
                return tenantId;
            }
        }

        // 🍕 2. FALLBACK NORMALE: JWT Token
        var claim = context.User?.FindFirst("tenantId")?.Value;
        if (!string.IsNullOrEmpty(claim) && Guid.TryParse(claim, out var jwtTenantId))
        {
            return jwtTenantId;
        }

        return Guid.Empty;
    }
}