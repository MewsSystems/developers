import React from "react";
import styled from "styled-components";
import { useQueryClient } from "react-query";

import { MovieItem } from "./MovieItem";
import { Pagination } from "./Pagination";
import { Movie, MovieGridProps } from "../interfaces";
import { handleURL } from "../utils/handleUrl";
import { useFetchMovies } from "../hooks/useFetchMovies";

const MovieGridContainer = styled.div`
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
    grid-gap: 20px;
    padding: 20px;
`;

export const MovieGrid = ({ term, page, setPage }: MovieGridProps) => {
    const queryClient = useQueryClient();

    const handlePageChange = (newPage: number) => {
        setPage(newPage);
        queryClient.setQueryData("page", newPage);
    };

    const { data, isLoading, isError } = useFetchMovies(handleURL(term, page));

    // TODO: remove this before production
    console.log("MovieGrid =>  term: ", term, "data: ", data);

    return (
        <main>
            {!isLoading && data && data.total_pages && (
                <Pagination
                    page={page}
                    total_pages={data.total_pages}
                    onPreviousPage={() => handlePageChange(page - 1)}
                    onNextPage={() => handlePageChange(page + 1)}
                />
            )}
            {isLoading ? (
                <p>Loading movies ...</p>
            ) : isError ? (
                <p>Error: Unable to fetch data</p>
            ) : term.trim().length === 0 ? (
                <>
                    <h2>Showing Top Popular Movies</h2>
                    <MovieGridContainer>
                        {data &&
                            data.results.map((item: Movie) => (
                                <MovieItem key={item.id} movie={item} />
                            ))}
                    </MovieGridContainer>
                </>
            ) : data && data.results.length === 0 ? (
                <>
                    <h2>No results found for: {term}</h2>
                </>
            ) : (
                <>
                    <h2>Showing results for: {term}</h2>
                    <MovieGridContainer>
                        {data &&
                            data.results.map((item: Movie) => (
                                <MovieItem key={item.id} movie={item} />
                            ))}
                    </MovieGridContainer>
                </>
            )}
        </main>
    );
};
