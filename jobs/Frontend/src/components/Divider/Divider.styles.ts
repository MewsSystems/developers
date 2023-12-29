import { theme } from '@/theme'
import styled from 'styled-components'

export const StyledDivider = styled.hr`
  display: block;
  height: 1px;
  background-color: ${theme.colors.border.secondary};
  border: 0;
  border-top: 1px solid ${theme.colors.border.secondary};
`
