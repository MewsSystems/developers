"use client";

import Paging from "@/components/Paging";
import SearchInput from "@/components/SearchInput";
import { useGetMoviesQuery } from "@/features/movies/api/api";
import SearchMovieResult from "@/features/movies/components/SearchMovieResult";
import { useSearchParamsReplace } from "@/hooks/useSearchParamReplace";
import { useSearchParams } from "next/navigation";
import { useEffect, useState } from "react";
import styled from "styled-components";

const Container = styled.div`
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 2rem;
    padding: 1rem;
    padding-top: 5rem;
    max-width: 45rem;
    margin: 0 auto;
`;

const ContentBox = styled.div`
    background-color: #fff;
    padding: 2rem;
    width: 100%;
    border-radius: 1rem;
    box-shadow: var(--box-shadow);
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

    if (isLoading) {
        return <div>Loading...</div>;
    }

    if (error || data === undefined) {
        return <div>Error</div>;
    }

    const { results = [], total_pages: totalPages = 1, page = 1 } = data;

    return (
        <Container>
            <SearchInput
                placeholder="Search for movies"
                initialValue={searchQuery}
                onChange={(value) => setSearchQuery(value)}
                aria-label="Search for movies"
            />
            {results.length > 0 && (
                <>
                    <Paging current={page} total={totalPages} />
                    <ContentBox>
                        {results.map((result) => (
                            <SearchMovieResult
                                key={result.id}
                                searchMovie={result}
                            />
                        ))}
                    </ContentBox>
                    <Paging current={page} total={totalPages} />
                </>
            )}
        </Container>
    );
}
