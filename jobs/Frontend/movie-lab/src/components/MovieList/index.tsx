'use client';

import { useEffect, useState } from 'react';
import { MovieType } from '@/src/types/movie';
import { useRouter, useSearchParams } from "next/navigation";
import {
  Card,
  CloseButton,
  Grid, LeftColumn,
  ModalContent,
  ModalWrapper,
  MovieDetails,
  MovieImage, RightColumn, Image
} from "@/src/components/MovieList/style";

export default function MovieList({ data, totalPages }: { data: MovieType[], totalPages: number }) {
  const router = useRouter();
  const searchParams = useSearchParams();
  const searchPage = parseInt(searchParams.get("page") ?? "1", 10);
  const searchQuery = searchParams.get('q');

  const [page, setPage] = useState<number>(searchPage);
  const [selectedMovie, setSelectedMovie] = useState<MovieType | null>(null);

  useEffect(() => {
    if(searchPage === 1){
      setPage(searchPage);
    }
  }, [searchPage]);

  const handleScroll = () => {
    console.log('handleScroll')
    if (
      window.innerHeight + document.documentElement.scrollTop
      === document.documentElement.offsetHeight
    ) {
      setPage(prev => prev + 1);
    }
  };

  useEffect(() => {
    window.addEventListener('scroll', handleScroll);
    return () => window.removeEventListener('scroll', handleScroll);
  }, []); // Add event listener on mount and remove on unmount

  useEffect(() => {
    let url = searchQuery ? `/?q=${searchQuery}` : '/?';

    if (page > 1) {
      url += (searchQuery ? '&' : '') + `page=${page > totalPages ? totalPages : page}`;
    }

    router.push(url, { scroll: false });
  }, [page, router]);

  useEffect(() => {
    let url = searchQuery ? `/?q=${searchQuery}` : '/?';

    router.push(url, { scroll: false });
  }, [searchQuery, router]);

  const openModal = (movie: MovieType) => {
    setSelectedMovie(movie);
  };

  const closeModal = () => {
    setSelectedMovie(null);
  };

  return (
    <>
      <Grid>
        {data?.map((movie: MovieType, index: number) => (
          movie.poster_path ?
            <Card key={`${movie.id}-${index}`} onClick={() => openModal(movie)}>
              {movie.poster_path &&
                <MovieImage
                  src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
                  alt={movie.title}
                />}
              <MovieDetails>
                <h3>{movie.title}</h3>
                <p>{movie.overview}</p>
              </MovieDetails>
            </Card>
            : null
        ))}
      </Grid>

      {selectedMovie && (
        <ModalWrapper>
          <ModalContent>
            <CloseButton onClick={closeModal}>&times;</CloseButton>
            <LeftColumn>
              {selectedMovie.poster_path && (
                <Image
                  src={`https://image.tmdb.org/t/p/w500${selectedMovie.poster_path}`}
                  alt={selectedMovie.title}
                />
              )}
            </LeftColumn>
            <RightColumn>
              <h1>{selectedMovie.title}</h1>
              <h5>{new Date(selectedMovie.release_date).getFullYear()}</h5>
              <p>{selectedMovie.overview}</p>
            </RightColumn>
          </ModalContent>
        </ModalWrapper>
      )}
    </>
  );
}
