"use client";

import { useGetMovieDetailQuery } from "@/features/movies/api/api";
import MovieRating from "@/features/movies/components/MovieRating";
import Image from "next/image";
import { useRouter } from "next/navigation";
import styled from "styled-components";

const Container = styled.div`
    display: flex;
    flex-direction: column;
    align-items: start;
    gap: 2rem;
    padding: 1rem;
    padding-top: 5rem;
    max-width: 55rem;
    margin: 0 auto;
`;

const ContentBox = styled.div`
    display: flex;
    background-color: #fff;
    overflow: hidden;
    width: 100%;
    border-radius: 1rem;
    box-shadow: var(--box-shadow);
`;

const Detail = styled.div`
    padding: 2rem;
    max-height: 450px;
`;

const Title = styled.h1`
    font-weight: bolder;
    font-size: 1.7rem;
    color: #3d3d3d;
    margin: 0;
`;

const Overview = styled.p`
    color: #6b6b6b;
    font-size: 1rem;
    overflow: auto;
    margin: 0;
`;

const Header = styled.div`
    display: flex;
    align-items: start;
    justify-content: space-between;
    margin-bottom: 1rem;
    gap: 1rem;
`;

const StyledImage = styled(Image)`
    background-color: #ccc;
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

export default function MovieDetail({ params }: { params: { id: number } }) {
    const router = useRouter();
    const { data, isLoading, error } = useGetMovieDetailQuery(params.id);

    if (isLoading) {
        return <div>Loading...</div>;
    }

    if (error || data === undefined) {
        return <div>Error</div>;
    }

    const { title, poster_path, overview, vote_average, release_date } = data;

    return (
        <Container>
            <div>
                <BackButton onClick={() => router.back()}>
                    Go back to search
                </BackButton>
            </div>
            <ContentBox>
                {poster_path && (
                    <StyledImage
                        width={300}
                        height={450}
                        alt={`Movie poster for ${title}`}
                        src={`https://image.tmdb.org/t/p/w300/${poster_path}`}
                    />
                )}

                <Detail>
                    <Header>
                        <Title>{title}</Title>
                        <MovieRating rating={vote_average} />
                    </Header>
                    {release_date && (
                        <p>
                            <strong>Release date:</strong>{" "}
                            {new Date(release_date).getFullYear()}
                        </p>
                    )}
                    <Overview>
                        {overview ||
                            "This movie is still without any description."}
                    </Overview>
                </Detail>
            </ContentBox>
        </Container>
    );
}
