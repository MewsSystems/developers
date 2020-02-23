import { SceneMeta } from 'components/SceneMeta'
import { RouteComponentProps } from '@reach/router'
import { useGetMovieDetail } from 'hooks/useGetMovieDetail'
import React from 'react'
import { HeroBanner } from 'components/HeroBanner'
import { Heading } from 'components/Heading'
import { COLORS } from 'constants/colors'
import styled from 'styled-components'
import { Credits } from './components/Credits'
import { Content } from 'components/Content'
import { ErrorContent } from 'components/ErrorContent'
import { useTranslation } from 'react-i18next'
import { Rating } from 'components/Rating'
import { Loading } from './components/Loading'
import { Info } from './components/Info'

const StyledHeroBanner = styled(HeroBanner)`
  display: flex;
  flex-direction: column;
  justify-content: flex-end;
  padding: 2rem;
`

const Title = styled(Heading)`
  margin-bottom: 0.2rem;
  text-shadow: 1px 1px 0 ${COLORS.BLACK};
`

const Overview = styled.p`
  max-width: 45rem;
  margin: 1rem 0 0;
  color: ${COLORS.WHITE};
  text-shadow: 1px 1px 0 ${COLORS.BLACK};
`

const Grid = styled.div`
  display: grid;
  grid-gap: 2rem;
  grid-template-columns: auto 24rem;
`

export interface DetailProps extends RouteComponentProps {
  id?: number
}

export const Detail: React.FC<DetailProps> = ({ id }) => {
  const { t } = useTranslation()
  const { movieDetail, isLoading, hasErrored } = useGetMovieDetail(id)

  if (hasErrored)
    return (
      <ErrorContent
        title={t('error-content.title')}
        text={t('error-content.text')}
      />
    )
  if (isLoading) return <Loading />

  if (movieDetail) {
    const {
      backdrop_path,
      title,
      overview,
      vote_average,
      vote_count,
    } = movieDetail

    return (
      <>
        <SceneMeta title={title} description={overview} />
        <StyledHeroBanner background={backdrop_path && backdrop_path}>
          <Content>
            <Title level={1} color={COLORS.WHITE}>
              {title}
            </Title>
            <Rating rating={vote_average / 2} total={vote_count} />
            <Overview>{overview}</Overview>
          </Content>
        </StyledHeroBanner>
        <Content>
          <Grid>
            <Credits id={id} />
            <Info data={movieDetail} />
          </Grid>
        </Content>
      </>
    )
  }

  return null
}
