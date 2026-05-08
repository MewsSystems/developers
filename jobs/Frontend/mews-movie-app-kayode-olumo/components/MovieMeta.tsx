import { Calendar, Clock, Star } from "lucide-react"
import { formatYear, formatRuntime } from "@/lib/utils/formatters"

interface MovieMetaProps {
  vote_average: number
  vote_count: number
  release_date?: string
  runtime?: number
}

export function MovieMeta({ vote_average, vote_count, release_date, runtime }: MovieMetaProps) {
  return (
    <div className="flex flex-wrap gap-4 sm:gap-6 text-sm">
      <div className="flex items-center gap-2">
        <Star className="h-4 w-4 fill-primary text-primary" />
        <span className="font-semibold">{vote_average.toFixed(1)}</span>
        <span className="text-muted-foreground">({vote_count.toLocaleString()} votes)</span>
      </div>

      {release_date && (
        <div className="flex items-center gap-2">
          <Calendar className="h-4 w-4 text-muted-foreground" />
          <span>{formatYear(release_date)}</span>
        </div>
      )}

      {runtime && runtime > 0 && (
        <div className="flex items-center gap-2">
          <Clock className="h-4 w-4 text-muted-foreground" />
          <span>{formatRuntime(runtime)}</span>
        </div>
      )}
    </div>
  )
}
