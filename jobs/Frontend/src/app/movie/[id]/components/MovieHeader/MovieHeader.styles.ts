import styled from 'styled-components'

export const DetailHeader = styled.div`
  display: grid;
  gap: 16px;
  grid-template-columns: 1fr auto;
  align-items: flex-start;
  margin: 72px 0 32px;

  @media (max-width: 768px) {
    margin: 32px 0;
  }
`
