import { useQueryClient } from "react-query";
import { useParams, Link, useNavigate } from "react-router-dom";
import { MovieResult, Movie } from "../interfaces";
import { Header } from "../components/Header";
import { Footer } from "../components/Footer";

const IMAGE_PATH = process.env.REACT_APP_IMAGE_POSTER;

export const MovieDetails = () => {
    const { id } = useParams<{ id: string }>();
    const queryClient = useQueryClient();
    const currentMovies = queryClient.getQueryData<MovieResult>("movieList");
    const navigate = useNavigate();

    // ! TODO: remove this before production
    console.log("MovieDetails =>  currentMovies: ", currentMovies);

    if (!currentMovies) return <p>Doing another request - Loading movie details...</p>;

    const movie = currentMovies.results?.find((movie: Movie) => movie.id.toString() === id);
    if (!movie) return <p>Movie not found...</p>;

    const {
        title,
        overview,
        release_date: releaseDate,
        original_language: language,
        poster_path: image,
        vote_average: vote,
    } = movie;

    const handleGoBack = () => {
        navigate(-1);
    };

    return (
        <>
            <Header />
            <main>
                <Link to="#" onClick={handleGoBack}>
                    Go back
                </Link>
                <h1>{title}</h1>
                <p>{overview}</p>
                <p>Release Date: {releaseDate}</p>
                <p>Original Language: {language}</p>
                <p>Vote Average: {vote}</p>
                <img src={`${IMAGE_PATH}/${image}.jpg`} alt={title} />
            </main>
            <Footer />
        </>
    );
};
