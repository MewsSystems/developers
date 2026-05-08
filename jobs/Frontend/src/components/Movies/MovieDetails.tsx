import { Loader2Icon } from "lucide-react";
import { Card, CardContent, CardFooter } from "../ui/card";
import { MovieDetailsResponse, MovieSearchResponse } from "src/types/custom";
import { DetailLists } from "./DetailLists";
import { DataPoint } from "./DataPoint";

type SearchTypes = {
  data?: MovieDetailsResponse;
  error: unknown;
  isLoading: boolean;
  isFetching: boolean;
};

export const MovieDetails = ({
  movieDetails,
}: {
  movieDetails: SearchTypes;
}) => {
  const movie = movieDetails.data;

  return (
    <>
      {movie ? (
        <Card className="h-[76vh] w-full rounded-sm">
          {/* <h2 className="ml-4 py-2 text-left">
            {movie?.original_title} Details:
          </h2> */}

          <CardContent className="h-[75vh] overflow-scroll p-2" tabIndex={0}>
            {movie.poster_path ? (
              <img
                alt={movie.title}
                width="120px"
                className="mb-4 self-center"
                src={`https://image.tmdb.org/t/p/original/${movie.poster_path}`}
              />
            ) : (
              <img width="120px" src="/no-image-placeholder.png" />
            )}

            <dl className="flex flex-col">
              <DataPoint title="Movie Title" value={movie.title} />
              <DataPoint title="Release date" value={movie.release_date} />
              <DataPoint title="Runtime" value={movie.runtime} />
            </dl>
            <DetailLists title="Genres" data={movie.genres} />
            <DetailLists
              title="Production Countries"
              data={movie.production_countries}
            />
          </CardContent>
        </Card>
      ) : (
        <Placeholder loading={movieDetails.isFetching} />
      )}
    </>
  );
};

const Placeholder = ({ loading }: { loading: boolean }) => {
  return (
    <div className="h-[76vh] w-full rounded-sm bg-white">
      <h2 className="ml-4 py-2 text-left">Movie details here:</h2>
      {loading && (
        <Loader2Icon
          width={50}
          height={50}
          className="align-center ml-auto mr-auto mt-[50%] animate-spin"
        />
      )}
    </div>
  );
};
