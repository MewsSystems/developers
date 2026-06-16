import { useQuery } from '@tanstack/react-query';
import { ReactQueryPrimaryKey } from '../../../enums/reactQueryPrimaryKey';
import { trendingMovies } from '../../../api/tmdbApi';
import { Spacer } from '../../../enums/style/spacer';
import { Loader } from '../../shared/Loader';
import { MoviesList } from './MoviesList';
import { NetworkErrorMessage } from '../../shared/Error';

export function TrendingMovies({page}: { page: number }) {
    const timeWindow = 'day';
    const {data, isError, isPending} = useQuery({
        queryKey: [ReactQueryPrimaryKey.TmdbTendingMovies, timeWindow, page],
        queryFn: () => trendingMovies(timeWindow, page)
    });

    if (isPending) {
        return (
            <div style={{paddingTop: Spacer.Lg}}>
                <Loader>
                    <h2 style={{display: 'inline'}}>Loading top trending movies</h2>
                </Loader>
            </div>
        );
    }

    if (isError) {
        return <NetworkErrorMessage/>;
    }

    const {
        results
    } = data;

    return (
        <>
            <h2 style={{paddingTop: Spacer.Lg}}>Top trending today</h2>
            <MoviesList data={{results: results.slice(0, 8), totalPages: 1, totalResults: 1, page: 1}} page={page}/>
        </>
    );
}