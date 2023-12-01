import MovieRating from "./MovieRating";
import styled from "styled-components";
import Image from "next/image";

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
    return (
        <ContentBox>
            {posterPath && (
                <StyledImage
                    width={300}
                    height={450}
                    alt={`Movie poster for ${title}`}
                    src={`https://image.tmdb.org/t/p/w300/${posterPath}`}
                />
            )}

            <Detail>
                <Header>
                    <Title>{title}</Title>
                    <MovieRating rating={voteAverage} />
                </Header>
                {releaseDate && (
                    <p>
                        <strong>Release date:</strong>{" "}
                        {new Date(releaseDate).getFullYear()}
                    </p>
                )}
                <Overview>
                    {overview || "This movie is still without any description."}
                </Overview>
            </Detail>
        </ContentBox>
    );
};

export default MovieDetail;
