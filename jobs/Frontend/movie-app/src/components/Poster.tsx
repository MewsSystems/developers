import styled from "styled-components"

const PosterImage = styled.img<{ $width: string; $height: string }>`
  height: ${(props) => props.$height};
  width: ${(props) => props.$width};
`

const PlaceholderPoster = styled.div<{ $width: string; $height: string }>`
  height: ${(props) => props.$height};
  width: ${(props) => props.$width};
  display: flex;
  align-items: center;
  justify-content: center;
  border: 1px solid ${(props) => props.theme.light_gray};
  color: ${(props) => props.theme.light_gray};
`

function Poster({
  url,
  title,
  height = "70px",
  width = "46px",
}: {
  url?: string
  title: string
  height?: string
  width?: string
}) {
  const imageUrl = `https://image.tmdb.org/t/p/w440_and_h660_face/${url}`
  return url ? (
    <PosterImage
      src={imageUrl}
      alt={`${title} poster`}
      $height={height}
      $width={width}
    />
  ) : (
    <PlaceholderPoster
      title="Poster not found"
      $height={height}
      $width={width}
    />
  )
}

export default Poster
