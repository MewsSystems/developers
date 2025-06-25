import Image from 'next/image';
import { MovieDetailResponse } from '@/types/api';
import { ChipList } from '@/components/ChipList';
import { formatDate, formatVote } from '@/lib/format';

interface Props {
  movie: MovieDetailResponse;
}

export function MovieDetailsView({ movie }: Props) {
  return (
    <div className="bg-white p-6 rounded-xl shadow space-y-6">
      <div className="flex flex-col sm:flex-row gap-6">
        {movie.poster_url.default && (
          <div className="flex justify-center">
            <Image
              src={movie.poster_url.default}
              alt={`Poster for ${movie.title ?? 'movie'}`}
              width={342}
              height={513}
              className="rounded-md"
              priority
            />
          </div>
        )}

        <div className="space-y-2 text-stone-800">
          <h2 className="text-2xl font-bold">{movie.title}</h2>
          <h3 className="text-lg italic text-stone-600">{movie.original_title}</h3>

          {movie.tagline && <p className="text-purple-700 italic">{movie.tagline}</p>}

          <p>Release Date: {formatDate(movie.release_date)}</p>
          <p>Rating: {formatVote(movie.vote_average * 10)}</p>
          <p>Status: {movie.status}</p>
          <p>Runtime: {movie.runtime} mins</p>

          {movie.spoken_languages.length > 0 && (
            <p>Language: {movie.spoken_languages[0].english_name}</p>
          )}

          {movie.origin_country.length > 0 && (
            <p>Origin Country: {movie.origin_country.join(', ')}</p>
          )}
        </div>
      </div>

      {/* Second Row: Overview */}
      {movie.overview && (
        <div>
          <h4 className="text-lg font-semibold mb-2">Overview</h4>
          <p className="text-stone-700 leading-relaxed">{movie.overview}</p>
        </div>
      )}

      <ChipList
        title="Genres"
        items={movie.genres.map((genre) => genre.name)}
        bgColor="bg-purple-100"
        textColor="text-purple-800"
      />

      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <ChipList
          title="Production Companies"
          items={movie.production_companies.map((company) => company.name)}
          bgColor="bg-stone-100"
          textColor="text-stone-700"
        />
        <ChipList
          title="Production Countries"
          items={movie.production_countries.map((country) => country.name)}
          bgColor="bg-stone-100"
          textColor="text-stone-700"
        />
      </div>
    </div>
  );
}
