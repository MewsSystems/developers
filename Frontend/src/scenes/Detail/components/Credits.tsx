import React from 'react'
import { useGetMovieCredits } from 'hooks/useGetMoveCredits'
import { useTranslation } from 'react-i18next'
import { Heading } from 'components/Heading'
import styled from 'styled-components'
import { CastCard } from 'components/CastCard'
import { useGetImagePath } from 'hooks/useGetImagePath'
import { ConfigurationProfileSizesEnum } from 'model/api/ConfigurationProfileSizes'
import { ErrorContent } from 'components/ErrorContent'
import { AnimatedLine } from 'components/Loading/AnimatedLine'

const Grid = styled.div`
  display: grid;
  grid-gap: 1rem;
  grid-template-columns: repeat(4, 1fr);
`

export interface CreditsProps {
  id: number
}

export const Credits: React.FC<CreditsProps> = ({ id }) => {
  const { t } = useTranslation('detail')
  const { credits, isLoading, hasErrored } = useGetMovieCredits(id)
  const getImagePath = useGetImagePath(ConfigurationProfileSizesEnum.W_185)

  if (hasErrored)
    return (
      <ErrorContent
        title={t('error-content.title')}
        text={t('error-content.text')}
      />
    )

  if (isLoading) return <AnimatedLine width="20rem" height="2rem" />

  if (credits) {
    const { cast, crew } = credits

    return (
      <div>
        <Heading level={2}>{t('cast')}</Heading>
        <Grid>
          {cast.map(({ id, profile_path, name, character }, index) => (
            <CastCard
              key={id + index}
              background={getImagePath(profile_path)}
              name={name}
              role={character}
            />
          ))}
        </Grid>
        <Heading level={2}>{t('crew')}</Heading>
        <Grid>
          {crew.map(({ id, profile_path, name, job }, index) => (
            <CastCard
              key={id + index}
              background={getImagePath(profile_path)}
              name={name}
              role={job}
            />
          ))}
        </Grid>
      </div>
    )
  }

  return null
}
