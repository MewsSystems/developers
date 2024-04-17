import './movieDetail.css';
import { Link } from 'react-router-dom';

export default function MovieDetail() {
  return (
    <main>
      <h1>Movie Detail</h1>
      <Link to={'/'} className="button">
        back
      </Link>
    </main>
  );
}
