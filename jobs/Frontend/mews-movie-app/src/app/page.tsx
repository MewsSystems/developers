"use client";

import Message from "@/components/Message";
import SearchInput from "@/components/SearchInput";
import Spinner from "@/components/Spinner";
import { useGetMoviesQuery } from "@/features/movies/api/api";
import SearchResults from "@/features/movies/components/SearchResults";
import { useSearchParamsReplace } from "@/hooks/useSearchParamReplace";
import { useSearchParams } from "next/navigation";
import { useEffect, useState } from "react";
import styled from "styled-components";

const Container = styled.div`
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 2rem;
    padding: 2rem 1rem;
    max-width: 45rem;
    margin: 0 auto;

    @media (min-width: 768px) {
        padding: 5rem 1rem;
    }
`;

const StyledSpinner = styled(Spinner)`
    margin: 3rem auto;
`;

export default function Home() {
    const urlParams = useSearchParams();

    const searchUrlParam = urlParams.get("search") || "";
    const pageUrlParam = urlParams.get("page") || 1;

    const { replace } = useSearchParamsReplace();

    const [searchQuery, setSearchQuery] = useState<string>(searchUrlParam);

    const { data, error, isLoading } = useGetMoviesQuery({
        query: searchQuery,
        page: Number(pageUrlParam),
    });

    useEffect(() => {
        const params: Record<string, string> = { search: searchQuery };

        if (searchQuery !== searchUrlParam) {
            params["page"] = "";
        }

        replace(params);
    }, [searchQuery, replace, searchUrlParam]);

    return (
        <Container>
            <SearchInput
                placeholder="Search for movies"
                initialValue={searchQuery}
                onChange={(value) => setSearchQuery(value)}
                aria-label="Search for movies"
            />

            {isLoading && <StyledSpinner />}

            {error && data === undefined && (
                <Message title="Oh no! Something went wrong.">
                    Please try again later.
                </Message>
            )}

            {data && data.results && (
                <SearchResults
                    searchQuery={searchQuery}
                    page={data.page || 1}
                    totalPages={data.total_pages || 1}
                    results={data.results || []}
                />
            )}
        </Container>
    );
}
