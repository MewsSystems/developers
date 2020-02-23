import React, { useCallback } from 'react'
import styled from 'styled-components'
import { Star } from 'components/Star'
import { COLORS } from 'constants/colors'
import { useTranslation } from 'react-i18next'

const Container = styled.div`
  display: grid;
  grid-gap: 0.6rem;
  grid-template-columns: min-content auto;
`

const Stars = styled.div`
  display: grid;
  grid-gap: 0.2rem;
  grid-template-columns: repeat(5, 1fr);
`

const Value = styled.span`
  color: ${COLORS.GOLD};
  font-weight: bold;
  text-shadow: 1px 1px 0 ${COLORS.BLACK};
`

const Total = styled.span`
  color: ${COLORS.WHITE};
  font-size: 0.8rem;
  text-shadow: 1px 1px 0 ${COLORS.BLACK};
`

export interface RatingProps {
  rating: number
  total: number
}

export const Rating: React.FC<RatingProps> = ({ rating, total }) => {
  const { t } = useTranslation('rating')

  const renderStars = useCallback(
    () =>
      Array(5)
        .fill(0)
        .map((_, i) => {
          if (i < rating) {
            if (i + 1 > rating) {
              const ratingDiff = Math.abs(i - rating)

              if (ratingDiff > 0 && ratingDiff <= 0.25) {
                return <Star key={i} type="empty" color={COLORS.GOLD} />
              } else if (ratingDiff > 0.25 && ratingDiff <= 0.75) {
                return <Star key={i} type="half" color={COLORS.GOLD} />
              } else {
                return <Star key={i} type="full" color={COLORS.GOLD} />
              }
            } else {
              return <Star key={i} type="full" color={COLORS.GOLD} />
            }
          } else {
            return <Star key={i} type="empty" color={COLORS.GOLD} />
          }
        }),
    [rating]
  )

  return (
    <Container>
      <Stars>{renderStars()}</Stars>
      <div>
        <Value>{rating}</Value> <Total>({t('out-of', { total })})</Total>
      </div>
    </Container>
  )
}
