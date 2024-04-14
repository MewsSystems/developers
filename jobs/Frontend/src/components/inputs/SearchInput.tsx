export interface SearchInputData {
    query: string;
    setQuery: (query: string) => void;
}

export default function SearchInput({ query, setQuery }: SearchInputData) {
    // todo: add label
    return (
        <input
            type="search"
            maxLength={1_000}
            value={query}
            onChange={(e) => setQuery(e.target.value)}
        />
    );
}