// MoviesGrid component: renders a list of MovieCard components
// Currently used for displaying search results, but can be reused for
// other movie-related lists like "popular movies" or "top-rated".

import React from "react";
import { useNavigate } from "react-router-dom";
import styled from "styled-components";
import MovieCard from "../MovieCard";
import { Movie } from "../../types";

const MoviesGridContainer = styled.div`
	display: grid;
	grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
	gap: 16px;
	padding: 16px;
`;

const MoviesGrid = ({ movies }: { movies: Movie[] }) => {
	const navigate = useNavigate();

	const handleMovieClick = (id: string) => {
		navigate(`/movie/${id}`);
	};

	return (
		<MoviesGridContainer>
			{movies.map((movie) => (
				<MovieCard
					onClick={() => handleMovieClick(movie.id)}
					id={movie.id}
					key={movie.id}
					title={movie.title}
					posterPath={movie.posterPath}
					releaseDate={movie.releaseDate}
					rating={movie.voteAverage}
				/>
			))}
		</MoviesGridContainer>
	);
};

export default React.memo(MoviesGrid);
