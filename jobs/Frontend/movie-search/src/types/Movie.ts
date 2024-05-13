type Movie = {
    id: number;
    title: string;
    tagline: string;
    overview: string;
    release_date: string;
    runtime: number;
    backdrop_path: string;
    vote_average: number;
    vote_count: number;
    genres: [{
        id: number,
        name: string
    }],
}

export default Movie;