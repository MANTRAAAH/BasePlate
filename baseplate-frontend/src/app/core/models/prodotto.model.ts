// prodotto.model.ts

export interface ProdottoReadDto {
  id: number;
  nome: string;
  descrizione: string;
  prezzo: number;
  nomeCategoria: string;
  ingredienti: string[];
  allergeni: string[];
  immagineUrl?: string;

}

export interface CreaProdottoDto {
  nome: string;
  descrizione: string;
  prezzo: number;
  categoriaId: number;
  ingredientiIds: number[];
  allergeniIds: number[];
  immagineUrl?: string;
}
// Interfaccia generica per ID e Nome (usata per Categorie, Ingredienti, ecc.)
export interface ElementoBase {
  id: number;
  nome: string;
}

// L'oggetto unico che ci restituisce l'endpoint /lookup
export interface LookupDati {
  categorie: ElementoBase[];
  allergeni: ElementoBase[];
  ingredienti: ElementoBase[];
}
