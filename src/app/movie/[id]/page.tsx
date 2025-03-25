"use client";

import { ArrowLeft } from "lucide-react";
import { notFound, useParams, useRouter } from "next/navigation";
import { useCallback } from "react";
import { MovieCast } from "~/components/movie/movie-detail/movie-cast";
import { MovieHeader } from "~/components/movie/movie-detail/movie-header";
import { MovieInfo } from "~/components/movie/movie-detail/movie-info";
import { MoviePoster } from "~/components/movie/movie-detail/movie-poster";
import { Button } from "~/components/ui/button";
import { Loader } from "~/components/ui/loader";
import { api } from "~/trpc/react";

export default function MovieDetail() {
  const router = useRouter();
  const params = useParams();
  const id = Number(params?.id);

  const { data: movie, isLoading } = api.movie.getDetails.useQuery({ id });

  const handleBack = useCallback(() => {
    router.back();
  }, [router]);

  if (isLoading) return <Loader isLoading={isLoading} />;
  if (!movie) return notFound();

  return (
    <div className="min-h-screen bg-black text-white">
      <div className="container mx-auto px-4 py-8">
        <Button
          variant="outline"
          onClick={handleBack}
          className="mb-6 border-primary/30 text-primary hover:bg-primary/10 hover:text-primary"
        >
          <ArrowLeft /> Go Back
        </Button>
        <div className="grid gap-8 md:grid-cols-3">
          <MoviePoster movie={movie} />
          <div className="space-y-4 md:col-span-2">
            <MovieHeader movie={movie} />
            <MovieInfo movie={movie} />
            <MovieCast movie={movie} />
          </div>
        </div>
      </div>
    </div>
  );
}
