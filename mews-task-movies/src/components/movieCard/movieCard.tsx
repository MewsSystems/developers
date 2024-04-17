import { Link } from 'react-router-dom';
export default function MovieCard({ movie }: { movie: any }) {
  return (
    <div>
      <div>
        <img src={movie.poster_path}></img>
      </div>
      <h2>{movie.title}</h2>
      <p>{movie.overview}</p>
      <Link to={`/${movie.id}`}>Movie detail</Link>
    </div>
  );
}
