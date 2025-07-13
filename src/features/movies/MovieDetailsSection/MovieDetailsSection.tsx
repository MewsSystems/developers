import { BackToSearchLink } from '@/components/BackToSearchLink';
import { MovieDetailsView } from '@/components/MovieDetailsView';
import { MovieDetailResponse } from '@/types/api';

type Props = {
  movieData: MovieDetailResponse | null;
  error?: string | null;
};

export function MovieDetailsSection({ movieData, error }: Props) {
  if (error) return <p>Error loading movie details.</p>;
  if (!movieData) return <p>No movie data found.</p>;

  return (
    <section className="flex flex-col requires-wide-layout gap-2 sm:gap-3">
      <title>{`Search for movies: ${movieData.title}`}</title>
      {movieData.overview && <meta name="description" content={movieData.overview} />}
      <BackToSearchLink movie={movieData} />
      <MovieDetailsView movie={movieData} />
    </section>
  );
}
