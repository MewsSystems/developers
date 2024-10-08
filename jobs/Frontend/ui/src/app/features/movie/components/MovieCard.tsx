import Link from "next/link"

import { Card, CardContent, CardHeader, CardTitle } from "@/app/components/ui"

import { Movie } from "../types"

import { MovieCardPoster } from "./MovieCardPoster"

export const MovieCard = ({ movie }: { movie: Movie }) => (
  <Card className="flex flex-col bg-secondary">
    <Link href={`/movie/${movie.id}`}>
      <CardHeader className="flex items-center p-2">
        <MovieCardPoster title={movie.title} posterPath={movie.poster_path} />
      </CardHeader>
      <CardContent className="p-2">
        <CardTitle className="text-balance text-center text-xl font-semibold leading-none tracking-tight">
          {movie.title}
        </CardTitle>
      </CardContent>
    </Link>
  </Card>
)
