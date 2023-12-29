import styled from 'styled-components'

export const StyledLayout = styled.main`
  display: grid;
  max-width: 1920px;
  margin: 0 auto;
  padding: 6rem 6rem 0 6rem;
  grid-template-columns: minmax(360px, 600px) minmax(420px, 2fr);
  gap: 128px;

  @media (max-width: 1320px) {
    gap: 72px;
    padding: 5rem 5rem 0 5rem;
  }

  @media (max-width: 1050px) {
    grid-template-columns: 1fr;
    justify-items: center;
    gap: 0px;
  }

  @media (max-width: 768px) {
    padding: 2rem 2rem 0 2rem;
  }
`
