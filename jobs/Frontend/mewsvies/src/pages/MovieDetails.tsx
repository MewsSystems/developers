import { useQueryClient } from "react-query";
import { useParams, Link, useNavigate } from "react-router-dom";
import styled from "styled-components";
import { MovieResult, Movie } from "../interfaces";
import { Header } from "../components/Header";
import { Footer } from "../components/Footer";

const IMAGE_PATH = process.env.REACT_APP_IMAGE_POSTER;

const StyledLink = styled(Link)`
    background-color: var(--btn-primary);
    border: 1px solid var(--btn-primary);
    border-radius: 15px;
    color: var(--white);
    font-family: "Axiforma-Regular", sans-serif;
    font-size: 0.875rem;
    padding: 0.5rem 2rem;
    transition: all 0.25s ease-in;

    &:hover {
        color: var(--btn-primary);
        background-color: var(--white);
        border-color: var(--btn-primary-hover);
        transition: all 0.25s ease-in;
    }
`;

const MovieTitle = styled.h2`
    color: var(--btn-primary);
    font-family: "Axiforma-Regular", sans-serif;
`;

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
            <main className="flex flex-col py-4 flex-1">
                <StyledLink to="#" onClick={handleGoBack} className="mt-4 max-w-4xl mx-auto">
                    Go back
                </StyledLink>

                <article className="bg-white p-6 max-w-4xl mx-auto">
                    <div className="flex flex-col sm:flex-row">
                        {image ? (
                            <img
                                src={`${IMAGE_PATH}/${image}`}
                                alt={title}
                                className="sm:mr-8 rounded-lg"
                            />
                        ) : null}
                        <div className="text-left">
                            <MovieTitle className="text-4xl my-6">{title}</MovieTitle>
                            <p className="text-lg mb-2">{overview}</p>
                            <p className="text-md">Release Date: {releaseDate}</p>
                            <p className="text-md">Original Language: {language}</p>
                            <p className="text-md">Vote Average: {vote}</p>
                        </div>
                    </div>
                </article>
            </main>
            <Footer />
        </>
    );
};
