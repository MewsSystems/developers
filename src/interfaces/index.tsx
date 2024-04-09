export interface SearchFormProps {
    handleSearch: (term: string) => void;
    term: string;
}

export interface MovieGridProps {
    term: string;
    page: number;
    setPage: (page: number) => void;
}

export interface PaginationProps {
    page: number;
    total_pages: number;
    onPreviousPage: () => void;
    onNextPage: () => void;
}

export interface MovieResult {
    page: number;
    results: [];
    total_pages: number;
    total_results: number;
}

export interface Movie {
    id: string;
    title: string;
    overview: string;
    release_date: number;
    original_language: string;
    poster_path: string;
    vote_average: string;
}
