import { useState, useEffect } from "react";
import axios from "axios";

const API_KEY = "03b8572954325680265531140190fd2a";
const API_URL = "https://api.themoviedb.org/3/search/movie";

export const useMovies = (query, page) => {
  const [movies, setMovies] = useState([]);
  const [hasMore, setHasMore] = useState(false);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchMovies = async () => {
      setLoading(true);
      setError(null);

      try {
        const response = await axios.get(API_URL, {
          params: {
            api_key: API_KEY,
            query: query,
            page: page,
          },
        });

        if (page === 1) {
          setMovies(response.data.results);
        } else {
          setMovies((prevMovies) => [...prevMovies, ...response.data.results]);
        }

        setHasMore(page < response.data.total_pages);
      } catch (err) {
        setError("Error fetching data");
      } finally {
        setLoading(false);
      }
    };

    if (query.length > 0) {
      fetchMovies();
    } else {
      setMovies([]);
    }
  }, [query, page]);

  return { movies, hasMore, loading, error };
};
