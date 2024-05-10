import { buildMovieDBUrl } from '@/utils/buildMovieDBUrl';
import React, { useEffect, useState } from 'react'
import { Card } from '../Card';
import { Movie } from '@/types/Movie';


const getTrending = async () => {

    const url = buildMovieDBUrl("trending/movie/day");
    const options = { method: "GET", headers: { accept: "application/json" } };

    const response = await fetch(url, options);
    const data = await response.json();
    return data.results as Movie[];
};

export default function Trending() {
    const [trending, setTrending] = useState<Movie[]>([]);
    useEffect(() => {
        const fetch = async () => {
            const response = await getTrending();
            setTrending(response);
        }
        fetch();
    }, [])
    return (
        <div>

            <h2>Trending</h2>
            {
                trending && trending.map(trendingMovie => <Card key={trendingMovie.id} movie={trendingMovie} handleClick={() => { }} />)}</div>
    )
}
