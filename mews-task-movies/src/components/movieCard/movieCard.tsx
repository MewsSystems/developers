import './movieCard.css';
import { Link } from 'react-router-dom';
export default function MovieCard({ movie }: { movie: any }) {
  return (
    <Link to={`/${movie.id}`} className="movie_card_container">
      <div className="movie_card_image_wrapper">
        <img
          src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
          className="movie_card_image"
        ></img>
      </div>
      <div className="description">
        <h2>{movie.title}</h2>
        <p>{movie.overview}</p>
        <Link to={`/${movie.id}`} className="button detail_link">
          Movie detail
        </Link>
      </div>
    </Link>
  );
}
