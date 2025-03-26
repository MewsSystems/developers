import { useMovieDetail } from "@/hooks/useMovieDetail";
import Tag from "@/components/ui/Tag";
import { useParams } from "react-router";
import { Skeleton } from "@/components/ui/skeleton";
import YearAndRating from "@/components/YearAndRating";

const MovieDetailPage = () => {
  const { id } = useParams();
  const { data, isLoading, isError } = useMovieDetail(Number(id));

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
      {isError && <div>Error</div>}
      {data && (
        <>
          <div className="flex flex-col md:flex-row mx-auto gap-8 w-2/3 h-1/2">
            <div className="rounded-xl overflow-hidden relative w-full md:max-w-md h-40 md:h-full">
              <img className="absolute inset-0 w-full h-full object-cover" src={data.image} alt={data.title} />
            </div>
            <div className="flex flex-col gap-4">
              <div>
                <h2>{data.title}</h2>
                <YearAndRating size="lg" rating={data.voteAverage} ratingCount={data.voteCount} year={data.year} />
              </div>
              <div className="flex flex-wrap gap-2">
                {data.genres.map((genre) => (
                  <Tag key={genre.id}>{genre.name}</Tag>
                ))}
              </div>
              <div>
                <h3>Overview</h3>
                <p className="text-justify">{data.overview}</p>
              </div>
            </div>
          </div>
        </>
      )}
    </>
  );
};

export default MovieDetailPage;
