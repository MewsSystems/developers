export type Movie = {
	id: string;
	title: string;
	overview: string;
	yearReleased: string;
	posterUrl: string;
	cast: Array<{ name: string; character: string }>;
	genres: Array<string>;
	directors: Array<string>;
};

export type MovieSearchResult = {
	id: string;
	title: string;
	imageUrl?: string;
};

export type MovieSearchResponse = {
	currentPage: number;
	totalPages: number;
	results: Array<MovieSearchResult>;
};
