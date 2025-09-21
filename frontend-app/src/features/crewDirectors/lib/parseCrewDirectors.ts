import { formatList } from "@/shared/lib/utils";
import type { Crew } from "@/entities/movie/types";
import type { CrewDirectorDetails } from "@/features/crewDirectors/types";

export function parseCrewDirectors({
  crew,
  language,
}: {
  crew: Crew[];
  language: string;
}): CrewDirectorDetails[] {
  const filterBy = "Director";
  return crewEntries(filterBy, language, crew);
}

function crewEntries(filterBy: string, language: string, crew?: Crew[]) {
  const crewMap: Record<string, string[]> = (crew ?? []).reduce(
    (crewMap: Record<string, string[]>, crewItem) => {
      crewMap[crewItem.name] = crewMap[crewItem.name] ?? [];
      crewMap[crewItem.name].push(crewItem.job);
      return crewMap;
    },
    {}
  );

  return Object.entries(crewMap)
    .filter((entry) => entry[1].includes(filterBy))
    .map(([name, jobs]) => ({ name, jobs: formatList(jobs, language) }));
}
