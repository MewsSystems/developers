import { Box, Grid, List, Pagination } from '@mui/material';
import MovieSummary from '../../types/MovieSummary';
import MovieListItem from './MovieListItem';

const MovieList = ({
	movies,
	page,
	setPage,
	totalPages,
}: {
	movies: MovieSummary[];
	page: number;
	setPage: (page: number) => void;
	totalPages: number | undefined;
}) => {
	const handlePaginationChange = (event: React.ChangeEvent<unknown>, value: number) => {
		setPage(value);
	};

	return (
		<div data-test='movie-list'>
			<List sx={{ width: '100%', maxWidth: '100%', bgcolor: 'background.paper' }}>
				{movies?.map((movie) => (
					<Grid item xs={4} sm={4} key={movie.id} className='movie-item'>
						<MovieListItem movie={movie} />
					</Grid>
				))}
			</List>
			{movies?.length && (
				<Box display='flex' justifyContent='center' marginTop={'20px'}>
					<Pagination count={totalPages} page={page} onChange={handlePaginationChange} />
				</Box>
			)}
		</div>
	);
};

export default MovieList;
