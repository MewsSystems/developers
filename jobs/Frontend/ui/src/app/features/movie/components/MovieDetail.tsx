import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/app/components/ui"

import { type MovieDetail as MDetail } from "../types"

import { MovieCardPoster } from "./MovieCardPoster"

export const MovieDetail = ({ movie }: { movie: MDetail }) => {
  return (
    <Card className="flex flex-col lg:flex-row">
      <CardHeader className="space-y-4">
        <CardTitle className="text-balance text-xl font-semibold leading-none tracking-tight">
          {movie.title}
        </CardTitle>
        <CardDescription className="text-sm">{movie.overview}</CardDescription>
      </CardHeader>
      <CardContent className="flex shrink-0 justify-center p-2">
        <MovieCardPoster title={movie.title} posterPath={movie.poster_path} />
      </CardContent>
    </Card>
  )
}
