import { getMovies } from "@movie/services/api/movie-api"
import { useService } from "./lib/useService";
import { Card } from "./lib/components/card/card";

export const App = () => {
  const { data, loading } = useService(getMovies);

  if (loading) return <div>Loading...</div>;
  if (!data) return <div>No data</div>;
  
  return (
    <div style={{ 
      padding: '2rem',
      display: 'grid',
      gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))',
      gap: '2rem'
    }}>
      {data.map((movie) => (
        <Card key={movie.id}>
          <Card.Image 
            src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`} 
            alt={movie.title}
          />
          <Card.Body>
            <Card.Title>{movie.title}</Card.Title>
            <Card.Description>{movie.overview}</Card.Description>
          </Card.Body>
          <Card.Footer>
            <small>Release Date: {movie.release_date}</small>
          </Card.Footer>
        </Card>
      ))}
    </div>
  );
};
