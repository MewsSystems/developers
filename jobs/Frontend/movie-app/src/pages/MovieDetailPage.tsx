import { useParams } from "react-router-dom"
import styled from "styled-components"
import { useGetMovieQuery } from "@/features/api/apiSlice"
import ErrorCard from "@/components/ErrorCard"
import { Badge, BadgeContainer } from "@/components/badges"
import Poster from "@/components/Poster"

const OverviewContainer = styled.div`
  margin: 1rem 0;
`

const MovieDetailContainer = styled.div`
  & h2,
  h3 {
    margin-bottom: 1rem;
  }
  display: flex;
  flex-direction: row;
  line-height: 1.5;
  justify-content: space-between;
`

const PosterContainer = styled.div`
  margin-left: 1rem;
`

function ExtraInformation({ movie }) {
  return (
    <div>
      <h3>Information</h3>
      <ul>
        <li>
          <b>Homepage:</b>{" "}
          <a href={movie.homepage} target="_blank">
            External link
          </a>
        </li>
        <li>
          <b>Score:</b> {movie.vote_average || "Unknown"} out of 10
          {movie.vote_count > 0 && (
            <span> (from {movie.vote_count} votes)</span>
          )}
        </li>
        <li>
          <b>Popularity:</b> {movie.popularity}
        </li>
        <li>
          <b>Release date:</b> {movie.release_date || "Unknown"}
        </li>
        <li>
          <b>Status:</b> {movie.status}
        </li>
        <li>
          <b>Collection:</b>{" "}
          {movie.belongs_to_collection?.name ||
            "Doesn't belong to a collection"}
        </li>
        <li>
          <b>Budget:</b> {movie.budget || "Unknown"}
        </li>
        <li>
          <b>Revenue:</b> {movie.revenue || "Unknown"}
        </li>
        <li>
          <b>Production companies:</b>{" "}
          {movie.production_companies.length > 0
            ? movie.production_companies.map((company, i) => (
                <span key={company.id}>
                  {i > 0 && ", "}
                  {company.name}
                </span>
              ))
            : "Unknown"}
        </li>
        <li>
          <b>Production countries:</b>{" "}
          {movie.production_countries > 0
            ? movie.production_countries.map((country, i) => (
                <span key={country.iso_3166_1}>
                  {i > 0 && ", "}
                  {country.name}
                </span>
              ))
            : "Unknown"}
        </li>
      </ul>
    </div>
  )
}

function MovieDetail({ movie }) {
  return (
    <MovieDetailContainer>
      <div>
        <h2>{movie.title}</h2>
        {movie.genres.length > 0 && (
          <BadgeContainer>
            {movie.genres.map((genre) => (
              <Badge key={genre.id}>{genre.name}</Badge>
            ))}
          </BadgeContainer>
        )}
        <OverviewContainer>
          <h3>Overview</h3>
          <p>{movie.overview || "There isn't an available overview."}</p>
        </OverviewContainer>
        <ExtraInformation movie={movie} />
      </div>
      <PosterContainer>
        <Poster
          url={movie.poster_path}
          title={movie.title}
          height="450px"
          width="300px"
        />
      </PosterContainer>
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
