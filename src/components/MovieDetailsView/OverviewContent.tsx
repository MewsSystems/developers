import { ChipList } from '@/components/ChipList';
import { MovieDetailResponse } from '@/types/api';

interface OverviewContentProps {
  movie: MovieDetailResponse;
}

export function OverviewContent({ movie }: OverviewContentProps) {
  const hasProductionCompanies = !!movie.production_companies.length;
  const hasProductionCountries = !!movie.production_countries.length;
  const hasGenres = !!movie.genres.length;
  const hasOverview = !!movie.overview;

  return (
    <>
      {hasOverview && (
        <div>
          <h3 className="text-lg font-semibold mb-2">Overview</h3>
          <p className="text-stone-700 leading-relaxed">{movie.overview}</p>
        </div>
      )}

      {hasGenres && (
        <ChipList
          title="Genres"
          items={movie.genres.map((genre) => genre.name)}
          bgColor="bg-purple-100"
          textColor="text-purple-800"
        />
      )}

      <div
        className={`grid grid-cols-1 gap-4 ${hasProductionCompanies && hasProductionCountries ? 'sm:grid-cols-2 lg:grid-cols-1' : ''}`}
      >
        {hasProductionCompanies && (
          <ChipList
            title="Production Companies"
            items={movie.production_companies.map((company) => company.name)}
            bgColor="bg-stone-100"
            textColor="text-stone-700"
          />
        )}
        {hasProductionCountries && (
          <ChipList
            title="Production Countries"
            items={movie.production_countries.map((country) => country.name)}
            bgColor="bg-stone-100"
            textColor="text-stone-700"
          />
        )}
      </div>
    </>
  );
}
