import { Link, useParams } from 'react-router-dom'
import { useGetMovieDetails } from '../hooks/movies/useGetMovieDetails'
import { styled } from 'styled-components'
import movieClient from '../api/movieClient/movieClient'
import { ArrowLeftCircleFill } from '../components/ui/icon/Icon'

function MovieDetails() {
  const { id } = useParams() as { id: string }
  const {
    isLoading,
    isError,
    error,
    data: movieDetails,
  } = useGetMovieDetails(id)

  if (isLoading) return <div>Loading...</div>
  if (isError) return <div>Error: {error.message}</div>

  if (movieDetails)
    return (
      <MovieDetailsContainer>
        <div className="flex-container">
          <div className="poster">
            <a href="/">
              <ArrowLeftCircleFill width={42} height={42} fill="#D50C2F" />
            </a>
            <img
              src={movieClient.buildMoviePosterUrl(movieDetails.poster_path)}
              alt="Movie poster"
            />
          </div>
          <div className="details">
            <h1>{movieDetails.title}</h1>
            <p>
              <b>Overview:</b> {movieDetails.overview}
            </p>
            <br />
            <p>
              <b>Status:</b> {movieDetails.status}
            </p>
            <p>
              <b>Released:</b> {movieDetails.release_date}
            </p>
            <p>
              <b>Genres:</b> {movieDetails.genres.map((g) => g.name).join(', ')}
            </p>
            <p>
              <b>Budget:</b> {movieDetails.budget}$
            </p>
            <p>
              <b>Votes:</b> {movieDetails.vote_count}
            </p>
            <p>
              <b>Vote Avg:</b> {movieDetails.vote_average}
            </p>
            <p>
              <b>Popularity:</b> {movieDetails.popularity}
            </p>
          </div>
        </div>
      </MovieDetailsContainer>
    )
}

const MovieDetailsContainer = styled.main`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  margin: 1rem;

  .flex-container {
    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    justify-content: center;
    gap: 1rem;

    .poster {
      display: flex;
      flex: 0 0 auto;
      position: relative;

      a {
        position: absolute;
        top: 0;
      }

      img {
        height: 100%;
        max-height: 500px;
      }
    }

    .details {
      flex: 0 1 400px;
    }
  }
`

export default MovieDetails
