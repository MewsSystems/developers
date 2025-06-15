import {useEffect, useState} from 'react';
import {useNavigate} from 'react-router-dom';

export default function SearchView() {
  const [query, setQuery] = useState('');
  const [page, setPage] = useState(1);
  const [results, setResults] = useState<any[]>([]);
  const [totalPages, setTotalPages] = useState(1);
  const [totalResults, setTotalResults] = useState(0);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      setError('');
      try {
        const endpoint = 'https://api.themoviedb.org/3/search/movie';
        const accessToken = `eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiIwMjZmMDc4ODVjOTg4MDk3ZjgzYjMzZDNlYWQ1NmUxNSIsIm5iZiI6MTc0NDIyOTc1NC43NTcsInN1YiI6IjY3ZjZkNTdhMzE3NzUyNzZkNmQ5MjY0ZiIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.xZthcXWnp7VjkhzpHfh9lb2_OoUwagBJCGE9az55_oU`;
        const res = await fetch(endpoint + '?' + new URLSearchParams({query: query, page: page.toString()}).toString(), {
          headers: new Headers({Authorization: `Bearer ${accessToken}`}),
        }).then(res => res.json());
        setResults(res['results']);
        setTotalPages(res['total_pages']);
        setTotalResults(res['total_results']);
        setPage(res['page']);
      } catch (err: any) {
        setError(err.message || 'Something went wrong');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [query, page]);

  return (
    <div className="p-6 space-y-4">
      <input
        className="border p-2 rounded w-full"
        type="text"
        placeholder="Search..."
        value={query}
        onChange={(e) => {
          setQuery(e.target.value);
          setPage(1); // reset to first page on new search
        }}
      />
      {loading && <div>Loading...</div>}
      {error && <div className="text-red-500">{error}</div>}
      {!loading && !error && (
        <>
          <div className="flex items-center justify-center w-full">Found {totalResults} items</div>
          <table className="w-full table-auto border-collapse border border-gray-300">
            <thead>
            <tr>
              <th className="border px-4 py-2 text-left">Name</th>
            </tr>
            </thead>
            <tbody>
            {results.map(item => (
              <tr
                key={item.id}
                className="cursor-pointer hover:bg-gray-100"
                onClick={() => navigate(`/details/${item.id}`)}
              >
                <td className="border px-4 py-2">{item['original_title']}</td>
              </tr>
            ))}
            </tbody>
          </table>

          <div className="flex gap-2 items-center mt-2">
            <button
              disabled={page <= 1}
              onClick={() => setPage(p => p - 1)}
              className="px-3 py-1 border rounded disabled:opacity-50"
            >
              Prev
            </button>
            <span>Page {page} of {totalPages}</span>
            <button
              disabled={page >= totalPages}
              onClick={() => setPage(p => p + 1)}
              className="px-3 py-1 border rounded disabled:opacity-50"
            >
              Next
            </button>
          </div>
        </>
      )}
    </div>
  );
}
