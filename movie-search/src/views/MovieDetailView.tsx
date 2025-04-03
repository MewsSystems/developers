import {useParams} from "react-router-dom"
import {useMovieDetails} from "../api/useMovieDetails.ts"
import styled from "styled-components"
import {formatDate} from "../utils/formatDate.ts"
import {getPosterSrc} from "../utils/getPosterSrc.ts";
import {PageNotFound} from "./PageNotFound.tsx";

export const MovieDetailView = () => {
    const {id} = useParams()
    const {data: movie, isLoading, error} = useMovieDetails(Number(id))

    if (isLoading) return <p>Loading...</p>
    if (error) return <PageNotFound/>
    if (!movie) return <p>Movie not found</p>

    return (
        <Container>
            <Poster src={getPosterSrc(movie.poster_path)} alt={movie.title}/>
            <InfoContainer>
                <Title>
                    {movie.title}
                    {movie.vote_average > 0 && <Rating>⭐ {movie.vote_average.toFixed(1)}</Rating>}
                </Title>
                {movie.genres.length !== 0 && <Genres>
                    {movie.genres.map((genre) => (
                        <Genre key={genre.id}>{genre.name}</Genre>
                    ))}
                </Genres>}
                <Overview>{movie.overview}</Overview>
                {movie.runtime > 0 && <p>⏳ <strong>Runtime:</strong> {movie.runtime} min</p>}
                {movie.production_countries.length !== 0 &&
                    <p>🌎 <strong>Origin:</strong> {movie.production_countries[0].name}</p>}
                <p>📅 <strong>Release Date:</strong> {formatDate(movie.release_date)}</p>
            </InfoContainer>
        </Container>
    )
}

const Container = styled.div`
    padding: 20px;
    margin: auto;

    max-width: 1170px;

    display: flex;
    flex-direction: column-reverse;
    gap: 20px;

    @media (min-width: 768px) {
        flex-direction: row;
    }
`

const Poster = styled.img`
    width: 300px;
    border-radius: 10px;
`

const InfoContainer = styled.div`
    display: flex;
    flex-direction: column;
    gap: 24px;
`

const Title = styled.h1`
    font-size: 24px;
`

const Rating = styled.span`
    padding: 5px 10px;
    margin-left: 8px;

    display: inline-block;
    font-weight: bold;

    background: #ffcc0045;
    border-radius: 5px;
`

const Genres = styled.div`
    display: flex;
    gap: 10px;
`

const Genre = styled.span`
    padding: 5px 10px;
    background: #4444441a;
    border-radius: 5px;
`

const Overview = styled.p`
    line-height: 1.6;
`