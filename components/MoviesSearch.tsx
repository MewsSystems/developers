import { useSearchParams } from 'next/navigation';
import React from 'react';
import Header from './Header';
import MovieTable from './MovieTable';

export default function MoviesSearch() {
    const searchParams = useSearchParams();
    const query = searchParams?.get('query')?.toString() || ''

    return (
        <div className="bg-gray-900 min-h-screen py-20">
            <div className="mx-auto px-6">
                <Header />
                <MovieTable searchTerm={query} />
            </div>
        </div>
    );
}

