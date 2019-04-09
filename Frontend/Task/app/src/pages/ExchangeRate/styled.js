import styled from 'styled-components'

import { theme } from '../../../styles/theme'

export const List = styled.ul`
  margin: 0;
  padding: 0;
`

export const Aside = styled.div`
  width: 35rem;
`

export const Content = styled.div`
  margin-left: 3rem;
`

export const Wrapper = styled.div`
  display: flex;
  margin: 3rem;
`

export const Status = styled.div`
  margin-top: 2.5rem;
  color: ${theme.color.red};
  text-transform: uppercase;
`
