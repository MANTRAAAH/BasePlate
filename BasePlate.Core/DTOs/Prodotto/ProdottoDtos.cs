namespace BasePlate.Core.DTOs;

// DTO per ricevere i dati dal frontend
public class CreaProdottoDto
{
    public string Nome { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
    public decimal Prezzo { get; set; }
    public int CategoriaId { get; set; }

    // Lista di ID di ingredienti e allergeni associati
    public List<int> IngredientiIds { get; set; } = new();
    public List<int> AllergeniIds { get; set; } = new();

    // URL per future immagini (già pronto per il CMS)
    public string? ImmagineUrl { get; set; }
}

// DTO per restituire i dati al frontend (più dettagliato)
public class ProdottoReadDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
    public string? ImmagineUrl { get; set; } = string.Empty;
    public decimal Prezzo { get; set; }
    public string NomeCategoria { get; set; } = string.Empty;

    // Mostriamo nomi invece di soli ID per rendere la vita facile al frontend
    public List<string> Ingredienti { get; set; } = new();
    public List<string> Allergeni { get; set; } = new();
}