import { buildMovieDBUrl } from '@/utils/buildMovieDBUrl';
import Image from 'next/image';
import React from 'react'

const getMovieDetails = async (id: number) => {
    try {
        const url = buildMovieDBUrl(`movie/${id}`);
        const response = await fetch(url);
        const movie = await response.json();
        return movie as unknown as MovieDetails;
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
            {movie && <div>
                <Image
                    width={190}
                    height={290}
                    src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
                    alt={movie.original_title ?? 'Movie Image'}
                />
                <div>
                    <h2><strong>{movie.title}</strong>({movie.release_date})</h2>
                    <p>{movie.release_date} - {movie.genres.map(genre => genre.name)} - {movie.runtime}</p>
                    <p>{movie.vote_average} User score</p>

                    <p>{movie.tagline}</p>
                    Overview
                    <p>{movie.overview}</p>

                </div>
            </div>}
        </section>
    )
}
