import Link from "next/link";
import { SearchMovie } from "../api/api";
import styled from "styled-components";
import Image from "next/image";
import MovieRating from "./MovieRating";

const StyledLink = styled(Link)`
    padding: 1rem;
    display: flex;
    gap: 1rem;
    border-radius: 0.5rem;
    text-decoration: none;
    color: #3d3d3d;

    &:hover {
        background-color: var(--violet-50);
        color: var(--violet-600);
    }
`;

const Overview = styled.p`
    color: #999;
    font-size: 0.8rem;
    overflow: hidden;
    display: -webkit-box;
    -webkit-box-orient: vertical;
    -webkit-line-clamp: 2;
    margin-bottom: 0;
`;

const ImageWrapper = styled.div`
    border-radius: 0.5rem;
    width: 100px;
    height: 66px;
    background-color: #eee;
    flex-shrink: 0;
    overflow: hidden;
`;

const Content = styled.div`
    flex: 1;
`;

const Title = styled.h3`
    font-weight: 500;
    font-size: 1.2rem;
    margin: 0;
    margin-bottom: 0.5rem;
`;

type Props = {
    searchMovie: SearchMovie;
};

const SearchMovieResult = ({ searchMovie }: Props) => {
    const { id, title, vote_average, overview, backdrop_path } = searchMovie;

    return (
        <StyledLink href={`/detail/${id}`}>
            <ImageWrapper>
                {backdrop_path && (
                    <Image
                        alt={`Movie poster for ${title}`}
                        width={100}
                        height={66}
                        src={`https://image.tmdb.org/t/p/w200/${backdrop_path}`}
                    />
                )}
            </ImageWrapper>

            <Content>
                <Title>{title}</Title>
                <Overview>{overview}</Overview>
            </Content>
            <MovieRating rating={vote_average} />
        </StyledLink>
    );
};

export default SearchMovieResult;
