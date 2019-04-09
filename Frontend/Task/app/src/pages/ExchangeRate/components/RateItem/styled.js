import styled from 'styled-components'
import { theme } from '../../../../../styles/theme'

export const Pair = styled.div`
  flex: 1;
  font-weight: bold;
`

export const Wrapper = styled.div`
  display: flex;
  flex-grow: 1;
  color: ${theme.color.darkGray};
`

export const Rate = styled.div`
  flex: 1;
  text-align: center;
`

export const Trend = styled.div`
  flex: 1;
  text-align: right;

  &.grow {
    color: ${theme.color.green};
  }

  &.decline {
    color: ${theme.color.red};
  }
`

export const Item = styled.li`
  background: ${theme.color.gray};
  width: 400px;
  margin-bottom: 0.8rem;
  padding: 2rem;
  border-radius: 0.5rem;
  list-style: none;
`
