"use client";

import { motion } from "framer-motion";
import { Card, CardContent } from "~/components/ui/card";
import { fadeIn } from "~/lib/animation-variants";
import { type MovieDetailExtended } from "~/types/movie";

interface MovieInfoProps {
  movie: MovieDetailExtended;
}

export function MovieInfo({ movie }: MovieInfoProps) {
  return (
    <>
      <motion.div
        className="space-y-3"
        custom={0.6}
        variants={fadeIn}
        initial="hidden"
        animate="visible"
      >
        {movie.overview && (
          <>
            <h2 className="mb-2 text-xl font-semibold text-primary">
              Overview
            </h2>
            <p className="text-muted-foreground">{movie.overview}</p>
          </>
        )}
      </motion.div>
      <motion.div
        className="space-y-3"
        custom={0.8}
        variants={fadeIn}
        initial="hidden"
        animate="visible"
      >
        <Card className="border-primary/20 bg-background/20 backdrop-blur-sm">
          <CardContent className="p-6">
            <div className="grid grid-cols-2 gap-4">
              <div>
                <h3 className="font-semibold text-primary">Director</h3>
                <p className="text-muted-foreground">
                  {movie.director?.name && movie.director?.name !== ""
                    ? movie.director?.name
                    : "Not available"}
                </p>
              </div>

              <div>
                <h3 className="font-semibold text-primary">Budget</h3>
                <p className="text-muted-foreground">
                  {movie.budget.toLocaleString() === "0"
                    ? "Not available"
                    : `$${movie.budget.toLocaleString()}`}
                </p>
              </div>

              <div>
                <h3 className="font-semibold text-primary">Revenue</h3>
                <p className="text-muted-foreground">
                  {movie.revenue.toLocaleString() === "0"
                    ? "Not available"
                    : `$${movie.revenue.toLocaleString()}`}
                </p>
              </div>

              <div>
                <h3 className="font-semibold text-primary">Production</h3>
                <p className="text-muted-foreground">
                  {movie.production_companies
                    .map((production) => production.name)
                    .join(", ") !== ""
                    ? movie.production_companies
                        .map((production) => production.name)
                        .join(", ")
                    : "Unknown production company"}
                </p>
              </div>
            </div>
          </CardContent>
        </Card>
      </motion.div>
    </>
  );
}
