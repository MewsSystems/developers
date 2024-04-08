import { useQueryClient } from "react-query";
import { useParams, Link, useNavigate } from "react-router-dom";
import styled from "styled-components";
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

    const StyledLink = styled(Link)`
        color: #3182ce;
        text-decoration: none;
        padding: 0.5rem 1rem;
        border: 1px solid #3182ce;
        border-radius: 0.25rem;
        transition: all 0.3s;

        &:hover {
            background-color: #3182ce;
            color: white;
        }
    `;

    return (
        <>
            <Header />
            <main className="flex flex-col py-4">
                <StyledLink to="#" onClick={handleGoBack} className="mt-4 max-w-4xl mx-auto">
                    Go back
                </StyledLink>

                <article className="bg-white p-6 max-w-4xl mx-auto">
                    <div className="flex flex-col sm:flex-row">
                        <img
                            src={`${IMAGE_PATH}/${image}.jpg`}
                            alt={title}
                            className="mr-8 rounded-lg"
                        />
                        <div className="text-left">
                            <h2 className="text-4xl text-gray-700 my-6 leading-8 font-semibold">
                                {title}
                            </h2>
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
