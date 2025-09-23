import {
  MovieDetailGenres,
  MovieDetailProductionCountries,
} from "src/types/custom";

export const DetailLists = ({
  title,
  data,
}: {
  title: string;
  data: MovieDetailProductionCountries | MovieDetailGenres;
}) => {
  return (
    <>
      <h3 className="mt-4 font-bold">{title}:</h3>
      <ul aria-label={title}>
        {data?.map((d) => (
          <li key={d.name}>{d.name}</li>
        ))}
      </ul>
    </>
  );
};
