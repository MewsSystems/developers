import { BackToSearchLink } from '@/components/BackToSearchLink';
import { MovieDetailsView } from '@/components/MovieDetailsView';
import { MovieDetailResponse } from '@/types/api';

type Props = {
  movieData: MovieDetailResponse | null;
  error?: string | null;
};

export default function MovieDetailsSection({ movieData, error }: Props) {
  if (error) return <p>Error loading movie details.</p>;
  if (!movieData) return <p>No movie data found.</p>;

  return (
    <section className="space-y-4">
      <title>{`Search for movies: ${movieData.title}`}</title>
      <BackToSearchLink />
      <MovieDetailsView movie={movieData} />
    </section>
  );
}
