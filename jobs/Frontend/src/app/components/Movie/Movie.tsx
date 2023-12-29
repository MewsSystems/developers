'use client'

import { useGetConfigQuery } from '@/lib/features/api/apiSlice'
import type { Movie as MovieType } from '@/lib/types'
import {
  Poster,
  ReleaseDate,
  Title,
  VoteAverage,
  Wrapper,
} from './Movie.styles'
import { PosterImage, Typography } from '@/components'
import Link from 'next/link'
import { getFormattedDate } from '@/app/utils'

export const Movie = ({
  id,
  title,
  poster_path,
  release_date,
  vote_average,
}: MovieType) => {
  const { data, isLoading } = useGetConfigQuery({})

  if (isLoading || !data) {
    return
  }

  const { secure_base_url, poster_sizes = [] } = data.images
  const detailHref = `/movie/${id}-${title}`

  const posterSrc = `${secure_base_url}${poster_sizes[3]}${poster_path}`
  const releaseDate = getFormattedDate(new Date(release_date))

  return (
    <Wrapper data-testid="movie">
      <Link href={detailHref}>
        <Title>
          <Typography variant="primarySpan" data-testid="movie-name">
            {title}
            <VoteAverage>
              <Typography>{Math.round(vote_average * 10)}%</Typography>
            </VoteAverage>
          </Typography>
        </Title>
      </Link>
      <Poster>
        <ReleaseDate>
          <Typography variant="secondarySpan">{releaseDate}</Typography>
        </ReleaseDate>
        <Link href={detailHref}>
          <PosterImage
            src={posterSrc}
            width={342}
            height={513}
            alt={`${title} poster`}
          />
        </Link>
      </Poster>
    </Wrapper>
  )
}
