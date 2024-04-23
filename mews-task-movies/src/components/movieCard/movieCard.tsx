import { Movie } from '../../data/interfaces';
import './movieCard.css';

export default function MovieCard({
  movie,
  handleSelectedMovie,
}: {
  movie: Movie;
  handleSelectedMovie: (movieId: number) => void;
}) {
  return (
    <div
      onClick={() => handleSelectedMovie(movie.id)}
      className="movie_card_container"
    >
      <div className="description">
        <h2>{movie.title}</h2>
        <p className="card_overview">{movie.overview}</p>
        <div className="button detail_link">Movie detail</div>
      </div>
      {movie.poster_path && (
        <div className="movie_card_image_wrapper">
          <img
            src={`${process.env.REACT_APP_IMG_BASE_PATH}${movie.poster_path}`}
            className="movie_card_image"
            alt={`${movie.title} movie poster`}
          ></img>
        </div>
      )}
    </div>
  );
}
