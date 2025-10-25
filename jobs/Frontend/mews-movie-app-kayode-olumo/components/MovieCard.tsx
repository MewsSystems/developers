"use client"

import { useState } from "react"
import Image from "next/image"
import Link from "next/link"
import type { Movie } from "@/lib/types"
import { getImageUrl } from "@/lib/tmdb"
import { formatYear } from "@/lib/utils/formatters"
import { Skeleton } from "@/components/ui/skeleton"

interface MovieCardProps {
  movie: Movie
}

export function MovieCard({ movie }: MovieCardProps) {
  const [imageLoaded, setImageLoaded] = useState(false)

  return (
    <Link href={`/movie/${movie.id}`} className="group block">
      <div className="overflow-hidden rounded-xl transition-all duration-300 group-hover:shadow-xl group-hover:scale-105">
        <div className="aspect-[2/3] relative bg-gray-200">
          {!imageLoaded && <Skeleton className="absolute inset-0 rounded-none" />}
          <Image
            src={getImageUrl(movie.poster_path) || "/placeholder.svg?height=450&width=300"}
            alt={movie.title}
            fill
            className="object-cover"
            sizes="(max-width: 640px) 50vw, (max-width: 768px) 33vw, (max-width: 1024px) 25vw, 20vw"
            onLoad={() => setImageLoaded(true)}
          />
        </div>
      </div>
      <div className="mt-3">
        <h3 className="font-semibold text-sm leading-tight line-clamp-2 group-hover:text-primary transition-colors">
          {movie.title}
        </h3>
            {movie.release_date && (
              <p className="text-xs text-muted-foreground mt-1">{formatYear(movie.release_date)}</p>
            )}
      </div>
    </Link>
  )
}
