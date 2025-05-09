export interface CollectionsResponse {
  collections: Collection[];
}

export interface Collection {
  id: string;
  name: string;
  description: string;
  imageUrl: string;
  items: CollectionItem[];
}

export interface CollectionItem {
  id: string;
  name: string;
  description: string;
  imageUrl: string;
  itemType: number;
  details: Details;
}

export interface Details {
  tmdbId: number;
  imdbId: string;
  releaseDate: string;
  //   sourceJson: any; // or 'unknown' depending on how you want to handle it
}
