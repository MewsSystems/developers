// Movie
export interface Movie {
	id: string;
	title: string;
	posterPath: string | null;
	releaseDate: string;
	voteAverage?: number;
	overview?: string;
	genres?: Genre[];
	backdropPath?: string | null;
	runtime?: number | null;
}

export interface Genre {
	id: number;
	name: string;
}
