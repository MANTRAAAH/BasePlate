namespace BasePlate.Core.DTOs.Prodotti;

// DTO per la creazione di una categoria
public class CreaCategoriaDto
{
    public string Nome { get; set; } = string.Empty;
}

// DTO per la lettura/visualizzazione
public class CategoriaReadDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
}