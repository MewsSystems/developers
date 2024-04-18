import { Pagination } from './shared/pagination/Pagination.tsx';
import { MoviesListUl } from './MoviesList.styled';
import { MovieListItem } from './MovieListItem.tsx';
import { MovieModel } from '../interfaces/movieModel.ts';

interface MovieListProps {
    page: number,
    data: MovieModel.MovieList
}

export function MoviesList({page, data}: MovieListProps) {
    const {
        results,
        totalPages
    } = data;

    // TODO: remove
    console.log(results);

    const movieListItems = results.map(movie => <MovieListItem key={movie.id} {...movie} />);

    const showPagination = totalPages > 1;

    return (
        <>
            <MoviesListUl>
                {movieListItems}
            </MoviesListUl>
            {showPagination && <Pagination currentPage={page} numberOfPages={totalPages} />}
        </>
    );
}