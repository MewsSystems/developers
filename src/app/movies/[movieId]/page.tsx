import { use } from 'react';

interface MoviePageProps {
  params: Promise<{
    movieId: string;
  }>;
}

export default function MoviePage({ params }: MoviePageProps) {
  const { movieId } = use(params);

  return (
    <section className="space-y-4">
      <h1 className="text-stone-800 text-xl font-bold">Movie Details</h1>
      <p className="text-stone-600">Movie ID: {movieId}</p>
    </section>
  );
}
