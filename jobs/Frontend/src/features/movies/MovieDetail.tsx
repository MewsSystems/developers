import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import styled from "styled-components";
import { findMovie } from "../../common/api";
import type { Movie } from "../../common/movie";
import Image from "../../components/Image";
import { Tile, TileContainer } from "../../components/Tile";
import Button from "../../components/Button";
import screen from "../../common/screen";

const Layout = styled.main`
	display: grid;
	background-color: hsla(0, 0%, 85%, 0.7);
	border-radius: 0.625rem;
	box-shadow: 0px 0px 0.5rem 0.25rem rgba(0, 0, 0, 0.07);
	padding: 1rem;
	margin-block-start: 1rem;
	gap: 1rem;
	grid-template-areas:
		"title"
		"poster"
		"details";

	@media ${screen.M} {
		grid-template-areas:
			"title title"
			"poster details"
			"poster details";
	}
`;

const Title = styled.h2`
	font-size: 1.5rem;
	color: #333333;
	font-weight: bold;
	grid-area: title;
	line-height: 1;
	margin: 0;
`;

const Year = styled.span`
	color: #444444;
	font-weight: normal;
`;

const Poster = styled(Image)`
	grid-area: poster;
`;

const Details = styled.div`
	grid-area: details;
	display: flex;
	flex-direction: column;
	gap: 1rem;
`;

const Detail = styled.div`
	display: flex;
	flex-direction: column;
`;
const Label = styled.span`
	font-weight: bold;
`;

const Text = styled.span``;

export default function MovieDetail() {
	const navigate = useNavigate();
	const { movieId } = useParams();
	const [movie, setMovie] = useState<Movie>();

	useEffect(() => {
		findMovie(movieId!).then(setMovie);
	}, [movieId, setMovie]);

	if (!movie) {
		return <p>Loading...</p>;
	}

	const imgSrc = movie.posterUrl ? `https://image.tmdb.org/t/p/w500${movie.posterUrl}` : null;
	return (
		<>
			<Button variant="Back" text="Go Back" onClick={() => navigate("/")} />
			<Layout>
				<Title>
					{movie.title} <Year>({movie.yearReleased})</Year>
				</Title>
				{imgSrc ? <Poster src={imgSrc} alt="poster" /> : null}
				<Details>
					<Detail>
						<Label>Overview</Label>
						<Text>{movie.overview}</Text>
					</Detail>
					<Detail>
						<Label>Genres</Label>
						<TileContainer>
							{movie.genres.map((g) => (
								<Tile key={g} title={g} />
							))}
						</TileContainer>
					</Detail>
					<Detail>
						<Label>Director</Label>
						<Text>{movie.directors.join(", ")}</Text>
					</Detail>
					<Detail>
						<Label>Cast</Label>
						<TileContainer>
							{movie.cast.map(({ name, character }) => (
								<Tile key={name + character} title={name} subtitle={character} />
							))}
						</TileContainer>
					</Detail>
				</Details>
			</Layout>
		</>
	);
}
