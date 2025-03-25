"use client";

import { motion } from "framer-motion";
import { Calendar, Clock, Star } from "lucide-react";
import { Badge } from "~/components/ui/badge";
import { format } from "date-fns";
import { fadeIn } from "~/lib/animation-variants";
import { type MovieDetails } from "~/types/movie";

interface MovieHeaderProps {
  movie: MovieDetails;
}

export function MovieHeader({ movie }: MovieHeaderProps) {
  const formattedDate = movie.release_date
    ? format(new Date(movie.release_date), "MMM dd, yyyy")
    : null;

  return (
    <>
      <motion.h1
        className="mb-2 bg-gradient-to-r from-primary to-[#0072FF] bg-clip-text text-3xl font-bold text-transparent md:text-4xl"
        custom={0.4}
        variants={fadeIn}
        initial="hidden"
        animate="visible"
      >
        {movie.title}
      </motion.h1>
      <motion.div
        className="mb-6 flex flex-wrap items-center gap-3"
        custom={0.6}
        variants={fadeIn}
        initial="hidden"
        animate="visible"
      >
        <Badge
          variant="outline"
          className="flex items-center gap-1 border-yellow-500/50 bg-background/50 text-yellow-400"
        >
          <Star className="h-3.5 w-3.5 fill-yellow-400" aria-hidden="true" />
          <span>{movie.vote_average.toFixed(1)}</span>
        </Badge>
        <Badge
          variant="outline"
          className="flex items-center gap-1 border-primary/50 bg-background/50"
        >
          <Clock className="h-3.5 w-3.5" aria-hidden="true" />
          <span>{movie.runtime} min</span>
        </Badge>
        {formattedDate && (
          <Badge
            variant="outline"
            className="flex items-center gap-1 border-primary/50 bg-background/50"
          >
            <Calendar className="h-3.5 w-3.5" aria-hidden="true" />
            <span>{formattedDate}</span>
          </Badge>
        )}
      </motion.div>
      <motion.div
        className="mb-6 flex flex-wrap gap-2"
        custom={0.7}
        variants={fadeIn}
        initial="hidden"
        animate="visible"
      >
        {movie.genres.map((genre) => (
          <Badge
            key={genre.id}
            variant="outline"
            className="flex items-center gap-1 border-purple-500/30 bg-purple-500/10 text-white"
          >
            {genre.name}
          </Badge>
        ))}
      </motion.div>
    </>
  );
}
