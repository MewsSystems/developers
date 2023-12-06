import styled from "styled-components"
import { Link } from "./typography"
import Poster from "./Poster"

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

const PosterContainer = styled.div`
  margin-right: 1em;
`

function MovieCard({ movie }: { movie: MovieInterface }) {
  const detailUrl = `/movies/${movie.id}`
  return (
    <CardContainer>
      <PosterContainer>
        <Poster url={movie.poster_path} title={movie.title} />
      </PosterContainer>
      <Link to={detailUrl}>{movie.title}</Link>
    </CardContainer>
  )
}

export default MovieCard
