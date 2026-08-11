namespace BasePlate.Core.Interfaces;

public interface ITenantProvider
{
    Guid GetTenantId();
}