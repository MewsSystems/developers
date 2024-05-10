import { buildMovieDBUrl } from '@/utils/buildMovieDBUrl';
import React, { useEffect, useState } from 'react'
import { Movie } from '@/types/Movie';
import { VerticalCard } from '../Card/VerticalCard';
import { useRouter } from 'next/navigation';
import styled from 'styled-components';

const List = styled.ul`
    display: flex;
    gap: 10px;
`

const Section = styled.section`
    h2 {
        margin-bottom: 24px;
    }
`

const getTrending = async () => {
    const url = buildMovieDBUrl("movie/now_playing");
    const options = { method: "GET", headers: { accept: "application/json" } };

    const response = await fetch(url, options);
    const data = await response.json();
    return data.results as Movie[];
};

export default function Popular() {
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
            <h2>Now playing</h2>
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
