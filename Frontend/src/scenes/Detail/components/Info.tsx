import React from 'react'
import { MovieDetail } from 'model/api/MovieDetail'
import styled from 'styled-components'
import { useTranslation } from 'react-i18next'
import { COLORS } from 'constants/colors'
import { BOX_SHADOW, BORDER_RADIUS } from 'constants/index'
import { Heading } from 'components/Heading'
import { InfoItem } from './InfoItem'
import { DATE_FORMAT } from 'constants/date'
import { format } from 'date-fns'

const Grid = styled.div`
  display: grid;
  grid-gap: 0.5rem;
  padding: 1rem;
  background: ${COLORS.LIGHT_GRAY};
  border-radius: ${BORDER_RADIUS.MEDIUM};
  box-shadow: ${BOX_SHADOW.MEDIUM};
`

export interface InfoProps {
  data: MovieDetail
}

export const Info: React.FC<InfoProps> = ({
  data: { release_date, homepage, runtime, original_title },
}) => {
  const { t } = useTranslation('detail')

  return (
    <div>
      <Heading level={2}>{t('info.title')}</Heading>
      <Grid>
        <InfoItem label={t('info.release-date')}>
          {format(new Date(release_date), DATE_FORMAT)}
        </InfoItem>
        <InfoItem label={t('info.homepage')}>{homepage}</InfoItem>
        <InfoItem label={t('info.runtime')}>
          {runtime} {t('minutes')}
        </InfoItem>
        <InfoItem label={t('info.original-title')}>{original_title}</InfoItem>
      </Grid>
    </div>
  )
}
