import styled from "styled-components"
import { Link } from "./typography"

const CardContainer = styled.article`
  padding-top: 1em;
  background-color: ${(props) => props.theme.white};
  display: flex;
  align-items: center;
`

const Poster = styled.img`
  height: 70px;
  margin-right: 1em;
`

function MovieCard({ movie }) {
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
