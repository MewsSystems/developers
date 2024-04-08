import styled from "styled-components";
import { useQueryClient } from "react-query";

import { MovieItem } from "./MovieItem";
import { Pagination } from "./Pagination";
import { Movie, MovieGridProps } from "../interfaces";
import { handleURL } from "../utils/handleUrl";
import { useFetchMovies } from "../hooks/useFetchMovies";
import { Loading } from "./Loading";

const MainContainer = styled.main`
    display: flex;
    flex-direction: column;
    flex-grow: 1;
    width: 100%;
`;
const MovieGridContainer = styled.section`
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
    grid-gap: 20px;
    padding: 20px;
    width: 100%;
`;

const MovieGridTitle = styled.h2`
    font-family: "Axiforma-Light", sans-serif;

    span {
        font-family: "Axiforma-Bold", sans-serif;
        text-transform: capitalize;
    }
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
        <MainContainer className="mx-auto max-w-7xl px-4 sm:px-6 lg:px-8 min-h-80vh items-center">
            {isLoading && <Loading />}
            {isError && <p>Error: Unable to fetch data</p>}
            {data && (
                <>
                    {term.trim().length === 0 ? (
                        <MovieGridTitle className="text-2xl font-bold my-8">
                            Showing Top Popular Movies
                        </MovieGridTitle>
                    ) : (
                        <MovieGridTitle className="text-2xl font-bold my-8">
                            Showing results for: <span className="text-gray-700">{term}</span>
                        </MovieGridTitle>
                    )}
                    {data.results.length === 0 ? (
                        <MovieGridTitle className="text-1xl font-bold my-8">
                            No results found for:
                            <span className="text-gray-700">{term}</span>
                        </MovieGridTitle>
                    ) : (
                        <>
                            <Pagination
                                page={page}
                                total_pages={data.total_pages}
                                onPreviousPage={() => handlePageChange(page - 1)}
                                onNextPage={() => handlePageChange(page + 1)}
                            />
                            <MovieGridContainer className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 xl:grid-cols-5 gap-6">
                                {data.results.map((item: Movie) => (
                                    <MovieItem key={item.id} movie={item} />
                                ))}
                            </MovieGridContainer>
                            <Pagination
                                page={page}
                                total_pages={data.total_pages}
                                onPreviousPage={() => handlePageChange(page - 1)}
                                onNextPage={() => handlePageChange(page + 1)}
                            />
                        </>
                    )}
                </>
            )}
        </MainContainer>
    );
};
