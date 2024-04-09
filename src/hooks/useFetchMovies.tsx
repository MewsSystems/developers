import { useMemo } from "react";
import { useQuery, useQueryClient } from "react-query";
import { MovieResult } from "../interfaces";

export const useFetchMovies = (url: string) => {
    const memoizedUrl = useMemo(() => url, [url]);
    const queryClient = useQueryClient();

    const fetchData = async () => {
        const controller = new AbortController();
        const signal = controller.signal;

        try {
            const request = await fetch(memoizedUrl, { signal });

            if (!request.ok) {
                throw new Error("Network error: Code " + request.status);
            }
            const data = await request.json();
            return data;
        } finally {
            controller.abort();
        }
    };

    return useQuery<MovieResult>(["movieList", memoizedUrl], fetchData, {
        onSuccess: (data) => {
            queryClient.setQueryData(["movieList"], data);
        },
        onError: (error) => {
            console.error("Error fetching movies:", error);
        },
    });
};
