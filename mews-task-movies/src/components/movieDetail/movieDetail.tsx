import { SetStateAction, useEffect, useState } from 'react';
import { Movie, Genre } from '../../data/interfaces';
import Tag from '../tag/tag';
import { getMovieById } from '../../data/getMovies';
import './movieDetail.css';

export default function MovieDetail({
  selectedMovieId,
  setSelectedMovie,
}: {
  selectedMovieId: number;
  setSelectedMovie: React.Dispatch<SetStateAction<number>>;
}) {
  const [movie, setMovie] = useState<Movie>();

  useEffect(() => {
    getMovieById(selectedMovieId, setMovie);
  }, []);

  return (
    <>
      <h1>{movie?.title}</h1>
      <div className="back_button_container">
        <button className="button" onClick={() => setSelectedMovie(-1)}>
          back
        </button>
      </div>
      <div className="detail_container">
        {movie?.poster_path && (
          <img
            src={`${process.env.REACT_APP_IMG_BASE_PATH}${movie?.poster_path}`}
            className="movie_img"
          />
        )}
        <div className="detail_text">
          <div className="text_item">
            <h2>Original title</h2>
            <h3>{movie?.original_title ? movie?.original_title : '–'}</h3>
          </div>
          <div className="text_item">
            <h2>Genres</h2>
            <div className="text_item_container">
              {movie?.genres && movie?.genres.length > 0
                ? movie?.genres.map((genre: Genre) => (
                    <Tag key={genre.id} name={genre.name} />
                  ))
                : '–'}
            </div>
          </div>
          <div className="text_item">
            <h2>Origin country</h2>
            <div className="text_item_container">
              {movie?.origin_country
                ? movie?.origin_country.map((country: string) => (
                    <Tag key={country} name={country} />
                  ))
                : '–'}
            </div>
          </div>
          <div className="text_item">
            <h2>Original language</h2>
            <div className="text_item_container">
              {movie?.original_language ? (
                <Tag name={movie?.original_language} />
              ) : (
                '–'
              )}
            </div>
          </div>
          <div className="text_item overview">
            <h2>Overview</h2>
            {movie?.overview ? <p>{movie?.overview}</p> : '–'}
          </div>
        </div>
      </div>
    </>
  );
}
