import styled from "styled-components";
import Poster from "../Poster";

const Card = styled.div`
	display: flex;
	flex-direction: column;
	align-items: center;
	padding: ${({ theme }) => theme.spacing(2)};
	border: 1px solid ${({ theme }) => theme.palette.grey[300]};
	border-radius: ${({ theme }) => theme.borderRadius.regular};
	background-color: ${({ theme }) => theme.palette.background.paper};
	box-shadow: ${({ theme }) => theme.shadows.default};
`;

const Title = styled.h2`
	${({ theme }) => theme.typography.h2}
	color: ${({ theme }) => theme.palette.text.primary};
	text-align: center;
	margin: ${({ theme }) => theme.spacing(1)} 0
		${({ theme }) => theme.spacing(0.5)};
`;

const ReleaseDate = styled.p`
	${({ theme }) => theme.typography.body2}
	color: ${({ theme }) => theme.palette.text.secondary};
	margin: 0 0 ${({ theme }) => theme.spacing(1)};
`;

const Rating = styled.p`
	${({ theme }) => theme.typography.body1}
	color: ${({ theme }) => theme.palette.primary.main};
	margin: 0;
`;

interface MovieCardProps {
	title: string;
	posterPath: string | null;
	releaseDate: string;
	rating: number;
}

const BaseMovieCard = ({
	title,
	posterPath,
	releaseDate,
	rating,
}: MovieCardProps) => {
	return (
		<Card>
			<Poster
				posterPath={
					posterPath && `https://image.tmdb.org/t/p/w300${posterPath}`
				}
				alt={`${title} Poster`}
			/>
			<Title>{title}</Title>
			<ReleaseDate>Release Date: {releaseDate || "N/A"}</ReleaseDate>
			<Rating>Rating: {rating}/10</Rating>
		</Card>
	);
};

export default BaseMovieCard;
