import React, { useEffect, useState } from 'react';
import { Link, useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import './movieDetail.css';

const API_KEY = '03b8572954325680265531140190fd2a';

interface Movie {
  id: number;
  title: string;
  overview: string;
  backdrop_path: string;
  genres: Genres[];
  release_date: string;
  original_language: string;
  runtime: number;
}

interface Genres {
  id: number;
  name: string;
}

const MovieDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
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

  const convertTime = (totalMinutes: number) => {
    const hours = Math.floor(totalMinutes / 60);
    const minutes = totalMinutes % 60;
    return `${hours}h ${minutes}m`;
  };

  const handleBackClick = () => {
    navigate(-1); 
  };

  return (
    <div className='detail'>
      {/* Instead of a button, I would use <Link> and invent a function where the search status would be saved, so that when you go back, the list is shown again and the search is not reset.  */}
      <button onClick={handleBackClick} className='back'>Back</button>
      {movie.backdrop_path && (
        <img
          src={`https://image.tmdb.org/t/p/w500${movie.backdrop_path}`}
          alt={`${movie.title} backdrop`}
        />
      )}

      <h1>{movie.title}</h1>
      <p className='genre-list'>
        {movie.genres.map((genre) => (
          <span className='genre' key={genre.id}>{genre.name}</span>
        ))}
        <strong>{movie.original_language}</strong> {convertTime(movie.runtime)}
      </p>
      <p><strong>Release date: </strong>{movie.release_date}</p>
      <p className='overview'>{movie.overview}</p>
      
    </div>
  );
};

export default MovieDetail;
