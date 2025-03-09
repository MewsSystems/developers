import { Card, CardContent } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Star } from 'lucide-react'
import { MovieDetail } from '@/schemas/movieDetail'

type Props = {
  movie: MovieDetail
}
export const MovieDetailCard = ({ movie }: Props) => {
  return (
    <Card className="flex flex-col md:flex-row items-center gap-6 p-4 shadow-lg">
      {movie.poster_path ? (
        <img
          src={`https://image.tmdb.org/t/p/w500${movie.poster_path}`}
          alt={movie.title}
          className="w-64 rounded-lg shadow-md"
        />
      ) : (
        <div className="w-64 h-96 bg-gray-300 rounded-lg flex items-center justify-center">
          No Image
        </div>
      )}
      <CardContent className="flex flex-col gap-4 w-full">
        <h2 className="text-3xl font-bold">{movie.title}</h2>
        <div className="flex items-center gap-2">
          <Star className="w-5 h-5 text-yellow-500" />
          <span className="text-lg font-medium">
            {movie.vote_average.toFixed(1)}
          </span>
        </div>
        <p className="text-gray-600">
          {movie.overview || 'No description available.'}
        </p>
        <div>
          <h3 className="font-semibold">Genres</h3>
          <div className="flex gap-2 flex-wrap mt-1">
            {movie.genres.map((genre) => (
              <Badge key={genre.id} variant="secondary">
                {genre.name}
              </Badge>
            ))}
          </div>
        </div>
        <div>
          <h3 className="font-semibold">Production Companies</h3>
          <div className="flex gap-4 flex-wrap mt-1">
            {movie.production_companies.map((company) => (
              <div key={company.id} className="flex items-center gap-2">
                {company.logo_path && (
                  <img
                    src={`https://image.tmdb.org/t/p/w200${company.logo_path}`}
                    alt={company.name}
                    className="w-12 h-12 object-contain"
                  />
                )}
                <span>{company.name}</span>
              </div>
            ))}
          </div>
        </div>
      </CardContent>
    </Card>
  )
}
