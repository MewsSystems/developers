import React, { useEffect, useState } from 'react'
import { Movie } from '@/types/Movie';
import { VerticalCard } from '../Card/VerticalCard';
import { useRouter } from 'next/navigation';
import styled from 'styled-components';

const List = styled.ul`
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(130px, 1fr));
    gap: 18px;
    width: 90vw;
`

const Section = styled.section`
    h2 {
        margin-bottom: 24px;
    }
`

type DynamicSectionProps = { title: string, getMovies: () => Promise<Movie[]> }

export default function DynamicSection({ title, getMovies }: DynamicSectionProps) {
    const [movies, setMovies] = useState<Movie[]>([]);
    const { push } = useRouter();

    useEffect(() => {
        const fetch = async () => {
            const response = await getMovies();
            setMovies(response);
        }
        fetch();
    }, [getMovies])

    const handleClick = (movieId: number) => {
        push(`/movies/${movieId}`)
    }

    return (
        <Section>
            <h2>{title}</h2>
            <List>
                {

                    movies &&
                    movies.slice(0, 10).map(movie => <VerticalCard
                        key={movie.id}
                        movie={movie} handleClick={handleClick} />)}
            </List>
        </Section>
    )
}
