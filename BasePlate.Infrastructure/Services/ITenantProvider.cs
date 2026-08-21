using BasePlate.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BasePlate.Api.Services; // Controlla che il namespace sia il tuo

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

        // 👑 1. PRIORITÀ ASSOLUTA: L'header manuale (God Mode / Impersonation)
        if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var headerValue))
        {
            if (Guid.TryParse(headerValue, out var tenantId))
            {
                // TODO in produzione: Qui andrebbe aggiunto un controllo di sicurezza
                // per accettare l'header SOLO se l'utente attuale ha il ruolo "SuperAdmin".
                return tenantId;
            }
        }

        // 🍕 2. FALLBACK NORMALE: Leggiamo il claim dal token JWT (Camerieri e Manager)
        var claim = context.User?.FindFirst("tenantId")?.Value;
        if (!string.IsNullOrEmpty(claim) && Guid.TryParse(claim, out var jwtTenantId))
        {
            return jwtTenantId;
        }

        // 3. Nessun tenant trovato (es. chiamata anonima)
        return Guid.Empty;
    }
}