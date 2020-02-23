import React from 'react'
import styled from 'styled-components'
import { Heading } from 'components/Heading'

const StyledErrorConetnt = styled.div`
  margin: 3rem 0 3rem;
  text-align: center;
`

export interface ErrorContentProps {
  title: string
  text: string
}

export const ErrorContent: React.FC<ErrorContentProps> = ({ title, text }) => (
  <StyledErrorConetnt>
    <Heading level={1}>{title}</Heading>
    <p>{text}</p>
  </StyledErrorConetnt>
)
