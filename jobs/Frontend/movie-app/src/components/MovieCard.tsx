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

function MovieCard({ movie }: { movie: MovieInterface }) {
  const imageUrl = `https://image.tmdb.org/t/p/w440_and_h660_face/${movie.poster_path}`
  const detailUrl = `/movies/${movie.id}`
  return (
    <CardContainer>
      <Poster src={imageUrl} alt={`${movie.title} poster`} />
      <Link to={detailUrl}>{movie.title}</Link>
    </CardContainer>
  )
}

export default MovieCard
