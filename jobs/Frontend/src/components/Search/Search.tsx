import { Loader2Icon } from "lucide-react";
import { Card, CardContent, CardFooter } from "../ui/card";
import { MovieSearchResponse } from "src/types/custom";

import { useNavigate, useSearchParams } from "react-router-dom";
import { SearchPagination } from "./SearchPagination";

type SearchTypes = {
  data?: MovieSearchResponse;
  error: unknown;
  isLoading: boolean;
  isFetching: boolean;
};

export const SearchResults = ({ movies }: { movies: SearchTypes }) => {
  const navigate = useNavigate();

  const [searchParams] = useSearchParams();

  return (
    <>
      {movies.data ? (
        <Card className="h-full w-full rounded-sm">
          <h2 className="ml-4 py-2 text-left">Results:</h2>
          <CardContent className="h-[65vh] overflow-scroll p-0" tabIndex={0}>
            {movies.data && (
              <ul>
                {movies?.data?.results?.map((m) => {
                  return (
                    <li
                      key={m.id}
                      className="mb-6 flex cursor-pointer rounded-sm p-2 transition-all hover:bg-gray-200"
                      onClick={() => {
                        navigate({
                          pathname: `/${m.id}`,
                          search: `?search=${searchParams.get(
                            "search"
                          )}&page=${searchParams.get("page")}`,
                        });
                      }}
                    >
                      {m.poster_path ? (
                        <img
                          alt={m.title}
                          width="120px"
                          src={`https://image.tmdb.org/t/p/original/${m.poster_path}`}
                        />
                      ) : (
                        <img width="120px" src="/no-image-placeholder.png" />
                      )}
                      <div className="flex flex-col text-left">
                        <p className="ml-2 font-bold">{m.title}</p>
                        <small className="ml-2">{m.overview}</small>
                        <small className="ml-2 mt-auto font-bold">
                          {m.release_date}
                        </small>
                      </div>
                    </li>
                  );
                })}
              </ul>
            )}
          </CardContent>
          <CardFooter className="p-0">
            {movies.data && <SearchPagination data={movies.data} />}
          </CardFooter>
        </Card>
      ) : (
        <Placeholder loading={movies.isFetching} />
      )}
    </>
  );
};

const Placeholder = ({ loading }: { loading: boolean }) => {
  return (
    <div className="h-[76vh] w-full rounded-sm bg-white">
      <h2 className="ml-4 py-2 text-left">Movie search here:</h2>
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
