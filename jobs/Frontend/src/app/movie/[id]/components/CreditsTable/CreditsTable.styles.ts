import styled from 'styled-components'

export const StyledTable = styled.table`
  border-collapse: separate;
  border-spacing: 0 32px;
  border: none;

  @media (max-width: 768px) {
    border-spacing: 0 24px;
  }
`

export const StyledTd = styled.td`
  vertical-align: top;

  // To save space on mobile, display table as a single column
  @media (max-width: 1050px) {
    display: grid;
    grid-template-columns: 1fr;
  }
`
