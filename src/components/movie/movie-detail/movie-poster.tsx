import Image from "next/image";
import { motion } from "framer-motion";
import { Card } from "~/components/ui/card";
import { type MovieDetails } from "~/types/movie";
import { posterVariants } from "~/lib/animation-variants";
import { env } from "~/env";

interface MoviePosterProps {
  movie: MovieDetails;
}

export function MoviePoster({ movie }: MoviePosterProps) {
  const moviePlaceholder = "/placeholders/poster-placeholder.png";

  const moviePosterUrl = movie.poster_path
    ? `${env.NEXT_PUBLIC_TMDB_IMAGE_URL}${movie.poster_path}`
    : moviePlaceholder;

  return (
    <motion.div
      className="md:col-span-1"
      variants={posterVariants}
      initial="hidden"
      animate="visible"
      exit="hidden"
    >
      <Card className="overflow-hidden border-primary/20 bg-background/20 backdrop-blur-sm">
        <div className="relative aspect-[2/3] overflow-hidden">
          <Image
            src={moviePosterUrl}
            alt={movie.title}
            fill
            sizes="(max-widths: 900px) 100vw 33vw"
            className="object-cover"
          />
        </div>
      </Card>
    </motion.div>
  );
}
