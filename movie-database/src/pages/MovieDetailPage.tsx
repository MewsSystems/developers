import { useMovieDetail } from "@/hooks/useMovieDetail";
import Tag from "@/components/ui/Tag";
import { useParams } from "react-router";
import { StarIcon } from "lucide-react";

const MovieDetailPage = () => {
  const { id } = useParams();
  const { data, isLoading, isError } = useMovieDetail(Number(id));

  return (
    <>
      {isLoading && <div>Loading...</div>}
      {isError && <div>Error</div>}
      {data && (
        <>
          <div className="flex flex-col md:flex-row mx-auto gap-8 w-2/3 h-1/2">
            <div className="rounded-xl overflow-hidden relative w-full md:max-w-md h-40 md:h-auto">
              <img className="absolute inset-0 w-full h-full object-cover" src={data.image} alt={data.title} />
            </div>
            <div className="flex flex-col gap-4">
              <div>
                <h2>{data.title}</h2>
                <div className="flex gap-4 items-center text-lg text-slate-100">
                  <div>{data.year}</div>
                  <div>|</div>
                  <div className="flex gap-1 items-center">
                    <StarIcon size={16} />
                    <div>{data.voteAverage}</div>
                    <div>({data.voteCount})</div>
                  </div>
                </div>
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
