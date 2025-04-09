import { useParams, Link } from 'react-router-dom';
import {useEffect, useState} from "react";

export default function DetailsView() {
  const { id } = useParams();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [item, setItem] = useState<any>(null);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      setError('');
      try {
        const endpoint = `https://api.themoviedb.org/3/movie/${id}`;
        const accessToken = `eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIwMjZmMDc4ODVjOTg4MDk3ZjgzYjMzZDNlYWQ1NmUxNSIsIm5iZiI6MTc0NDIyOTc1NC43NTcsInN1YiI6IjY3ZjZkNTdhMzE3NzUyNzZkNmQ5MjY0ZiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.xZthcXWnp7VjkhzpHfh9lb2_OoUwagBJCGE9az55_oU`;
        const res = await fetch(endpoint, {
          headers: new Headers({Authorization: `Bearer ${accessToken}`}),
        }).then(res => res.json());
        setItem(res);
      } catch (err: any) {
        setError(err.message || 'Something went wrong');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  return (
    <div className="p-6 space-y-4">
      {(loading || item === null) && <div>Loading...</div>}
      {error && <div className="text-red-500">{error}</div>}
      {!loading && !error && item !== null && (
        <>
          <h1 className="text-2xl font-bold">{item.title}</h1>
          <p>{item.original_title}</p>
          <p>{item.status}</p>
        </>
      )}
      <Link to="/" className="text-blue-600 underline">← Back to Search</Link>
    </div>
  );
}
