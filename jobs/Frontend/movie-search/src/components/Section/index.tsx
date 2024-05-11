import React, { useEffect, useState } from 'react'
import { Movie } from '@/types/Movie';
import { VerticalCard } from '../Card/VerticalCard';
import { useRouter } from 'next/navigation';
import styled from 'styled-components';

const List = styled.ul`
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(140px, 1fr));
    gap: 12px;
`

const Section = styled.section`
    width: 100%;
`;

const SectionTitle = styled.h2`
     margin-bottom: 24px;
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
            <SectionTitle>{title}</SectionTitle>
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
