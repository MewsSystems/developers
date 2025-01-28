import React, {useEffect} from 'react';
import { Dispatch } from 'redux';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';

import { Movie } from '../../types/movieTypes';
import { Typography, Card, CardContent, CardMedia, Grid, Divider, Chip, IconButton, Box } from '@mui/material';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import { getMovieDetails } from '../../store/actions/movies.actions';

import moment from 'moment';

interface MovieDetailsProps {
    movieId: number;
}

const MovieDetails: React.FC<MovieDetailsProps> = ({ movieId }) => {
    const navigate = useNavigate();
    const dispatch: Dispatch<any> = useDispatch();

    useEffect(() => {
        dispatch(getMovieDetails(movieId));
    }, [dispatch, movieId]);

    const movie: Movie | null = useSelector((state: any) => state.movies.movieDetails);

    const handleBack = () => {
        navigate(-1); // Go back one step in the history
      };

    if (!movie) {
    return <div>Loading...</div>;
    }

    const imageUrl = `https://image.tmdb.org/t/p/original${movie.poster_path}`;
    const formattedReleaseDate = moment(movie.release_date).format('DD-MM-YYYY');

    return (
        <Grid container justifyContent="center" alignItems="center" sx={{ minHeight: '100vh' }}>
            <Grid item xs={12} sm={10} md={8} lg={6}>
                <Card sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', padding: '20px', backgroundColor: 'white' }}>
                    <Grid container spacing={3}>
                        <Grid item xs={12} sm={5} sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center' }}>
                            {movie.poster_path ? (
                            <CardMedia
                                component="img"
                                sx={{ maxWidth: '100%', maxHeight: '400px', objectFit: 'contain' }}
                                image={imageUrl}
                                alt={movie.title}
                            />
                            ) : (
                                <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                                  <Typography variant="body2">No Image Available</Typography>
                                </div>
                              )}
                        </Grid>
                        <Grid item xs={12} sm={7}>
                            <CardContent>
                                <Box sx={{ display: 'flex', alignItems: 'center' }}>
                                    <Typography variant="h4" gutterBottom sx={{ fontWeight: 'bold', marginRight: 'auto', marginBottom: '10px' }}>
                                        {movie.title}
                                    </Typography>
                                    <IconButton
                                        sx={{ marginBottom: '10px', color: 'rgba(255, 196, 0, 0.8)' }}
                                        onClick={handleBack}
                                    >
                                        <ArrowBackIcon />
                                    </IconButton>
                                </Box>
                                <Typography variant="body2" color="text.secondary">
                                    <strong>Release Date:</strong> {formattedReleaseDate}
                                </Typography>
                                <Typography variant="body2" color="text.secondary">
                                    <strong>Rating:</strong> {movie.vote_average}
                                </Typography>
                                <Typography variant="body2" color="text.secondary" sx={{ marginTop: '10px', marginBottom: '10px' }}>
                                    <div style={{ display: 'flex', flexWrap: 'wrap', gap: '5px', alignItems: 'center' }}>
                                    <strong>Genres:</strong>
                                        {movie.genres.map((genre) => (
                                            <Chip key={genre.id} label={genre.name} sx={{ margin: '5px' }} />
                                        ))}
                                    </div>
                                </Typography>
                                <Typography variant="body2" color="text.secondary">
                                    <strong>Runtime:</strong> {movie.runtime} mins
                                </Typography>
                                <Divider sx={{ margin: '20px 0' }} />
                                <Typography variant="body2" color="text.secondary" sx={{ fontWeight: 'bold', marginTop: '10px', marginBottom: '5px' }}>
                                    Overview
                                </Typography>
                                <Typography variant="body2" color="text.secondary" sx={{ textAlign: 'justify' }}>
                                    {movie.overview}
                                </Typography>
                            </CardContent>
                        </Grid>
                    </Grid>
                </Card>
            </Grid>
        </Grid> 
    );
};

export default MovieDetails;
