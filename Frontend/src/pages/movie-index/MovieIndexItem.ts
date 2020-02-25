import styled from '../../utils/styled'

export const TableWrapper = styled.div`
  position: relative;
  max-width: ${props => props.theme.widths.md};
  margin: 0 auto;
  min-height: 200px;
`

export const MovieLoading = styled.tr`
  td {
    height: 48px;
    text-align: center;
  }
`

export const MovieIndexDetail = styled.td`
  display: flex;
  flex-direction: row;
  align-items: center;
`

export const MovieIcon = styled('img')`
  width: 32px;
  height: 32px;
`

export const MovieIconPh = styled.div`
  width: 32px;
  height: 32px;
`

export const MovieName = styled('div')`
  flex: 1 1 auto;
  height: 100%;
  margin-left: 1rem;

  a {
    color: ${props => props.theme.colors.brand};
  }
`
