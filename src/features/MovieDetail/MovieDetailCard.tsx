import { Card, CardContent } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import {
  Star,
  Calendar,
  Clock,
  Globe,
  Flag,
  Film,
  Tv,
  Link as LinkIcon,
  Home,
} from 'lucide-react'
import { MovieDetail } from '@/schemas/movieDetail'
import { IMAGE_BASE_URL } from '@/lib/utils'

type Props = {
  movie: MovieDetail
}

export const MovieDetailCard = ({ movie }: Props) => {
  const formatDate = (dateString: string | null) => {
    if (!dateString) return 'Unknown'
    const date = new Date(dateString)
    return date.toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric',
    })
  }

  return (
    <Card className="flex flex-col md:flex-row items-start gap-6 p-4 shadow-lg">
      <div className="flex flex-col gap-4 items-center self-center md:self-start w-full md:w-auto">
        {movie.poster_path ? (
          <img
            src={`${IMAGE_BASE_URL}w500${movie.poster_path}`}
            alt={movie.title}
            className="w-64 rounded-lg shadow-md"
          />
        ) : (
          <div className="w-64 h-96 bg-gray-300 rounded-lg flex items-center justify-center">
            <Film size={48} className="text-gray-400" />
          </div>
        )}

        <div className="flex flex-col gap-2 w-full">
          <div className="flex items-center gap-2">
            <Star className="w-5 h-5 text-yellow-500" />
            <span className="text-lg font-medium">
              {movie.vote_average.toFixed(1)} ({movie.vote_count} votes)
            </span>
          </div>

          {movie.runtime && (
            <div className="flex items-center gap-2">
              <Clock className="w-4 h-4 text-gray-500" />
              <span>{movie.runtime} minutes</span>
            </div>
          )}

          <div className="flex items-center gap-2">
            <Calendar className="w-4 h-4 text-gray-500" />
            <span>{formatDate(movie.release_date)}</span>
          </div>
        </div>
      </div>

      <CardContent className="flex flex-col gap-4 w-full p-0 md:p-4">
        <div className="flex flex-col gap-1">
          <h2 className="text-3xl font-bold">{movie.title}</h2>
          {movie.original_title !== movie.title && (
            <h3 className="text-xl text-gray-600">{movie.original_title}</h3>
          )}
        </div>

        {movie.tagline && (
          <div className="italic text-gray-600">"{movie.tagline}"</div>
        )}

        <p className="text-gray-700">
          {movie.overview || 'No description available.'}
        </p>

        {movie.belongs_to_collection && (
          <div className="bg-blue-50 p-3 rounded-md border border-blue-200">
            <h3 className="font-semibold">
              Part of the {movie.belongs_to_collection.name} Collection
            </h3>
          </div>
        )}

        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          <div>
            <h3 className="font-semibold flex items-center gap-1">
              <Film className="w-4 h-4" /> Genres
            </h3>
            <div className="flex gap-2 flex-wrap mt-1">
              {movie.genres.length > 0 ? (
                movie.genres.map((genre) => (
                  <Badge key={genre.id} variant="secondary">
                    {genre.name}
                  </Badge>
                ))
              ) : (
                <span className="text-gray-500">No genres listed</span>
              )}
            </div>
          </div>

          {movie.spoken_languages.length > 0 && (
            <div>
              <h3 className="font-semibold flex items-center gap-1">
                <Globe className="w-4 h-4" /> Languages
              </h3>
              <div className="flex gap-2 flex-wrap mt-1">
                {movie.spoken_languages.map((language) => (
                  <Badge key={language.iso_639_1} variant="outline">
                    {language.english_name}
                  </Badge>
                ))}
              </div>
            </div>
          )}

          {movie.production_countries.length > 0 && (
            <div>
              <h3 className="font-semibold flex items-center gap-1">
                <Flag className="w-4 h-4" /> Production Countries
              </h3>
              <div className="flex gap-2 flex-wrap mt-1">
                {movie.production_countries.map((country) => (
                  <Badge key={country.iso_3166_1} variant="outline">
                    {country.name} ({country.iso_3166_1})
                  </Badge>
                ))}
              </div>
            </div>
          )}

          {movie.production_companies.length > 0 && (
            <div>
              <h3 className="font-semibold flex items-center gap-1">
                <Tv className="w-4 h-4" /> Production Companies
              </h3>
              <div className="flex gap-4 flex-wrap mt-1">
                {movie.production_companies.map((company) => (
                  <div key={company.id} className="flex items-center gap-2">
                    {company.logo_path && (
                      <img
                        src={`${IMAGE_BASE_URL}w200${company.logo_path}`}
                        alt={company.name}
                        className="w-12 h-12 object-contain"
                      />
                    )}
                    <span>{company.name}</span>
                    {company.origin_country && (
                      <small className="text-gray-500">
                        ({company.origin_country})
                      </small>
                    )}
                  </div>
                ))}
              </div>
            </div>
          )}
        </div>

        <div className="flex flex-wrap gap-4 mt-2">
          {movie.homepage && (
            <a
              href={movie.homepage}
              target="_blank"
              rel="noopener noreferrer"
              className="text-blue-600 hover:underline flex items-center gap-1"
            >
              <Home className="w-4 h-4" /> Official Website
            </a>
          )}

          {movie.imdb_id && (
            <a
              href={`https://www.imdb.com/title/${movie.imdb_id}`}
              target="_blank"
              rel="noopener noreferrer"
              className="text-blue-600 hover:underline flex items-center gap-1"
            >
              <LinkIcon className="w-4 h-4" /> View on IMDb
            </a>
          )}
        </div>

        {movie.adult && (
          <Badge variant="destructive" className="self-start mt-2">
            Adult Content
          </Badge>
        )}

        {movie.status && movie.status !== 'Released' && (
          <Badge variant="outline" className="self-start">
            Status: {movie.status}
          </Badge>
        )}
      </CardContent>
    </Card>
  )
}
