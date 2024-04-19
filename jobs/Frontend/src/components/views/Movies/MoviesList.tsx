import { Pagination } from '../../shared/pagination/Pagination.tsx';
import { MoviesListUl } from './MoviesList.styled.tsx';
import { MovieListItem } from './MovieListItem.tsx';
import { MovieModel } from '../../../interfaces/movieModel.ts';
import { Spacer } from '../../../enums/style/spacer.ts';

interface MovieListProps {
    page: number,
    data: MovieModel.MovieList
}

export function MoviesList({page, data}: MovieListProps) {
    const {
        results,
        totalPages
    } = data;

    if (results.length === 0) {
        return <h2 style={{paddingTop: Spacer.Lg}}>No movies found</h2>
    }

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