type MoviePageProps = {
  params: {
    movieId: string;
  };
};

export default function MoviePage({ params }: MoviePageProps) {
  return (
    <section className="space-y-4">
      <h1 className="text-stone-800 text-xl font-bold">Movie Details</h1>
      <p className="text-stone-600">Movie ID: {params.movieId}</p>
    </section>
  );
}
