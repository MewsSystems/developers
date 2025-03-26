import { useMovieDetail } from "@/hooks/useMovieDetail";
import Tag from "@/components/ui/Tag";
import { useParams } from "react-router";
import { Skeleton } from "@/components/ui/Skeleton";
import YearAndRating from "@/components/YearAndRating";
import ErrorAlert from "@/components/ErrorAlert";

const MovieDetailPage = () => {
  const { id } = useParams();
  const { data, isLoading, isError, error } = useMovieDetail(Number(id));

  const loadingSkeleton = (
    <div className="flex gap-8 w-2/3 mx-auto justify-center">
      <Skeleton className="h-80 w-80 " />
      <div className="space-y-8 w-full">
        <Skeleton className="h-10 w-40" />
        <Skeleton className="h-10 w-40" />
        <Skeleton className="h-20 w-" />
      </div>
    </div>
  );

  return (
    <>
      {isLoading && loadingSkeleton}
      {isError && <ErrorAlert error={error} />}
      {data && (
        <div className="flex flex-col items-center md:flex-row md:items-start gap-8 md:w-2/3">
          <div className="flex flex-col gap-4 md:order-1">
            <div>
              <h2 className="mb-2">{data.title}</h2>
              <YearAndRating size="lg" rating={data.voteAverage} ratingCount={data.voteCount} year={data.year} />
              <div className="text-foreground-secondary mt-2">
                Origin: {data.originCountries.join(', ')}
              </div>
            </div>
            <div className="flex flex-wrap items-center gap-2">
              {data.genres.map((genre) => (
                <Tag key={genre.id}>{genre.name}</Tag>
              ))}
            </div>
            <div>
              <h3>Overview</h3>
              <p className="text-justify">{data.overview}</p>
            </div>
          </div>
          <div className="w-1/2 sm:w-1/3 shrink-0 max-w-lg">
            <img className="w-full rounded-xl" src={data.image} alt={data.title} />
          </div>
        </div>
      )}
    </>
  );
};

export default MovieDetailPage;
