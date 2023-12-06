import { useParams } from "react-router-dom"
import styled from "styled-components"
import { useGetMovieQuery } from "@/features/api/apiSlice"
import ErrorCard from "@/components/ErrorCard"
import { Badge, BadgeContainer } from "@/components/badges"

const OverviewContainer = styled.div`
  margin: 1rem 0;
`

const MovieTitle = styled.h2`
  margin-bottom: 1rem;
`

const Poster = styled.img`
  height: 350px;
  margin-left: 1rem;
`

const MovieDetailContainer = styled.div`
  display: flex;
  flex-direction: row;
`

function MovieDetail({ movie }) {
  const imageUrl = `https://image.tmdb.org/t/p/w440_and_h660_face/${movie.poster_path}`
  console.log(movie)
  return (
    <MovieDetailContainer>
      <div>
        <MovieTitle>{movie.title}</MovieTitle>
        <BadgeContainer>
          {movie.genres.map((genre) => (
            <Badge key={genre.id}>{genre.name}</Badge>
          ))}
        </BadgeContainer>
        <OverviewContainer>
          <p>{movie.overview}</p>
        </OverviewContainer>
        <p>Popularity: {movie.popularity}</p>
        <p>Release date: {movie.release_date}</p>
        <p>Status: {movie.status}</p>
      </div>
      <div>
        <Poster src={imageUrl} alt={`${movie.title} poster`} />
      </div>
    </MovieDetailContainer>
  )
}

function MovieDetailPage() {
  const { movieId } = useParams()
  const {
    data: movie,
    isLoading,
    isSuccess,
    isError,
    error,
  } = useGetMovieQuery({ id: movieId })

  return (
    <div>
      {isLoading && <p>Loading...</p>}
      {isSuccess && movie && <MovieDetail movie={movie} />}
      {isSuccess && !movie && <p>There are no results</p>}
      {isError && <ErrorCard error={error} />}
    </div>
  )
}

export default MovieDetailPage
