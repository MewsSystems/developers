import { fetchMovieDetails } from '@/lib/fetch/app-serverside/fetchMovieDetails';
import { MovieDetailsSection } from '@/features/movies/MovieDetailsSection';
import { movieIdSlugSchema } from '@/lib/slug';

export const revalidate = 3600;

type MoviePageProps = {
  params: Promise<{
    movieId: string;
  }>;
};

export default async function MoviePage({ params }: MoviePageProps) {
  const { movieId: movieSlug } = await params;

  const parsedSlug = movieIdSlugSchema.safeParse(movieSlug);
  if (!parsedSlug.success) {
    return <section className="text-red-600 p-4">Invalid movie slug format.</section>;
  }

  const match = parsedSlug.data.match(/^(\d+)-/);
  const movieId = match?.[1];

  let movieData = null;
  let fetchError = null;
  try {
    movieData = await fetchMovieDetails(movieId!);
  } catch {
    fetchError = 'Failed to fetch movie details.';
  }

  return <MovieDetailsSection movieData={movieData} error={fetchError} />;
}
