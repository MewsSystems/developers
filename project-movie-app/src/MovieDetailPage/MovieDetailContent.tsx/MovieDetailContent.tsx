import { Button } from "../../Button/Button"
import { Link } from "react-router"
import { MovieDetailContentProps } from "./types"
import "./movieDetail.style.css"

export const MovieDetailContent: React.FC<MovieDetailContentProps> = ({movie}) => {

        return (
        <section className="movie-detail">
            <h1 className="movie-title">{movie.title}</h1>
            <p className="movie-tagline">
                <em>
                    {movie.tagline && `"${movie.tagline}"`}
                </em>
            </p>
            <div className="movie-content">
                <div className="movie-poster">
                    {movie.backdropPath && 
                        <img
                            src={movie.backdropPath}
                            alt={movie.title}
                        />
                    }
                </div>
                <div className="movie-info">
                    {movie.genres &&
                        <p><strong>Genres: </strong>{movie.genres}</p>
                    }
                    {movie.userScore &&
                        <p><strong>User Score: </strong>{movie.userScore}</p>
                    }
                    {movie.releaseDate &&
                        <p><strong>Release Date: </strong>{movie.releaseDate}</p>
                    }
                    {movie.originalLanguage &&
                        <p><strong>Original Language: </strong>{movie.originalLanguage}</p>
                    }
                    {movie.runtime  &&
                        <p><strong>Runtime: </strong>{movie.runtime}</p>
                    }
                    {movie.homepage &&
                        <p><strong>Homepage: </strong>
                            <a href={movie.homepage} target="_blank" rel="noopener noreferrer">
                                {movie.title}
                            </a>
                        </p>
                    }
                </div>
            </div>
            {movie.overview && 
                <div className="movie-overview">
                    <h2>Movie overview</h2>
                    <p>{movie.overview}</p>
                </div>
            }
            <Link to = "/">
                <Button label="Back to movies"/>
            </Link>
        </section>
    )
}