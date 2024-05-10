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

const getPopular = async () => {
    const url = buildMovieDBUrl("movie/now_playing");
    const options = { method: "GET", headers: { accept: "application/json" } };

    const response = await fetch(url, options);
    const data = await response.json();
    return data.results as Movie[];
};

export default function Popular() {
    const [popular, setPopular] = useState<Movie[]>([]);
    const { push } = useRouter();

    useEffect(() => {
        const fetch = async () => {
            const response = await getPopular();
            setPopular(response);
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

                    popular &&
                    popular.slice(0, 10).map(popularMovie => <VerticalCard
                        key={popularMovie.id}
                        movie={popularMovie} handleClick={handleClick} />)}
            </List>
        </Section>
    )
}
