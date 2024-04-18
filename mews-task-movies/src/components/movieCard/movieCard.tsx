import './movieCard.css';
import { Link } from 'react-router-dom';
export default function MovieCard({ movie }: { movie: any }) {
  return (
    <Link to={`/${movie.id}`} className="movie_card_container">
      {movie.poster_path && (
        <div className="movie_card_image_wrapper">
          <img
            src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
            className="movie_card_image"
            alt={`${movie.title} movie poster`}
          ></img>
        </div>
      )}
      <div className="description">
        <h2>{movie.title}</h2>
        <p className="card_overview">{movie.overview}</p>
        <div className="button detail_link">Movie detail</div>
      </div>
    </Link>
  );
}
