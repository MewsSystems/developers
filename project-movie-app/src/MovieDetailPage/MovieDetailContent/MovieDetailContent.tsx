import { Button } from "../../Button/Button"
import { MovieDetailContentProps } from "./types"
import { Wrapper, Title, Tagline, PosterWrapper, PosterImage, MovieDetail, MovieDetailItem, MovieLink, InfoPrimary, MovieOverview, MovieOverviewHeader, MovieOverviewContent, ButtonLink } from "./style"

export const MovieDetailContent: React.FC<MovieDetailContentProps> = ({movie}) => {

        return (
        <Wrapper>
            <Title>{movie.title}</Title>
            <Tagline>
                <em>
                    {movie.tagline && `"${movie.tagline}"`}
                </em>
            </Tagline>
            <InfoPrimary>
                <PosterWrapper>
                    {movie.backdropPath && 
                        <PosterImage
                            src={movie.backdropPath}
                            alt={movie.title}
                        />
                    }
                </PosterWrapper>
                <MovieDetail>
                    {movie.genres &&
                        <MovieDetailItem><strong>Genres: </strong>{movie.genres}</MovieDetailItem>
                    }
                    {movie.userScore &&
                        <MovieDetailItem><strong>User Score: </strong>{movie.userScore}</MovieDetailItem>
                    }
                    {movie.releaseDate &&
                        <MovieDetailItem><strong>Release Date: </strong>{movie.releaseDate}</MovieDetailItem>
                    }
                    {movie.originalLanguage &&
                        <MovieDetailItem><strong>Original Language: </strong>{movie.originalLanguage}</MovieDetailItem>
                    }
                    {movie.runtime  &&
                        <MovieDetailItem><strong>Runtime: </strong>{movie.runtime}</MovieDetailItem>
                    }
                    {movie.homepage &&
                        <MovieDetailItem><strong>Homepage: </strong>
                            <MovieLink href={movie.homepage} target="_blank" rel="noopener noreferrer" aria-label={`Visit homepage of ${movie.title}`}>
                                {movie.title}
                            </MovieLink>
                        </MovieDetailItem>
                    }
                </MovieDetail>
            </InfoPrimary>
            {movie.overview && 
                <MovieOverview>
                    <MovieOverviewHeader>Movie overview</MovieOverviewHeader>
                    <MovieOverviewContent>{movie.overview}</MovieOverviewContent>
                </MovieOverview>
            }
            <ButtonLink to = "/">
                <Button label="Back to movies"/>
            </ButtonLink>
        </Wrapper>
    )
}