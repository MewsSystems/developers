import { useGetMovieList } from "@/hooks/useMovieService";
import IntersectionObserverWrapper from "@/util/IntersectionObserverWrapper";
import { useEffect, useState } from "react";
import LoadingMessage from "./LoadingMessage";
import MovieTile from "./MovieTile";

export default function MovieTable({ searchTerm }: { searchTerm: string }) {
    const [movieList, setMovieList] = useState<any>([]);
    const [currentPage, setCurrentPage] = useState(1);

    let { data, fetchNextPage, isLoading } = useGetMovieList(searchTerm);

    useEffect(() => {
        setMovieList([]);
        setCurrentPage(1);
    }, [searchTerm]);

    useEffect(() => {
        if (data?.pages[currentPage - 1]?.message === 'Not found') {
            setMovieList([]);
        } else {
            setTableEntries();
        }
    }, [data, fetchNextPage]);

    const setTableEntries = () => {
        const currentPageData = data?.pages[currentPage - 1];

        if (currentPageData && Array.isArray(currentPageData.results)) {
            const itemList = currentPageData.results;
            setMovieList((prevList: any) => [...prevList, ...itemList]);
            setCurrentPage(prevPage => prevPage + 1);
        }
    };

    const fetchEntries = fetchNextPage;

    return (
        <>
            {isLoading ? <LoadingMessage /> : <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-8">
                {movieList.map((movie: any) => (
                    <MovieTile key={movie.id} movie={movie} />
                ))}
                <IntersectionObserverWrapper fetchEntries={fetchEntries} />
            </div>
            }
        </>
    );
}
