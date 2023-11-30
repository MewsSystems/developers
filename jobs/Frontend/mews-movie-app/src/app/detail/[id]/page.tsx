"use client";

import { useGetMovieDetailQuery } from "@/features/movies/api/api";
import { useRouter } from "next/navigation";

export default function MovieDetail({ params }: { params: { id: number } }) {
    const router = useRouter();
    const { data, isLoading, error } = useGetMovieDetailQuery(params.id);

    if (isLoading) {
        return <div>Loading...</div>;
    }

    if (error || data === undefined) {
        return <div>Error</div>;
    }

    return (
        <div>
            <button onClick={() => router.back()}>Go back to search</button>
            {data.title}
        </div>
    );
}
