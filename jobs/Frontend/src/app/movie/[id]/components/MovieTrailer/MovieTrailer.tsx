import type { Videos } from '@/lib/types'
import { StyledIframe, StyledTitle } from './MovieTrailer.styles'
import { Typography } from '@/components'

type Props = {
  videos: Videos
}

export const MovieTrailer = ({ videos }: Props) => {
  const trailer = videos.results.find(
    ({ site, type }) => site === 'YouTube' && type === 'Trailer',
  )

  if (!trailer) {
    return
  }

  return (
    <>
      <StyledTitle>
        <Typography variant="secondarySpan">Trailer</Typography>
      </StyledTitle>
      <StyledIframe
        width="100%"
        height="100%"
        src={`https://www.youtube.com/embed/${trailer.key}`}
        allowFullScreen
        title="Trailer"
      />
    </>
  )
}
