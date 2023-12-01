"use client";

import BackButton from "@/components/ui/BackButton";
import Message from "@/components/ui/Message";
import Spinner from "@/components/ui/Spinner";
import { useGetMovieDetailQuery } from "@/features/movies/api/api";
import MovieDetail from "@/features/movies/components/MovieDetail";
import { useRouter } from "next/navigation";
import styled from "styled-components";

const Container = styled.div`
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 1rem;
    padding: 2rem 1rem;
    width: 100%;
    max-width: 55rem;
    margin: 0 auto;

    @media (min-width: 768px) {
        padding: 5rem 1rem;
    }
`;

const Controls = styled.div`
    width: 100%;
    display: flex;
    justify-content: flex-start;
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
                    <Controls>
                        <BackButton onClick={() => router.back()}>
                            Back to search
                        </BackButton>
                    </Controls>

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
