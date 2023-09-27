import { Image } from "antd"
import styled from "styled-components"

export const Header = styled.div`
  display: grid;
  grid-template-columns: 200px auto;
  grid-gap: 2em;
`

export const StyledImage = styled(Image)`
  width: 100%;
  height: 100%;
  object-fit: cover;
`

export const MobileCoverImage = styled(Image)`
  width: 100%;
  height: 100px;
`

export const MovieTitle = styled.h1`
  font-size: 2em;
  margin-bottom: 0.5em;
`

export const OriginalMovieTitle = styled.h2`
  font-size: 1.5em;
  margin-bottom: 0.5em;
`

export const AdditionalInfo = styled.div`
  display: flex;
  flex-wrap: wrap;
  margin-bottom: 2em;
  margin-top: 2em;
`

export const AdditionalInfoItem = styled.div`
  flex: 0 0 auto;
  margin-right: 3em;
  margin-bottom: 1em;
`

export const Label = styled.div`
  font-weight: bold;
  margin-bottom: 0.5em;
`

export const Description = styled.div``
