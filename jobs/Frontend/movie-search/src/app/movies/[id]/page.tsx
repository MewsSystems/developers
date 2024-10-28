import Details from '@/components/Details';
import { MovieDetails as MD } from '@/types/MovieDetail';
import { buildMovieDBUrl } from '@/utils/buildMovieDBUrl';
import React from 'react'

const getMovieDetails = async (id: number) => {
    try {
        const url = buildMovieDBUrl(`movie/${id}`);
        const response = await fetch(url);
        const movie = await response.json();
        return movie as unknown as MD;
    } catch (error) {
        console.error('Error fetching movie data:', error);
        return null;
    }
}

export default async function MovieDetails({
    params: { id },
}: {
    params: { id: string }
}) {
    const movie = await getMovieDetails(+id);

    return (
        <section>
            {movie && <Details movie={movie} />}
        </section>
    )
}
