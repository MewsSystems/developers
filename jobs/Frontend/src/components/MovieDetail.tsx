import { MovieModel } from '../interfaces/movieModel.ts';

export function MovieDetail({ title, overview }: MovieModel.SearchMovieItem) {
    return (
        <>
            <h1>{title}</h1>
            <p>{overview}</p>
        </>
    );
}