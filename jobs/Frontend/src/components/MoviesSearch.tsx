import InfiniteScroll from 'react-infinite-scroll-component'

import { useMovies } from '@/state/movies'
import { type Movie, type MovieApiResponse } from '@/api/types'
import MoviesList from './MoviesList'

export default function MoviesSearch() {
	const { status, data, fetchNextPage, hasNextPage } = useMovies()

	const movies = data?.pages?.reduce((moviesList: Movie[], currentPage: MovieApiResponse<Movie[]>) => {
		return [...moviesList, ...currentPage.results]
	}, [])

	return (
		<div>
			<InfiniteScroll
				dataLength={movies?.length || 0}
				next={fetchNextPage}
				hasMore={hasNextPage}
				loader={<div>LOADING.... loader</div>}
				endMessage={status !== 'pending' &&
					<p style={{ textAlign: 'center' }}>
						<strong>
							{movies?.length ? 'Yay! You have seen it all' : 'No search result'}
						</strong>
					</p>
					
				}
			>
				{movies && <MoviesList movies={movies} />}
			</InfiniteScroll>
		</div>
	)
}
