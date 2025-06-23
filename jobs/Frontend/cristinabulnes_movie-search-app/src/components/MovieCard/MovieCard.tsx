import { forwardRef } from "react";
import styled from "styled-components";
import { Movie } from "../../types";
import BaseCard from "../BaseCard";
import Typography from "../Typography";

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

interface MovieCardProps extends Movie {
	onClick: () => void;
}

const MovieCard = forwardRef<HTMLDivElement, MovieCardProps>(
	(
		{
			id,
			title,
			posterPath,
			releaseDate,
			voteAverage,
			onClick,
		}: MovieCardProps,
		ref
	) => {
		return (
			<CardContainer
				onClick={onClick}
				tabIndex={0}
				role="button"
				aria-label={`View details of ${title}`}
				data-testid={`movie-card-${id}`}
				ref={ref}
			>
				<BaseCard size={1.5}>
					<BaseCard.Poster posterPath={posterPath} alt={`${title} Poster`} />
					<BaseCard.Content>
						<Typography variant="h5" color="text.primary">
							{title}
						</Typography>
						<Typography variant="body">Release Date: {releaseDate}</Typography>
						<Typography variant="subtitle" color="primary.main">
							Rating: {voteAverage || 0}/10
						</Typography>
					</BaseCard.Content>
				</BaseCard>
			</CardContainer>
		);
	}
);

export default MovieCard;
