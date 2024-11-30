import { Movie } from '../../api/movieClient/types'
import Button from '../ui/button/Button'
import movieClient from '../../api/movieClient/movieClient'
import Card from '../ui/card/Card'
import { Link } from 'react-router-dom'
import { styled } from 'styled-components'

type MovieCardProps = {
  movie: Movie
}

function MovieCard({ movie }: MovieCardProps) {
  const posterUrl = movieClient.buildMoviePosterUrl(movie.poster_path)

  return (
    <Card key={movie.id}>
      <MoviePoster
        style={{
          backgroundImage: `linear-gradient(rgba(255,255,255,.6), rgba(255,255,255,.5)), url("${posterUrl}")`,
        }}
      >
        <div className="details">
          <h2>{movie.title}</h2>
          <p>{movie.overview}</p>
        </div>
        <div className="actions">
          <Button>
            <Link to={`/movie/${movie.id}`}>More Details</Link>
          </Button>
        </div>
      </MoviePoster>
    </Card>
  )
}

const MoviePoster = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  flex: 1;
  border-radius: 0.8rem;
  background-size: cover;
  padding: 0.8rem;
  height: inherit;

  .details {
    color: var(--primary-brand-color-400);
    flex-grow: 1;

    p {
      display: -webkit-box;
      -webkit-line-clamp: 3;
      -webkit-box-orient: vertical;
      overflow: hidden;
    }
  }

  .actions {
    display: flex;
    flex-shrink: 0;
    flex-grow: 0;
    button {
      flex: 1;
    }
  }
`

export default MovieCard
