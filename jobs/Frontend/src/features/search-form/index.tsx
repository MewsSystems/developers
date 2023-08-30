import { useEffect, useState } from "react";
import { useDebounce } from "use-debounce";
import { Conditional } from "../../components/conditional";
import { setKeyword, getMoviesList, clearResults, selectState } from "./slice";
import { SearchInput } from "./search-input";
import { SearchResults } from "./search-results";
import { useAppDispatch, useAppSelector } from "state/hooks";
import { PaginationControl } from "./pagination-control";
import { ErrorMessage } from "components/error-message";

export const SearchForm = () => {
    const { keyword, movies, page, totalPages, status } = useAppSelector(selectState);
    const dispatch = useAppDispatch();

    const [tempKeyword, setTempKeyword] = useState(keyword);
    const [debouncedKeyword] = useDebounce(tempKeyword, 500);

    const handlePageChange = (newPage: number) => {
        dispatch(getMoviesList({ query: debouncedKeyword, page: newPage }));
    };

    useEffect(() => {
        if (debouncedKeyword === keyword)
            return;

        if (debouncedKeyword.length < 3) {
            dispatch(clearResults());
        }
        else {
            dispatch(setKeyword(debouncedKeyword));
            dispatch(getMoviesList({ query: debouncedKeyword, page: 1 }));
        }
    // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [debouncedKeyword]);

    if (status === 'failed') {
        return (
            <ErrorMessage
                header="There is a problem contacting the search service"
                message="Check your internet connection and try again..."
            />
        )
    }

    return (
        <>
            <SearchInput
                keyword={tempKeyword}
                onChange={setTempKeyword}
            />
            <Conditional showIf={debouncedKeyword.length > 2}>
                <SearchResults
                    page={page}
                    totalPages={totalPages}
                    movies={movies}
                />
                <PaginationControl
                    page={page}
                    totalPages={totalPages}
                    onChange={handlePageChange}
                />
            </Conditional>
        </>
    )
}