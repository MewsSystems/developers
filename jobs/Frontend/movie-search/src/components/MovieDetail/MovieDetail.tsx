import Movie from '../../types/Movie';
import formatDate from '../../utilities/DateFormatter';
import {
	DATE_INPUT_FORMAT,
	DATE_OUTPUT_FORMAT,
	IMAGE_BACKGROUND_UNAVAILABLE_URL,
	IMAGE_ORIGINAL_URL,
	SEARCH_VIEW_ROUTE_PATH,
} from '../../constants';
import {
	Box,
	Button,
	Card,
	CardContent,
	CardMedia,
	Divider,
	Link,
	Rating,
	Typography,
} from '@mui/material';

const MovieDetail = ({ movie }: { movie: Movie }) => {
	const rating = movie.vote_average / 2;
	const imagePath = movie.backdrop_path ? movie.backdrop_path : IMAGE_BACKGROUND_UNAVAILABLE_URL;

	return (
		<Box display='flex' justifyContent='center'>
			<Card
				elevation={0}
				sx={{
					minWidth: '90%',
					maxWidth: '90%',
					bgcolor: 'background.paper',
					borderRadius: '40px',
				}}>
				<CardMedia
					sx={{ minHeight: 600 }}
					image={`${IMAGE_ORIGINAL_URL}/${imagePath}`}
					title={movie.title}
				/>
				<CardContent>
					<Typography variant='h4' component='div' marginRight={1.5} display='inline-block'>
						{movie.title}
					</Typography>
					<Typography gutterBottom variant='h6' component='div'>
						{movie.tagline}
					</Typography>
					<Typography gutterBottom variant='body2' color='text.secondary'>
						Release date: {formatDate(movie.release_date, DATE_INPUT_FORMAT, DATE_OUTPUT_FORMAT)}
					</Typography>
					<Typography gutterBottom variant='body2' color='text.secondary'>
						Runtime: {movie.runtime}m
					</Typography>
					<div>
						{movie.genres.map((genre) => (
							<Button
								key={genre.id}
								variant='outlined'
								sx={{
									borderRadius: '20px',
									verticalAlign: 'text-bottom',
									marginRight: '5px',
									marginBottom: '4px',
								}}>
								{genre.name}
							</Button>
						))}
					</div>
					<Typography gutterBottom variant='body1' color='text.secondary'>
						{movie.overview}
					</Typography>
					<Rating name={'rating'} value={rating} size={'medium'} />
					<Typography gutterBottom variant='body2' color='text.secondary' marginLeft={'3px'}>
						{`${movie.vote_count} votes`}
					</Typography>
					<Divider
						sx={{
							mt: 1,
							mb: 1,
						}}
					/>
					<Link
						variant='body2'
						sx={{
							fontSize: 14,
							color: 'primary.light',
							opacity: 0.87,
							'&:hover, &:focus': {
								color: 'primary.main',
								opacity: 1,
								'& $icon': {
									opacity: 1,
								},
							},
						}}
						href={SEARCH_VIEW_ROUTE_PATH}>
						New search
					</Link>
				</CardContent>
			</Card>
		</Box>
	);
};

export default MovieDetail;
