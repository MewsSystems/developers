import { useId } from 'react';

export interface SearchInputData {
    query: string;
    setQuery: (query: string) => void;
}

export default function SearchInput({ query, setQuery }: SearchInputData) {
    const inputId = useId();

    return (
        <>
            <label htmlFor={inputId}>Search</label>
            <input
                autoFocus
                id={inputId}
                type="search"
                maxLength={1_000}
                value={query}
                onChange={(e) => setQuery(e.target.value)}
            />
        </>
    );
}