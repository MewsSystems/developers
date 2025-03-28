import {useParams} from "react-router-dom"
import {useMovieDetails} from "../api/useMovieDetails.ts"
import styled from "styled-components"
import {formatDate} from "../utils/formatDate.ts"

export const MovieDetailView = () => {
    const {id} = useParams()
    const {data: movie, isLoading, error} = useMovieDetails(Number(id))

    if (isLoading) return <p>Loading...</p>
    if (error) return <p>Error loading movie details</p>
    if (!movie) return <p>Movie not found</p>

    return (
        <Container>
            <Poster src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`} alt={movie.title}/>
            <InfoContainer>
                <Title>
                    {movie.title} <Rating>⭐ {movie.vote_average.toFixed(1)}</Rating>
                </Title>
                <Genres>
                    {movie.genres.map((genre) => (
                        <Genre key={genre.id}>{genre.name}</Genre>
                    ))}
                </Genres>
                <Overview>{movie.overview}</Overview>
                <p>⏳ <strong>Runtime:</strong> {movie.runtime} min</p>
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
    gap: 20px;
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