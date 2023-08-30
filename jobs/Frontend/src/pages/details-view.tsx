import { AppHeader } from "components/app-header";
import { BackgroundImage } from "components/background-image";
import { Internal } from "components/internal";
import { ButtonStyle } from "components/styled-button";
import { Poster } from "components/poster";
import { MovieDetails } from "models/movie-details";
import { useState, useEffect } from "react";
import { Link, useParams } from "react-router-dom";
import { MoviesDBService } from "services/movies-db";
import styled from "styled-components";

export const DetailsView = () => {
    const { id } = useParams<{ id: string }>();
    const emptyMovie: MovieDetails = { title: '', overview: '', vote_average: 0, release_date: '', poster_path: '', backdrop_path: '', vote_count: 0 };
    const [movie, setMovie] = useState<MovieDetails>(emptyMovie);

    useEffect(() => {
        MoviesDBService.getMovieById(id as string).then((movie) => setMovie(movie));
    }, [id]);

    const bgImage = Boolean(movie.backdrop_path)
        ? `https://image.tmdb.org/t/p/original${movie.backdrop_path}`
        : '/background.png';

    const overview = movie.overview.length > 0
        ? movie.overview
        : 'No overview available';

    const rating = movie.vote_average > 0
        ? movie.vote_average
        : 'N/A';

    const voteCount = movie.vote_count > 0
        ? ` (${movie.vote_count} votes)`
        : '';

    return (
        <BackgroundImage url={bgImage}>
            <Internal>
                <AppHeader />
                <h2>{movie.title}</h2>
                <Overview>{overview}</Overview>
                <p><b>Rating:</b> {rating}<small>{voteCount}</small></p>
                <p><b>Release date:</b> {movie.release_date}</p>
                <Poster title={movie.title} poster_path={movie.poster_path} />
                <ReturnLink to="/">Return to search results</ReturnLink>
            </Internal>
        </BackgroundImage>
    )
}

const Overview = styled.p`
    padding: 10px;
    width: 80%;
    font: 18px arial;
    border-radius: 5px;
    border: 1px solid lightgray;
    background-color: rgba(255, 255, 255, 0.3);
    box-shadow: 10px rgba(255, 255, 255, 0.3);
`;

const ReturnLink = styled(Link)`
    ${ButtonStyle}
    position: fixed;
    bottom: 10px;
    right: 20px;
    font-size: 20px;
`;