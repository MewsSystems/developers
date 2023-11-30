import Link from "next/link";
import { SearchMovie } from "../api/api";
import styled from "styled-components";

type Props = {
    searchMovie: SearchMovie;
};

const Result = styled.div`
    padding: 0.5rem;
`;

const SearchMovieResult = ({ searchMovie }: Props) => {
    const { id, title } = searchMovie;

    return (
        <Result>
            <Link href={`/detail/${id}`}>{title}</Link>
        </Result>
    );
};

export default SearchMovieResult;
