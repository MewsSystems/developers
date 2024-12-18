// MoviesGrid component: renders a list of MovieCard components
// Currently used for displaying search results, but can be reused for
// other movie-related lists like "popular movies" or "top-rated".

import React, { useRef } from "react";
import { useNavigate } from "react-router-dom";
import styled from "styled-components";
import MovieCard from "../MovieCard";
import { Movie } from "../../types";
import { useIntersectionObserver } from "../../hooks/useIntersectionObserver";

const MoviesGridContainer = styled.div`
	display: grid;
	grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
	gap: 16px;
	padding: 16px;
`;

interface MoviesGridProps {
	movies: Movie[];
	hasMore: boolean;
	loadMore: () => void;
}

const MoviesGrid = ({ movies, hasMore, loadMore }: MoviesGridProps) => {
	const navigate = useNavigate();
	const lastMovieRef = useRef<HTMLDivElement | null>(null);

	const handleMovieClick = (id: string) => {
		navigate(`/movie/${id}`);
	};

	useIntersectionObserver(() => {
		if (hasMore) {
			loadMore();
		}
	}, lastMovieRef);

	return (
		<MoviesGridContainer>
			{movies.map((movie, index) => (
				<MovieCard
					onClick={() => handleMovieClick(movie.id)}
					ref={index === movies.length - 1 ? lastMovieRef : undefined}
					id={movie.id}
					key={movie.id}
					title={movie.title}
					posterPath={movie.posterPath}
					releaseDate={movie.releaseDate}
					voteAverage={movie.voteAverage}
				/>
			))}
		</MoviesGridContainer>
	);
};

export default React.memo(MoviesGrid);
