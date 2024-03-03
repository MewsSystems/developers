import React from "react";
import { MovieDetail } from "@/scenes/MovieDetail/services/types";
import { Badge } from "@/components/ui/badge";
import { Calendar, Clock } from "lucide-react";
import { Button } from "@/components/ui/button";

type MovieDetailContentProps = {
  movie: MovieDetail;
};
const MovieDetailContent = ({ movie }: MovieDetailContentProps) => {
  return (
    <>
      <h1 className="text-5xl font-bold">{movie.title}</h1>
      <h2 className="text-2xl font-semibold">{movie.tagline}</h2>
      <div className="flex flex-row gap-2">
        {movie.genres.map((genre) => (
          <Badge key={genre.id}>{genre.name}</Badge>
        ))}
      </div>
      <span className="flex flex-row gap-2">
        <Calendar />
        {movie.release_date ? (
          <p>Released on {new Date(movie.release_date).toLocaleDateString()}</p>
        ) : (
          <p>No release date</p>
        )}
      </span>
      <span className="flex flex-row gap-2">
        <Clock /> <p>{movie.runtime} minutes</p>
      </span>
      <p className="text-justify">{movie.overview}</p>
      <p>
        Produced by:{" "}
        <strong>
          {movie.production_companies.map((company) => company.name).join(", ")}
        </strong>
      </p>
      <div className="flex flex-row gap-2">
        {movie.imdb_id && (
          <a
            href={`https://www.imdb.com/title/${movie.imdb_id}`}
            target="_blank"
          >
            <Button size="sm" variant="secondary">
              IMDB
            </Button>
          </a>
        )}
        {movie.homepage && (
          <a href={movie.homepage} target="_blank">
            <Button size="sm" variant="secondary">
              Homepage
            </Button>
          </a>
        )}
      </div>
    </>
  );
};

export default MovieDetailContent;
