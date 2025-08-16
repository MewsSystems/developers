import type { Genre } from "../../app/slices/movieDetails/interfaces/movie-details-response"
import type { SimpleMovieDetails } from "../../app/slices/movieDetails/interfaces/simple-movie-details"
import { StyledFlex } from "../flex/Flex.styled"
import { StyledMovieDetails } from "./MovieDetails.styled"
import { StyledMovieNumbers } from "./MovieNumbers.styled"

interface Props {
  movieDetails: SimpleMovieDetails
}

const getGenres = (genres: Genre[]) => {
  return genres?.map(genre => genre.name).join(" | ") ?? ""
}

const MovieDetails = ({ movieDetails }: Props) => {
  return (
    <StyledFlex>
      <StyledMovieDetails>
        <div>
          <img src={movieDetails.image} alt="Movie poster" />
        </div>
        <div>
          <h1>{movieDetails.title}</h1>
          <h3>{movieDetails.tagline}</h3>
          <StyledMovieNumbers>
            <div>
              <span>Language:</span>
              <span>{movieDetails.language}</span>
            </div>
            <div>
              <span>Length:</span>
              <span>{movieDetails.length} mins</span>
            </div>
            <div>
              <span>Rate:</span>
              <span>{movieDetails.rate}</span>
            </div>
            <div>
              <span>Budget:</span>
              <span>$ {movieDetails.budget}</span>
            </div>
            <div>
              <span>Release Date:</span>
              <span>{movieDetails.release_date}</span>
            </div>
          </StyledMovieNumbers>
          <div>
            <h2>Genres</h2>
            {getGenres(movieDetails.genres)}
          </div>
          <div>
            <h2>Overview</h2>
            <p>{movieDetails.overview}</p>
          </div>
        </div>
      </StyledMovieDetails>
    </StyledFlex>
  )
}
export default MovieDetails
