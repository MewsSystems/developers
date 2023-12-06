import styled from "styled-components"
import { Link } from "./typography"

export interface MovieInterface {
  id: number
  title: string
  poster_path: string
}

const CardContainer = styled.article`
  margin-bottom: 1em;
  background-color: ${(props) => props.theme.white};
  display: flex;
  align-items: center;
  &:last-of-type {
    margin-bottom: 0;
  }
}
`

const Poster = styled.img`
  height: 70px;
  margin-right: 1em;
`

const PlaceholderPosterContainer = styled.div`
  height: 70px;
  width: 46.66px;
  display: flex;
  align-items: center;
  justify-content: center;
  border: 1px solid ${(props) => props.theme.light_gray};
  color: ${(props) => props.theme.light_gray};
  margin-right: 1em;
`

const PlaceholderPoster = () => {
  return <PlaceholderPosterContainer>?</PlaceholderPosterContainer>
}

function MovieCard({ movie }: { movie: MovieInterface }) {
  const imageUrl = `https://image.tmdb.org/t/p/w440_and_h660_face/${movie.poster_path}`
  const detailUrl = `/movies/${movie.id}`
  return (
    <CardContainer>
      {movie.poster_path ? (
        <Poster src={imageUrl} alt={`${movie.title} poster`} />
      ) : (
        <PlaceholderPoster />
      )}
      <Link to={detailUrl}>{movie.title}</Link>
    </CardContainer>
  )
}

export default MovieCard
