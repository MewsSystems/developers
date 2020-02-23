import React from 'react'
import styled from 'styled-components'
import { COLORS } from 'constants/colors'
import { BORDER_RADIUS, BOX_SHADOW } from 'constants/index'
import { Heading } from './Heading'

const StyledCastCard = styled.div`
  display: flex;
  flex-direction: column;
  height: 16rem;
  border: 1px solid ${COLORS.GRAY};
  border-radius: ${BORDER_RADIUS.MEDIUM};
  box-shadow: ${BOX_SHADOW.MEDIUM};
`

const Image = styled.div`
  height: 10rem;
  background-color: ${COLORS.GRAY};
  background-size: cover;
  border-bottom: 1px solid ${COLORS.GRAY};
  border-top-left-radius: ${BORDER_RADIUS.MEDIUM};
  border-top-right-radius: ${BORDER_RADIUS.MEDIUM};
`

const Bottom = styled.div`
  display: flex;
  flex: 1 1;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  margin: 1rem;
`

const Name = styled(Heading)`
  margin: 0 0 0.3rem;
  font-size: 0.8rem;
  text-align: center;
`

const Role = styled.p`
  font-size: 0.6rem;
  text-align: center;
`

export interface CastCardProps {
  background?: string
  name: string
  role: string
}

export const CastCard: React.FC<CastCardProps> = ({
  background,
  name,
  role,
}) => (
  <StyledCastCard>
    <Image style={{ backgroundImage: `url(${background})` }} />
    <Bottom>
      <Name level={3}>{name}</Name>
      <Role>{role}</Role>
    </Bottom>
  </StyledCastCard>
)
