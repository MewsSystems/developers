import Link from 'next/link'
import {
  PosterHeader,
  StyledBrowse,
  StyledChevronLeftIcon,
  StyledLink,
} from './Poster.styles'
import { Logo, PosterImage, Typography } from '@/components'
import { useGetConfigQuery } from '@/lib/features/api/apiSlice'

type Props = {
  poster_path: string
  title: string
}

export const Poster = ({ poster_path, title }: Props) => {
  const { data, isLoading } = useGetConfigQuery({})

  if (isLoading || !data) {
    return
  }

  const { secure_base_url, poster_sizes = [] } = data.images
  const posterSrc = `${secure_base_url}${poster_sizes[5]}${poster_path}`

  return (
    <>
      <PosterHeader>
        <Link href="/" data-testid="back-button">
          <Logo isColorless />
        </Link>
        <StyledLink href="/">
          <Typography>
            <StyledChevronLeftIcon />
            <StyledBrowse>Browse</StyledBrowse>
          </Typography>
        </StyledLink>
      </PosterHeader>
      <PosterImage
        width={600}
        height={900}
        src={posterSrc}
        alt={`${title} poster`}
      />
    </>
  )
}
