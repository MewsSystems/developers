import './movieDetail.css';
import { useEffect, useState } from 'react';
import Tag from '../tag/tag';
import { getMovieById } from '../../data/getMovies';

export default function MovieDetail({
  selectedMovieId,
  setSelectedMovie,
}: {
  selectedMovieId: number;
  setSelectedMovie: any;
}) {
  const [movie, setMovie] = useState(Object);

  useEffect(() => {
    getMovieById(selectedMovieId, setMovie);
  }, [selectedMovieId]);

  return (
    <>
      <h1>{movie.title}</h1>
      <div className="back_button_container">
        <button className="button" onClick={() => setSelectedMovie(0)}>
          back
        </button>
      </div>
      <div className="detail_container">
        {movie.poster_path && (
          <img
            src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
            className="movie_img"
          />
        )}
        <div className="detail_text">
          <div className="text_item">
            <h2>Original title</h2>
            <h3>{movie.original_title ? movie.original_title : '–'}</h3>
          </div>
          <div className="text_item">
            <h2>Genres</h2>
            <div className="genres_container">
              {movie.genres
                ? movie.genres.map((genre: any) => (
                    <Tag key={genre.id} name={genre.name} />
                  ))
                : '–'}
            </div>
          </div>
          <div className="text_item">
            <h2>Origin country</h2>
            <p>
              {movie.origin_country
                ? movie.origin_country.map((country: string) => (
                    <Tag key={country} name={country} />
                  ))
                : '–'}
            </p>
          </div>
          <div className="text_item">
            <h2>Original language</h2>
            {movie.original_language ? (
              <Tag name={movie.original_language} />
            ) : (
              '–'
            )}
          </div>
          <div className="text_item">
            <h2>Overview</h2>
            {movie.overview ? <p>{movie.overview}</p> : '–'}
          </div>
        </div>
      </div>
    </>
  );
}
