import { Link } from "react-router-dom";
import styled from "styled-components";
import { Movie } from "../interfaces";

const IMAGE_PATH = process.env.REACT_APP_IMAGE_COVER;

const MovieItemContainer = styled.article`
    transition:
        transform 0.3s ease-in,
        box-shadow 0.3s ease-in-out;

    &:hover {
        img {
            transform: scale(1.025);
            transition: transform 0.25s ease-out;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }
        h2 {
            color: var(--btn-primary);
        }
    }
`;

const MovieTitle = styled.h2`
    transition: color 0.2s ease-in-out;
`;

export const MovieItem = ({ movie }: { movie: Movie }) => {
    const { id, title, poster_path: image } = movie as Movie;

    return (
        <MovieItemContainer>
            <Link to={{ pathname: `/movie/${id}` }}>
                <div key={id} className="flex flex-col">
                    {image ? (
                        <img src={`${IMAGE_PATH}/${image}`} alt={title} className="rounded-lg" />
                    ) : (
                        <img
                            src="https://placehold.jp/200x300.png?text=Placeholder+Image"
                            alt={title}
                            className="rounded-lg"
                        />
                    )}

                    <MovieTitle className="text-2xl text-gray-700 my-2 font-semibold">
                        {title}
                    </MovieTitle>
                </div>
            </Link>
        </MovieItemContainer>
    );
};
