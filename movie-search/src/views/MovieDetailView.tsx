import {useParams} from "react-router-dom"
import {useMovieDetails} from "../api/useMovieDetails.ts"
import styled from "styled-components"
import {formatDate} from "../utils/formatDate.ts"
import {getPosterSrc} from "../utils/getPosterSrc.ts";
import {PageNotFound} from "./PageNotFound.tsx";
import {Loading, LoadingWrapper} from "../components/Loading.tsx";

export const MovieDetailView = () => {
    const {id} = useParams()
    const {data: movie, isLoading, error} = useMovieDetails(Number(id))

    if (isLoading) return <LoadingWrapper><Loading/></LoadingWrapper>
    if (!movie || error) return <PageNotFound/>

    const movieScore = movie.vote_average.toFixed(1)

    return (
        <Container role="region" aria-labelledby="movie-title">
            <picture>
                <source srcSet={getPosterSrc(movie.poster_path, "webp")} type="image/webp"/>
                <source srcSet={getPosterSrc(movie.poster_path, "jpg")} type="image/jpeg"/>
                <Poster src={getPosterSrc(movie.poster_path)} alt={movie.title || "Placeholder Poster"}/>
            </picture>
            <InfoContainer>
                <Title>
                    {movie.title}
                    {movie.vote_average > 0 &&
                        <Rating aria-label={`Rating: ${movieScore} out of 10`}>⭐ {movieScore}</Rating>}
                </Title>
                {movie.genres.length !== 0 && <Genres role="list" aria-label="Genres">
                    {movie.genres.map((genre) => (
                        <Genre role="listitem" key={genre.id}>{genre.name}</Genre>
                    ))}
                </Genres>}
                <Overview>{movie.overview}</Overview>
                {movie.runtime > 0 &&
                    <p><span role="img" aria-label="Runtime">⏳</span>
                        <strong>Runtime:</strong> {movie.runtime} min
                    </p>}
                {movie.production_countries.length !== 0 &&
                    <p><span role="img" aria-label="Country of origin">🌎</span>
                        <strong>Origin:</strong> {movie.production_countries[0].name}
                    </p>}
                {movie.release_date && <p>
                    <span role="img" aria-label="Release date">📅</span>
                    <strong>Release Date:
                    </strong> {formatDate(movie.release_date)}
                </p>}
            </InfoContainer>
        </Container>
    )
}

const Container = styled.div`
    padding: ${({theme}) => theme.spacing.lg};
    margin: auto;

    max-width: ${({theme}) => theme.layout.containerWidth};

    display: flex;
    flex-direction: column-reverse;
    gap: ${({theme}) => theme.spacing.lg};

    @media (min-width: ${({theme}) => theme.breakpoints.tablet}) {
        flex-direction: row;
    }
`

const Poster = styled.img`
    width: 300px;
    border-radius: ${({theme}) => theme.radii.lg};
`

const InfoContainer = styled.div`
    display: flex;
    flex-direction: column;
    gap: ${({theme}) => theme.spacing.xl};
`

const Title = styled.h1`
    font-size: ${({theme}) => theme.fontSizes.xl};
    font-weight: 700;
    color: ${({theme}) => theme.colors.text};
`

const Rating = styled.span`
    padding: ${({theme}) => theme.spacing.xs} ${({theme}) => theme.spacing.md};
    margin-left: ${({theme}) => theme.spacing.sm};

    display: inline-block;
    font-weight: 600;

    background: ${({theme}) => theme.colors.transparentYellow};
    border-radius: ${({theme}) => theme.radii.sm}
`

const Genres = styled.div`
    display: flex;
    gap: ${({theme}) => theme.spacing.sm};
`

const Genre = styled.span`
    padding: ${({theme}) => theme.spacing.xs} ${({theme}) => theme.spacing.md};
    background: ${({theme}) => theme.colors.transparentBackground};
    border-radius: ${({theme}) => theme.radii.md};
`

const Overview = styled.p`
    line-height: 1.6;
    color: ${({theme}) => theme.colors.textMuted};
    font-size: ${({theme}) => theme.fontSizes.base};
`
