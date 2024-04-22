import { Cast as CastData } from "@/app/services/tmdb";
import { getTMDBPersonLink } from "@/app/services/tmdb/tmdb.utils";
import { BaseComponentProps } from "@/types";

export type CastProps = BaseComponentProps & {
  cast: CastData[];
};

export function Cast({ cast }: CastProps) {
  // We show only the first 3 cast members and their role
  const stars = cast.slice(0, 3);

  return (
    <ul className="grid grid-cols-2 gap-4 lg:grid-cols-3">
      {stars.map((cast) => (
        <li key={cast.id}>
          <a
            aria-label={`See more information about ${cast.name}`}
            target="_blank"
            rel="noopener noreferrer"
            href={cast.id ? getTMDBPersonLink(cast.id) : undefined}
          >
            {cast.name}
          </a>
          <div className="text-sm text-secondary">{cast.character}</div>
        </li>
      ))}
    </ul>
  );
}
