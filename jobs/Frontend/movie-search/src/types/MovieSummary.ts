type MovieSummary = {
    id: number;
    title: string;
    overview: string | undefined;
    poster_path: string | undefined;
    vote_average: number;
}

export default MovieSummary;