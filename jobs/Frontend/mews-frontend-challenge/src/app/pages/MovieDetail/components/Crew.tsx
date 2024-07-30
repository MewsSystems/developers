import { getTMDBPersonLink } from "@/app/services/tmdb";
import { BaseComponentProps } from "@/types";
import { Crew as CrewData } from "@/app/services/tmdb";

export type CrewProps = BaseComponentProps & {
  crew: CrewData[];
};

export function Crew({ crew, ...props }: CrewProps) {
  return (
    <ul className="grid grid-cols-2 gap-4 lg:grid-cols-3" {...props}>
      {crew.map((crewMember) => (
        <li key={crewMember.id}>
          <a
            aria-label={`See more information about ${crewMember.name}`}
            target="_blank"
            rel="noopener noreferrer"
            href={crewMember.id ? getTMDBPersonLink(crewMember.id) : undefined}
          >
            {crewMember.name}
          </a>
        </li>
      ))}
    </ul>
  );
}
