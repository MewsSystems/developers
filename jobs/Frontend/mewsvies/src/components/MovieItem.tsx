import { Link } from "react-router-dom";
import { Movie } from "../interfaces";

const IMAGE_PATH = process.env.REACT_APP_IMAGE_COVER;

export const MovieItem = ({ movie }: { movie: Movie }) => {
    const {
        id,
        title,
        overview,
        release_date: releaseDate,
        original_language: language,
        poster_path: image,
        vote_average: vote,
    } = movie as Movie;

    return (
        <>
            <Link to={{ pathname: `/movie/${id}` }}>
                <div key={id}>
                    <h2>{title}</h2>
                    <p>{overview}</p>
                    <p>
                        <b>Release date: </b>
                        {releaseDate}
                    </p>
                    <p>
                        <b>Original Language: </b>
                        {language}
                    </p>
                    <p>
                        <b>Rate: </b>
                        {vote}
                    </p>
                    <img src={`${IMAGE_PATH}/${image}.jpg`} alt={title} />
                </div>
            </Link>
        </>
    );
};
