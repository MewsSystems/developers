import MovieSummary from '../../types/MovieSummary';
import {
	IMAGE_POSTER_THUMBNAIL_URL,
	IMAGE_THUMBNAIL_UNAVAILABLE_URL,
	MOVIE_DETAIL_PATH,
	OVERVIEW_UNAVAILABLE_TEXT,
} from '../../constants';
import {
	Card,
	CardMedia,
	CardContent,
	Box,
	Divider,
	Link,
	Rating,
	Typography,
} from '@mui/material';
import { ArrowForwardIos } from '@mui/icons-material';

const MovieListItem = ({ movie }: { movie: MovieSummary }) => {
	const overview = movie.overview ? movie.overview : OVERVIEW_UNAVAILABLE_TEXT;
	const rating = movie.vote_average / 2;
	const imagePath = movie.poster_path
		? `${IMAGE_POSTER_THUMBNAIL_URL}/${movie.poster_path}`
		: IMAGE_THUMBNAIL_UNAVAILABLE_URL;
	const detailPath = `${MOVIE_DETAIL_PATH}${movie.id}`;

	return (
		<Card
			elevation={0}
			sx={{
				display: 'flex',
				padding: 2,
				borderRadius: '16px',
			}}>
			<Link href={detailPath}>
				<CardMedia
					component={'img'}
					image={imagePath}
					alt={movie.title}
					sx={{
						width: '92px',
						height: '138px',
						flexShrink: 0,
						backgroundColor: 'grey.200',
						borderRadius: '12px',
						boxShadow: '0 2px 8px 0 #c1c9d7, 0 -2px 8px 0 #cce1e9',
					}}
				/>
			</Link>
			<CardContent
				sx={{
					pr: 2,
				}}>
				<Link
					href={detailPath}
					variant='h3'
					marginRight={'20px'}
					sx={{
						fontSize: 24,
						color: 'primary.main',
						opacity: 0.87,
						textDecoration: 'none',
						'&:hover, &:focus': {
							textDecoration: 'underline',
							color: 'primary.dark',
							opacity: 1,
							'& $icon': {
								opacity: 1,
							},
						},
					}}>
					{movie.title}
				</Link>
				<Rating
					name={'rating'}
					value={rating}
					size={'medium'}
					sx={{
						verticalAlign: 'text-bottom',
					}}
				/>
				<Typography gutterBottom variant='body2' color='text.secondary'>
					{overview}
				</Typography>
				<Divider
					sx={{
						mt: 1,
						mb: 1,
					}}
				/>
				<Box
					sx={{
						display: 'flex',
						alignItems: 'center',
					}}>
					<Link
						href={detailPath}
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
						}}>
						Read more{' '}
						<ArrowForwardIos
							sx={{
								opacity: 0.6,
								fontSize: '1.125em',
								verticalAlign: 'middle',
								'&:first-child': {
									marginRight: 1,
								},
								'&:last-child': {
									marginLeft: 1,
								},
							}}
						/>
					</Link>
				</Box>
			</CardContent>
		</Card>
	);
};

export default MovieListItem;
