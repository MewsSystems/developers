import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import {
  Container,
  Typography,
  CardMedia,
  CircularProgress,
} from '@mui/material';
import { Movie } from '../../shared/interfaces/movie.interface';
import Feedback from '../../shared/components/feedback/feedback';
import FallBackImg from '../../assets/images/fallback-image.png';

const MovieDetailPage = () => {
  const { id } = useParams();
  const [movie, setMovie] = useState<Movie | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(false);

  useEffect(() => {
    const fetchMovie = async () => {
      setLoading(true);
      try {
        const response = await fetch(
          `https://api.themoviedb.org/3/movie/${id}?api_key=${process.env.THEMOVIEDB_API_KEY}`,
        );
        if (!response.ok) {
          setMovie(null);
          setError(true);
          return;
        }
        const result = await response.json();
        setMovie(result);
        setError(false);
      } catch {
        setMovie(null);
        setError(true);
      } finally {
        setLoading(false);
      }
    };

    fetchMovie();
  }, []);

  return (
    <Container
      maxWidth="lg"
      style={{
        minHeight: 'calc(100vh - 64px)',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
      }}
    >
      {error && (
        <Feedback
          title="Movie not found"
          subtitle="The movie you are looking for does not exists"
        />
      )}
      {loading && <CircularProgress size={60} />}
      {!loading && !!movie && (
        <div
          style={{
            display: 'flex',
            flexDirection: 'column',
            flexGrow: 1,
            marginTop: '32px',
          }}
        >
          <CardMedia
            component="img"
            image={`https://image.tmdb.org/t/p/w1280${movie.backdrop_path}`}
            alt="Backdrop"
            onError={({ currentTarget }) => {
              currentTarget.src = FallBackImg;
            }}
            sx={{ width: '100%', objectFit: 'cover', maxHeight: '50vh' }}
          />
          <div style={{ padding: '20px' }}>
            <Typography variant="h5" gutterBottom className="m-b-8">
              {movie.title}
            </Typography>
            <Typography
              variant="subtitle1"
              color="textSecondary"
              style={{ marginBottom: '8px' }}
            >
              Original Title: {movie.original_title}
            </Typography>
            <Typography variant="body1" className="m-b-8">
              Overview: {movie.overview}
            </Typography>
            <Typography variant="body1" className="m-b-8">
              Release Date: {movie.release_date}
            </Typography>
            <Typography variant="body1" className="m-b-8">
              Language: {movie.original_language}
            </Typography>
            <Typography variant="body1" className="m-b-8">
              Adult: {movie.adult ? 'Yes' : 'No'}
            </Typography>
            <Typography variant="body1" className="m-b-8">
              Vote Average: {movie.vote_average}
            </Typography>
            <Typography variant="body1" className="m-b-8">
              Vote Count: {movie.vote_count}
            </Typography>
          </div>
        </div>
      )}
    </Container>
  );
};

export default MovieDetailPage;
