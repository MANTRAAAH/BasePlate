using BasePlate.Core.Interfaces;

namespace BasePlate.Api.Services;

public class CurrentTenantProvider : ITenantProvider
{
    // Per ora restituiamo un Guid fisso di test. 
    // In futuro, qui leggeremo l'ID dal Token JWT o dall'Header della richiesta HTTP.
    public Guid GetTenantId()
    {
        return Guid.Parse("11111111-1111-1111-1111-111111111111");
    }
}