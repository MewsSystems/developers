import styled from "styled-components";
import BaseMovieCard, { BaseMovieCardProps } from "../BaseMovieCard";

const CardContainer = styled.div`
	display: flex;
	cursor: pointer;
	transition: transform 0.2s, box-shadow 0.2s;

	&:hover {
		will-change: transform, box-shadow;
		transform: scale(1.02);
		box-shadow: ${({ theme }) => theme.shadows.hover};
	}

	&:focus-within {
		box-shadow: ${({ theme }) => theme.shadows.focus};
		outline: none;
	}
`;

interface MovieCardProps extends BaseMovieCardProps {
	onClick: () => void;
}

const MovieCard = ({
	id,
	title,
	posterPath,
	releaseDate,
	rating,
	onClick,
}: MovieCardProps) => {
	return (
		<CardContainer
			onClick={onClick}
			tabIndex={0}
			role="button"
			aria-label={`View details of ${title}`}
		>
			<BaseMovieCard
				id={id}
				title={title}
				posterPath={posterPath}
				releaseDate={releaseDate}
				rating={rating}
			/>
		</CardContainer>
	);
};

export default MovieCard;
