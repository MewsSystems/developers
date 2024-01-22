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
    <StyledCard onClick={() => console.log(id)}>
      <StyledImage src={image}></StyledImage>
      <StyledMovieTitle>
        <h3>{title}</h3>
      </StyledMovieTitle>
    </StyledCard>
  )
}
export default MovieCard
