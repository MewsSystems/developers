'use client';

import { buildMovieDBUrl } from '@/utils/buildMovieDBUrl';
import Image from 'next/image';
import React, { useEffect, useState } from 'react'

type MovieDetails = {
    adult: boolean;
    backdrop_path: string;
    belongs_to_collection: any;
    budget: number;
    genres: any[];
    homepage: string;
    id: number;
    imdb_id: string;
    origin_country: string[];
    original_language: string;
    original_title: string;
    overview: string;
    popularity: number;
    poster_path: string;
    production_companies: any[];
    production_countries: any[];
    release_date: string;
    revenue: number;
    runtime: number;
    spoken_languages: { english_name: string; iso_639_1: string; name: string; }[]
    status: string;
    tagline: string;
    title: string;
    video: boolean;
    vote_average: number;
    vote_count: number;
}

const getMovieDetails = async (id: number) => {
    const url = buildMovieDBUrl(`movie/${id}`);
    const response = await fetch(url);
    const movie = await response.json();
    return movie;
}

export default function MovieDetails({ params }) {
    const [movie, setMovie] = useState<MovieDetails | null>(null);

    useEffect(() => {
        const fetchMovie = async () => {
            const movie = await getMovieDetails(params.id)
            setMovie(movie);
        }

        fetchMovie();

    }, [params.id])

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
