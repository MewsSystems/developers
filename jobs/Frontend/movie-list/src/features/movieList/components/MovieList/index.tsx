import { FC } from "react";
import { useGetSearchMoviesQuery } from "../../hooks";

export const MovieList: FC = () => {
  const { data, isLoading } = useGetSearchMoviesQuery({
    query: "avengers",
    page: 1,
  });

  if (isLoading) {
    return <div>{isLoading}</div>;
  }

  if (!data) {
    return <div>Error loading movie list</div>;
  }

  return (
    <ul>
      {data.map((movieItem, key) => {
        return <li key={key}>{movieItem.original_title}</li>;
      })}
    </ul>
  );
};
