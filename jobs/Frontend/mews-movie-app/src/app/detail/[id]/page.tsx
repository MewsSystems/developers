"use client";

import Message from "@/components/Message";
import Spinner from "@/components/Spinner";
import { useGetMovieDetailQuery } from "@/features/movies/api/api";
import MovieDetail from "@/features/movies/components/MovieDetail";
import MovieRating from "@/features/movies/components/MovieRating";
import Image from "next/image";
import { useRouter } from "next/navigation";
import styled from "styled-components";

const Container = styled.div`
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 2rem;
    padding: 5rem 1rem;
    width: 100%;
    max-width: 55rem;
    margin: 0 auto;
`;

const BackButton = styled.button`
    border: none;
    background: none;
    color: var(--violet-600);
    font-size: 1rem;
    font-weight: 500;
    cursor: pointer;

    &:hover {
        text-decoration: underline;
    }

    &:focus-visible {
        outline: 3px solid var(--focus-color);
    }
`;

export default function Detail({ params }: { params: { id: number } }) {
    const router = useRouter();
    let { data, isLoading, error } = useGetMovieDetailQuery(params.id);

    return (
        <Container>
            {isLoading && <Spinner />}

            {error && data === undefined && (
                <Message title="Oh no! Something went wrong.">
                    Please try again later.
                </Message>
            )}

            {data && (
                <>
                    <div>
                        <BackButton onClick={() => router.back()}>
                            Go back to search
                        </BackButton>
                    </div>

                    <MovieDetail
                        title={data.title}
                        posterPath={data.poster_path}
                        overview={data.overview}
                        releaseDate={data.release_date}
                        voteAverage={data.vote_average}
                    />
                </>
            )}
        </Container>
    );
}
