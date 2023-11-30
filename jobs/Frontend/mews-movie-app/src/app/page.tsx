"use client";

import Paging from "@/components/Paging";
import SearchInput from "@/components/SearchInput";
import { useGetMoviesQuery } from "@/features/movies/api/api";
import SearchMovieResult from "@/features/movies/components/SearchMovieResult";
import { useSearchParamsReplace } from "@/hooks/useSearchParamReplace";
import { useSearchParams } from "next/navigation";
import { useEffect, useState } from "react";

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
        <main>
            <div>
                <SearchInput
                    placeholder="Search movies"
                    initialValue={searchQuery}
                    onChange={(value) => setSearchQuery(value)}
                    aria-label="Search movies"
                />
            </div>
            {results.length > 0 && (
                <div>
                    <Paging current={page} total={totalPages} />
                    {results.map((result) => (
                        <SearchMovieResult
                            key={result.id}
                            searchMovie={result}
                        />
                    ))}
                    <Paging current={page} total={totalPages} />
                </div>
            )}
        </main>
    );
}
