import React from 'react'
import styled from 'styled-components'
import { LINEAR_GRADIENT } from 'constants/index'
import { useGetImagePath } from 'hooks/useGetImagePath'
import { ConfigurationBackdropSizesEnum } from 'model/api/ConfigurationBackdropSizesEnum'
import { COLORS } from 'constants/colors'

export const StyledHeroBanner = styled.div`
  height: 28rem;
  background-color: ${COLORS.GRAY};
  background-repeat: no-repeat;
  background-position: 50% 50%;
  background-size: cover;
`

export interface HeroBannerProps {
  background?: string
  children?: React.ReactNode | React.ReactNodeArray
  className?: string
}

export const HeroBanner: React.FC<HeroBannerProps> = ({
  background,
  children,
  className,
}) => {
  const imagePath = useGetImagePath(ConfigurationBackdropSizesEnum.W_1280)(
    background
  )
  const backgroundImage = `${LINEAR_GRADIENT.DARKER}, url(${imagePath})`

  return (
    <StyledHeroBanner style={{ backgroundImage }} className={className}>
      {children}
    </StyledHeroBanner>
  )
}
