'use client';

import MovieDetail from "@/components/MovieDetail";
import { useGetMovieDetails } from "@/hooks/useMovieService";
import { usePathname } from "next/navigation";
import "../../globals.css";


export function Movie() {
    const pathname = usePathname() || '';
    let { data, isLoading } = useGetMovieDetails(pathname.slice(1));

    return (
        <div className="bg-gray-900 py-20">
            {!isLoading && <MovieDetail movie={data}></MovieDetail>}
        </div>
    );
}

export default Movie;
