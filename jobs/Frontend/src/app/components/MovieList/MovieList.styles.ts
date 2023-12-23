import styled from 'styled-components'

export const Container = styled.div`
  display: grid;
  grid-template-columns: repeat(
    auto-fit,
    minmax(min(100%/1, max(396px, 100%/5)), 1fr)
  );
  justify-content: center;
  align-items: flex-end;
  user-select: none;
  gap: 64px;

  @media (max-width: 1050px) {
    grid-template-columns: auto;
    gap: 48px;
  }
`

export const PaginatorWrapper = styled.div`
  margin: 48px 0 96px;
`
