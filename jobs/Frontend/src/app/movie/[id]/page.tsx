'use client'

import { useGetMovieByIdQuery } from '@/lib/features/api/apiSlice'
import { DetailContainer } from './page.styles'
import {
  CreditsTable,
  MovieTrailer,
  MovieHeader,
  MovieOverview,
  Poster,
} from './components'
import notFound from './not-found'

type Props = {
  params: {
    id: string
  }
}

export default function MovieDetailPage(props: Props) {
  const id = props.params.id
  const { data, isLoading, isError } = useGetMovieByIdQuery({ id })

  if (isError) {
    return notFound()
  }

  if (isLoading || !data) {
    return
  }

  const {
    title,
    poster_path,
    vote_average,
    release_date,
    videos,
    runtime,
    overview,
    credits,
    genres,
  } = data

  return (
    <>
      <div>
        <Poster poster_path={poster_path} title={data.title} />
      </div>

      <DetailContainer>
        <MovieHeader
          title={title}
          vote_average={vote_average}
          release_date={release_date}
          runtime={runtime}
        />
        <MovieOverview overview={overview} />
        <CreditsTable credits={credits} genres={genres} />
        <MovieTrailer videos={videos} />
      </DetailContainer>
    </>
  )
}
