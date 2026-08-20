using System.Security.Claims;
using BasePlate.Core.Interfaces;
using Microsoft.AspNetCore.Http; // 👈 Necessario per leggere le richieste HTTP

namespace BasePlate.Api.Services;

public class CurrentTenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentTenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetTenantId()
    {
        // 1. Controlliamo se c'è una richiesta HTTP in corso e se l'utente è loggato
        var user = _httpContextAccessor.HttpContext?.User;

        if (user?.Identity?.IsAuthenticated != true)
        {
            // Nessun utente loggato (es. stiamo facendo Login o Registrazione)
            // Restituiamo Guid.Empty così il DbContext non sovrascrive nulla!
            return Guid.Empty;
        }

        // 2. Cerchiamo il claim "tenantId" che abbiamo stampato nel Token durante il Login
        var tenantClaim = user.FindFirst("tenantId")?.Value;

        // 3. Se lo troviamo ed è un Guid valido, lo restituiamo. Altrimenti vuoto.
        if (Guid.TryParse(tenantClaim, out var tenantId))
        {
            return tenantId;
        }

        return Guid.Empty;
    }
}