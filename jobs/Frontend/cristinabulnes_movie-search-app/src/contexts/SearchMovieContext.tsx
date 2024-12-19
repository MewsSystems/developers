import React, {
	createContext,
	useContext,
	useState,
	ReactNode,
	useMemo,
} from "react";
import { Movie } from "../types";

interface SearchMovieContextType {
	query: string;
	setQuery: React.Dispatch<React.SetStateAction<string>>;
	movies: Movie[];
	setMovies: React.Dispatch<React.SetStateAction<Movie[]>>;
	isLoading: boolean;
	setIsLoading: React.Dispatch<React.SetStateAction<boolean>>;
	error: string | null;
	setError: React.Dispatch<React.SetStateAction<string | null>>;
	hasMore: boolean;
	setHasMore: React.Dispatch<React.SetStateAction<boolean>>;
	page: number;
	setPage: React.Dispatch<React.SetStateAction<number>>;
}

const SearchMovieContext = createContext<SearchMovieContextType>(
	{} as SearchMovieContextType
);

export const useSearchMovieContext = () => {
	const context = useContext(SearchMovieContext);
	if (!context) {
		throw new Error(
			"useSearchMovieContext must be used within a SearchMovieProvider"
		);
	}
	return context;
};

interface SearchMovieProviderProps {
	children: ReactNode;
}

export const SearchMovieProvider = ({ children }: SearchMovieProviderProps) => {
	const [query, setQuery] = useState<string>("");
	const [movies, setMovies] = useState<Movie[]>([]);
	const [isLoading, setIsLoading] = useState<boolean>(false);
	const [error, setError] = useState<string | null>(null);
	const [hasMore, setHasMore] = useState<boolean>(false);
	const [page, setPage] = useState<number>(1);

	const value = useMemo(
		() => ({
			query,
			setQuery,
			movies,
			setMovies,
			isLoading,
			setIsLoading,
			error,
			setError,
			page,
			setPage,
			hasMore,
			setHasMore,
		}),
		[query, movies, isLoading, error, page, hasMore]
	);

	return (
		<SearchMovieContext.Provider value={value}>
			{children}
		</SearchMovieContext.Provider>
	);
};
