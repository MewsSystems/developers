import { useId } from 'react';
import { Label, TextInput } from './SearchInput.styled';

interface SearchInputProps {
    label: string;
    query: string;
    setQuery: (query: string) => void;
}

export default function SearchInput({label, query, setQuery}: SearchInputProps) {
    const inputId = useId();

    return (
        <>
            <Label htmlFor={inputId}>{label}</Label>
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