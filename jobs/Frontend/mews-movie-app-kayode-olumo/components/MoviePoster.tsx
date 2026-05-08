"use client"

import { useState } from "react"
import Image from "next/image"
import { Skeleton } from "@/components/ui/skeleton"

interface MoviePosterProps {
  src: string
  alt: string
  priority?: boolean
}

export function MoviePoster({ src, alt, priority = false }: MoviePosterProps) {
  const [imageLoaded, setImageLoaded] = useState(false)

  return (
    <div className="aspect-[2/3] relative rounded-xl overflow-hidden shadow-lg">
      {!imageLoaded && <Skeleton className="absolute inset-0 rounded-none" />}
      <Image
        src={src || "/placeholder.svg"}
        alt={alt}
        fill
        className="object-cover"
        priority={priority}
        onLoad={() => setImageLoaded(true)}
      />
    </div>
  )
}
