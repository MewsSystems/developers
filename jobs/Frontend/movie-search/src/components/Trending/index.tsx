import { buildMovieDBUrl } from '@/utils/buildMovieDBUrl';
import React, { useEffect, useState } from 'react'
import { Movie } from '@/types/Movie';
import { useRouter } from 'next/navigation';
import styled from 'styled-components';
import { VerticalCard } from '../Card/VerticalCard';

const List = styled.ul`
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(120px, 1fr));
    gap: 18px;
    width: 90vw;
`

const Section = styled.section`
    h2 {
        margin-bottom: 24px;
    }
`

const getTrending = async () => {
    const url = buildMovieDBUrl("trending/movie/day");
    const options = { method: "GET", headers: { accept: "application/json" } };

    const response = await fetch(url, options);
    const data = await response.json();
    return data.results as Movie[];
};

export default function Trending() {
    const [trending, setTrending] = useState<Movie[]>([]);
    const { push } = useRouter();

    useEffect(() => {
        const fetch = async () => {
            const response = await getTrending();
            setTrending(response);
        }
        fetch();
    }, [])

    const handleClick = (movieId: number) => {
        push(`/movies/${movieId}`)
    }

    return (
        <Section>
            <h2>Trending today</h2>
            <List>
                {

                    trending &&
                    trending.slice(0, 10).map(trendingMovie => <VerticalCard
                        key={trendingMovie.id}
                        movie={trendingMovie} handleClick={handleClick} />)}
            </List>
        </Section>
    )
}
