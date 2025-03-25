import { motion } from "framer-motion";
import Image from "next/image";
import { Card, CardContent } from "~/components/ui/card";
import { castVariant, fadeIn } from "~/lib/animation-variants";
import { type MovieDetailExtended } from "~/types/movie";

interface MovieCastProps {
  movie: MovieDetailExtended;
}

export function MovieCast({ movie }: MovieCastProps) {
  const castPlaceholder = "/placeholders/cast-placeholder.png";

  return (
    <>
      {movie.cast.length > 0 ? (
        <motion.div
          className="space-y-2"
          custom={0.7}
          variants={fadeIn}
          initial="hidden"
          animate="visible"
        >
          <h2 className="space-y-2 text-xl font-semibold text-primary">Cast</h2>
          <div className="grid grid-cols-2 gap-4 md:grid-cols-3">
            {movie?.cast?.slice(0, 5).map((actor) => {
              const castImageUrl = actor.profile_path
                ? `https://media.themoviedb.org/t/p/w138_and_h175_face${actor.profile_path}`
                : castPlaceholder;

              return (
                <motion.div
                  key={actor.id}
                  custom={0.7}
                  variants={castVariant}
                  initial="hidden"
                  animate="visible"
                  whileHover="hover"
                >
                  <Card className="overflow-hidden border-primary/20 bg-background/20 backdrop-blur-sm">
                    <CardContent className="flex items-center gap-3 p-3">
                      <div className="relative h-10 w-10 overflow-hidden rounded-full border border-primary/30 bg-background/70">
                        <Image
                          src={castImageUrl} // Use the calculated castImageUrl here
                          alt={actor.name}
                          fill
                          className="object-cover"
                        />
                      </div>
                      <div>
                        <p className="text-sm font-medium text-white">
                          {actor.name}
                        </p>
                        <p className="text-xs text-primary">
                          {actor.character}
                        </p>
                      </div>
                    </CardContent>
                  </Card>
                </motion.div>
              );
            })}
          </div>
        </motion.div>
      ) : (
        <h2 className="space-y-2 text-xl font-semibold text-muted-foreground">
          No cast available
        </h2>
      )}
    </>
  );
}
