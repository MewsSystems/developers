export interface PageList<T> {
    results: T[];
    page: number; total_pages: number;
    total_results: number;
}

export interface Results<T> {
    results: T[];
    id: number;
}
