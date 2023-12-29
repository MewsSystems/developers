'use client'

import { useState } from 'react'
import { StyledImage } from './PosterImage.styles'

type Props = {
  src: string
  alt: string
  width: number
  height: number
}

export const PosterImage = ({ src, alt, width, height }: Props) => {
  const [posterSrc, setPosterSrc] = useState(src)

  return (
    <StyledImage
      src={posterSrc}
      alt={alt}
      width={width}
      height={height}
      placeholder="blur"
      data-testid="poster-image"
      blurDataURL="/poster_fallback.svg"
      onError={() => {
        // set fallback image if poster is not available
        setPosterSrc('/poster_fallback.svg')
      }}
    />
  )
}
