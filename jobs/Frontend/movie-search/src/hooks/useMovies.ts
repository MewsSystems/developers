import { useEffect, useState } from "react";
import axios from "axios";
import { useParams } from "react-router";
import Movie from "../types/Movie";
import { API_BASE_URL, API_KEY } from "../constants";
import MovieSummary from "../types/MovieSummary";

export const useMovies = () => {
    const { id } = useParams<string>();
    const [movie, setMovie] = useState<Movie>();
    const [movies, setMovies] = useState<MovieSummary[]>();
    const [query, setQuery] = useState<string>('');
    const [page, setPage] = useState<number>(1);
    const [totalPages, setTotalPages] = useState<number>();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(false);

    useEffect(() => {
        const fetchMovies = async (query: string, page: number) => {
            setError(false);
            setLoading(true);
            try {
                const response = await axios.get(`${API_BASE_URL}/search/movie`, {
                    params: {
                        api_key: API_KEY,
                        query: query,
                        page: page
                    },
                });
                setMovies(response.data.results);
                setTotalPages(response.data.total_pages);
            } catch (e) {
                setError(true);
            } finally {
                setLoading(false);
            }
        };

        fetchMovies(query, page);
    }, [page, query]);

    useEffect(() => {
        const fetchDetail = async () => {
            setError(false);
            setLoading(true);
            try {
                const response = await axios.get(`${API_BASE_URL}/movie/${id}`, {
                    params: {
                        api_key: API_KEY
                    },
                });
                setMovie(response.data);
            } catch (e) {
                setError(true);
            } finally {
                setLoading(false);
            }
        };

        fetchDetail();
    }, [id]);

    return {
        movie,
        movies,
        loading,
        error,
        query,
        page,
        totalPages,
        setQuery,
        setPage
    };
};