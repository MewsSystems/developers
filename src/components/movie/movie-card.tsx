import Image from "next/image";
import { Star } from "lucide-react";
import { memo } from "react";
import { format } from "date-fns";
import { type Movie } from "~/types/movie";
import { Badge } from "../ui/badge";
import { Card, CardContent, CardFooter } from "../ui/card";
import { env } from "~/env";
interface MovieCardProps {
  movie: Movie;
  onClick?: () => void;
}

const MovieCard = memo(function MovieCard({ movie, onClick }: MovieCardProps) {
  const moviePlaceholder = "/placeholders/poster-placeholder.png";

  const formattedDate = movie.release_date
    ? format(new Date(movie.release_date), "MMM dd, yyyy")
    : null;

  const moviePosterUrl = movie.poster_path
    ? `${env.NEXT_PUBLIC_TMDB_IMAGE_URL}${movie.poster_path}`
    : moviePlaceholder;

  return (
    <Card
      className="group relative flex h-full cursor-pointer flex-col overflow-hidden border-primary/20 bg-background/20 backdrop-blur-sm"
      onClick={onClick}
    >
      <div className="relative z-10 flex flex-grow flex-col overflow-hidden rounded-lg bg-black">
        <div className="relative aspect-[2/3] w-full overflow-hidden">
          <Image
            src={moviePosterUrl}
            alt={movie.title}
            fill
            className="object-cover transition-transform duration-700 group-hover:scale-110"
            sizes="(max-width: 640px) 100vw, (max-width: 768px) 50vw, (max-width: 1024px) 33vw, 25vw"
          />
          <div
            className="absolute inset-0 bg-gradient-to-t from-black via-transparent to-transparent opacity-80"
            aria-hidden="true"
          />
          <div className="absolute bottom-0 left-0 right-0 p-3">
            <div className="flex items-center gap-2">
              <Badge
                variant="outline"
                className="flex items-center gap-1 border-yellow-500/50 bg-black/80 text-yellow-400 backdrop-blur-sm"
              >
                <Star className="h-3 w-3 fill-yellow-400" aria-hidden="true" />
                <span className="text-xs">{movie.vote_average.toFixed(1)}</span>
              </Badge>
              {formattedDate && (
                <Badge
                  variant="outline"
                  className="border-primary/50 bg-black/80 backdrop-blur-sm"
                >
                  <span className="text-xs">{formattedDate}</span>
                </Badge>
              )}
            </div>
          </div>
        </div>

        <CardContent className="flex-grow p-4">
          <h3 className="mb-1 line-clamp-1 text-lg font-semibold text-white transition-colors group-hover:text-primary">
            {movie.title}
          </h3>
          <p className="line-clamp-2 text-sm text-muted-foreground">
            {movie.overview}
          </p>
        </CardContent>

        <CardFooter className="p-4 pt-0">
          <Badge
            variant="outline"
            className="ml-auto border-primary/50 bg-background/50 text-primary transition-all group-hover:border-primary group-hover:bg-primary/20"
          >
            View Details
          </Badge>
        </CardFooter>
      </div>
    </Card>
  );
});

export default MovieCard;
