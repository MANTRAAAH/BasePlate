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
