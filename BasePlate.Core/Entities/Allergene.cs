using BasePlate.Core.Interfaces; // 👈 Importa l'interfaccia
namespace BasePlate.Core.Entities;



public class Allergene : TenantEntity
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;

    public ICollection<Ingrediente> Ingredienti { get; set; } = new List<Ingrediente>();
}