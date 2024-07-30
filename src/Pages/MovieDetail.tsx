import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import axios from 'axios';
import './movieDetail.css';

const API_KEY = '03b8572954325680265531140190fd2a';

interface Movie {
  id: number;
  title: string;
  overview: string;
  backdrop_path: string;
}

const MovieDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [movie, setMovie] = useState<Movie | null>(null);

  useEffect(() => {
    document.body.classList.add('detail-background');
    return () => {
      document.body.classList.remove('detail-background');
    };
  }, []);

  useEffect(() => {
    const fetchMovie = async () => {
      try {
        const response = await axios.get(
          `https://api.themoviedb.org/3/movie/${id}`,
          {
            params: {
              api_key: API_KEY,
            },
          },
        );
        setMovie(response.data);
      } catch (error) {
        console.error('Error fetching movie details:', error);
      }
    };

    fetchMovie();
  }, [id]);

  if (!movie) {
    return <div>Loading...</div>;
  }

  return (
    <div className='detail'>
      {movie.backdrop_path && (
        <img
          src={`https://image.tmdb.org/t/p/w500${movie.backdrop_path}`}
          alt={`${movie.title} backdrop`}
          loading="lazy"
        />
      )}
      <h1>{movie.title}</h1>
      <p>{movie.overview}</p>
    </div>
  );
};

export default MovieDetail;
