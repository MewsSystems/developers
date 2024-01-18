import { Label } from "@/components/ui/label";
import { MoviesDetailsFull } from "@/lib/types";

export default function MoviesDetails({
  movieDetails,
}: {
  movieDetails: MoviesDetailsFull | undefined;
}) {
  return (
    <div className="grid md:grid-cols-2 gap-6 lg:gap-12 items-start max-w-6xl px-4 mx-auto py-6">
      <div className="grid md:grid-cols-5 gap-3 items-start">
        <div className="md:col-span-4">
          <img
            alt="Movie Poster"
            className="aspect-[2/3] object-cover border border-gray-200 w-full rounded-lg overflow-hidden dark:border-gray-800"
            height={900}
            src={`https://image.tmdb.org/t/p/w500${movieDetails?.poster_path}`}
            width={600}
          />
        </div>
      </div>
      <div className="grid gap-4 md:gap-10 items-start">
        <div className="hidden md:flex items-start">
          <div className="grid gap-4">
            <h1
              className="font-bold text-3xl lg:text-4xl"
              id="movie_title"
              aria-placeholder=""
            >
              {movieDetails?.original_title}
            </h1>
            <div>
              <p>Rating: {movieDetails?.vote_average.toFixed(1)}</p>
              <p>Release Date: {movieDetails?.release_date}</p>
            </div>
          </div>
        </div>
        <div className="grid gap-2">
          <Label className="text-base" htmlFor="description">
            Description
          </Label>
          <p>{movieDetails?.overview}</p>
        </div>
      </div>
    </div>
  );
}
