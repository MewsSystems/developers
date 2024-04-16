import { useId } from 'react';
import { Label, TextInput } from './SearchInput.styled';

export interface SearchInputData {
    query: string;
    setQuery: (query: string) => void;
}

export default function SearchInput({ query, setQuery }: SearchInputData) {
    const inputId = useId();

    return (
        <>
            <Label htmlFor={inputId}>Search movies</Label>
            <TextInput
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