import { useParams } from 'react-router-dom';
import './MovieDetail.css';

function MovieDetail() {
  const { id } = useParams<{ id: string }>();
  return (
    <div className='flex flex-column align-items-center'>
      DETAILS {id}
    </div>
  );
}

export default MovieDetail;
