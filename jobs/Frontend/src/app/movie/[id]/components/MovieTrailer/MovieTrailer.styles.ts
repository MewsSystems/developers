import styled from 'styled-components'

export const StyledIframe = styled.iframe`
  max-width: 853px;
  aspect-ratio: 16 / 9;
  margin: 24px 0 72px;

  @media (max-width: 768px) {
    margin: 16px 0 32px;
  }
`

export const StyledTitle = styled.span`
  margin-top: 20px;

  @media (max-width: 768px) {
    margin-top: 8px;
  }
`
