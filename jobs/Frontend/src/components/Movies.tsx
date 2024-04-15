import SearchInput from './inputs/SearchInput';
import { useState } from 'react';
import { MoviesList } from './MoviesList';

export default function Movies() {
    const [query, setQuery] = useState('');

    return (
        <>
            <SearchInput query={query} setQuery={setQuery} />
            { !!query && <MoviesList query={query} /> }
        </>
    );
}

