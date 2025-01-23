import { Movie } from "@types"
import { CSSProperties } from "react"
import Loading from "@components/Loading"
import { CalendarIcon } from "@heroicons/react/24/outline"
import { formatDate } from "@utils/dates"

interface RowProps {
  movie: Movie | undefined
  style: CSSProperties
  onClick?: () => void
}

export const Row = ({ movie, style, onClick }: RowProps) => {
  return movie ? (
    <div style={style} className="border-b border-gray-300 p-2" onClick={() => onClick?.()}>
      <div className="flex cursor-pointer items-center">
        <img
          src={`https://image.tmdb.org/t/p/w200${movie.poster_path ?? movie.backdrop_path ?? ""}`}
          alt={movie.title}
          className="h-24 w-16 flex-shrink-0 object-cover"
        />
        <div className="ml-4 w-96 flex-1">
          <h2 className="truncate text-lg font-semibold" title="The Movie Title">
            {movie.title}
          </h2>
          {movie.release_date ? (
            <p className="flex items-center py-1 text-sm text-gray-600">
              <CalendarIcon className="mr-1 h-4 w-4" />
              <span title="Release Date">{formatDate(movie.release_date)}</span>
            </p>
          ) : null}
          <p className="truncate py-1 text-sm text-gray-900" title="Overview">
            {movie.overview}
          </p>
        </div>
      </div>
    </div>
  ) : (
    <div style={style} className="flex items-center justify-center border-b border-gray-200 p-2">
      <Loading />
    </div>
  )
}

export default Row
