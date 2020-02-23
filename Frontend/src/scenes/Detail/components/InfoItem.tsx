import React from 'react'
import styled from 'styled-components'

const StyledInfoItem = styled.div`
  font-size: 0.8rem;
`

export interface InfoItemProps {
  label: React.ReactNode | React.ReactNodeArray
  chilren?: React.ReactNode | React.ReactNodeArray
}

export const InfoItem: React.FC<InfoItemProps> = ({ label, children }) => (
  <StyledInfoItem>
    <strong>{label}</strong>
    <div>{children}</div>
  </StyledInfoItem>
)
