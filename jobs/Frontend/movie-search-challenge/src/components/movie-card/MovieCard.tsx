import { Link } from "react-router-dom"
import { StyledCard } from "./Card.styled"
import { StyledImage } from "./Image.styled"
import { StyledMovieTitle } from "./MovieTitle.styled"

interface Props {
  id: number
  image: string
  title: string
}

const MovieCard = ({ id, image, title }: Props) => {
  return (
    <Link to={`/${id}`}>
      <StyledCard>
        <StyledImage src={image}></StyledImage>
        <StyledMovieTitle>
          <h3>{title}</h3>
        </StyledMovieTitle>
      </StyledCard>
    </Link>
  )
}
export default MovieCard
