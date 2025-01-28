import React from 'react';
import { MovieListProps } from '../../types/movieTypes';
import { Grid, Card, CardContent, CardMedia, Typography, Box } from '@mui/material';
import { Link } from 'react-router-dom';
import '../../styles/MovieList.css';

const MovieList: React.FC<MovieListProps> = ({ movies, searchPerformed }) => {
    if (searchPerformed && movies.length === 0) {
        return (
        <Box sx={{ backgroundColor: '#212121', padding: '20px', borderRadius: '8px' }}>
            <Typography variant="body2" align="center" sx={{ color: '#fff' }}>
                No movies found
            </Typography>
        </Box>
    );
}

  return (
    <Grid container spacing={2}>
      {movies.map((movie) => (
        <Grid key={movie.id} item xs={12} sm={6} md={4}>
          <Link to={`/details/${movie.id}`} className='movie-list-link'>
          <Box className='movie-list-box'>
              <Card sx={{ display: 'flex', flexDirection: 'column', height: '100%' }}>
                {movie.poster_path ? (
                  <CardMedia
                    component="img"
                    height="500"
                    image={`https://image.tmdb.org/t/p/w500/${movie.poster_path}`}
                    alt={movie.title}
                  />
                ) : (
                  <div style={{ height: 500, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                    <Typography variant="body2">No Image Available</Typography>
                  </div>
                )}
                <CardContent sx={{ flexGrow: 1 }}>
                  <Typography variant="h6" gutterBottom>
                    {movie.title}
                  </Typography>
                  <Typography variant="body2" color="textSecondary" sx={{ flexGrow: 1 }}>
                    {movie.overview}
                  </Typography>
                </CardContent>
              </Card>
            </Box>
          </Link>
        </Grid>
      ))}
    </Grid>
  );
};

export default MovieList;
