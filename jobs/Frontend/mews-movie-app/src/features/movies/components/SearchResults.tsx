import Paging from "@/components/ui/Paging";
import { SearchMovie } from "../api/api";
import MovieResult from "./MovieResult";
import styled from "styled-components";
import Message from "@/components/ui/Message";

const ContentBox = styled.div`
    background-color: #fff;
    padding: rem;
    width: 100%;
    border-radius: 1rem;
    box-shadow: var(--box-shadow);
    overflow: hidden;

    @media (min-width: 768px) {
        padding: 2rem;
    }
`;

type Props = {
    searchQuery?: string;
    page: number;
    totalPages: number;
    results: SearchMovie[];
};

const SearchResults = ({
    searchQuery = "",
    page,
    totalPages,
    results,
}: Props) => {
    if (results.length === 0 && searchQuery.length > 0) {
        return (
            <Message title="No movies found">
                Try searching for something else.
            </Message>
        );
    }

    if (results.length === 0 && searchQuery.length === 0) {
        return (
            <Message title="Let's find some movies!">
                Use the search bar above to find movies by title.
            </Message>
        );
    }

    return (
        <>
            <Paging current={page} total={totalPages} />
            <ContentBox>
                {results.map((result) => (
                    <MovieResult key={result.id} searchMovie={result} />
                ))}
            </ContentBox>

            <Paging current={page} total={totalPages} />
        </>
    );
};

export default SearchResults;
