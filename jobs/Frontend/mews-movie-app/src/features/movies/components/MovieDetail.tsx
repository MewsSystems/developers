import MovieRating from "./MovieRating";
import styled from "styled-components";
import Image from "next/image";

const ContentBox = styled.div<{ $image: string }>`
    display: flex;
    background-color: #fff;
    overflow: hidden;
    width: 100%;
    border-radius: 1rem;
    box-shadow: var(--box-shadow);
    position: relative;
    background-image: ${(props) => {
        return `linear-gradient(rgba(0,0,0,0.3), rgba(0,0,0,1)), url(${props.$image})`;
    }};
    background-size: cover;
    background-position: top center;

    @media (min-width: 640px) {
        background: white;
    }
`;

const Detail = styled.div`
    padding: 2rem;
    min-height: 450px;
    overflow: auto;

    @media (min-width: 640px) {
        height: 450px;
    }
`;

const Title = styled.h1`
    font-weight: bolder;
    font-size: 1.7rem;
    color: #fff;
    margin: 0;

    @media (min-width: 640px) {
        color: var(--gray-700);
    }
`;

const Overview = styled.p`
    color: var(--gray-200);
    font-size: 0.9rem;
    overflow: auto;
    margin: 0;

    @media (min-width: 640px) {
        color: #6b6b6b;
        font-size: 1rem;
    }
`;

const ReleaseDate = styled.p`
    color: var(--gray-200);

    @media (min-width: 640px) {
        color: var(--gray-700);
    }
`;

const Header = styled.div`
    display: flex;
    align-items: start;
    justify-content: space-between;
    margin-bottom: 1rem;
    gap: 1rem;
`;

const StyledImage = styled(Image)`
    background-color: var(--gray-400);
    display: none;

    @media (min-width: 640px) {
        display: initial;
    }
`;

type Props = {
    title?: string;
    posterPath?: string;
    releaseDate?: string;
    overview?: string;
    voteAverage?: number;
};

const MovieDetail = ({
    title = "No title",
    posterPath,
    releaseDate,
    overview,
    voteAverage,
}: Props) => {
    const imageUrl = posterPath
        ? `https://image.tmdb.org/t/p/w300/${posterPath}`
        : "";

    return (
        <ContentBox $image={imageUrl}>
            {posterPath && (
                <StyledImage
                    width={300}
                    height={450}
                    alt={`Movie poster for ${title}`}
                    src={imageUrl}
                />
            )}

            <Detail>
                <Header>
                    <Title>{title}</Title>
                    <MovieRating rating={voteAverage} />
                </Header>
                {releaseDate && (
                    <ReleaseDate>
                        <strong>Release date:</strong>{" "}
                        {new Date(releaseDate).getFullYear()}
                    </ReleaseDate>
                )}
                <Overview>
                    {overview || "This movie is still without any description."}
                </Overview>
            </Detail>
        </ContentBox>
    );
};

export default MovieDetail;
