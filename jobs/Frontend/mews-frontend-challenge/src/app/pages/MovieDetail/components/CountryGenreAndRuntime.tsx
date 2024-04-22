import { Genre } from "@/app/services/tmdb";
import { BaseComponentProps } from "@/types";
import { formatDuration, intervalToDuration } from "date-fns";

export type CountryGenreAndRuntimeProps = BaseComponentProps & {
  country?: string[];
  genres?: Genre[];
  runtime: number;
};

export function CountryGenreAndRuntime({
  country,
  genres,
  runtime,
  ...props
}: CountryGenreAndRuntimeProps) {
  return (
    <div className="flex flex-wrap text-secondary" {...props}>
      {country && (
        <span
          className={"after:px-2 after:content-['·'] last:after:content-none "}
        >
          {country.join(", ")}
        </span>
      )}
      {genres && (
        <span
          className={"after:px-2 after:content-['·'] last:after:content-none "}
        >
          {getGenreLabel(genres)}
        </span>
      )}
      {!!runtime && <span>{getRuntimeFromMinutes(runtime)}</span>}
    </div>
  );
}

function getGenreLabel(genres: Genre[] = []) {
  return genres?.map(({ name }) => name).join(", ");
}

function getRuntimeFromMinutes(runtime: number) {
  const duration = intervalToDuration({ start: 0, end: runtime * 60 * 1000 });

  return formatDuration(duration, { format: ["hours", "minutes"] });
}
