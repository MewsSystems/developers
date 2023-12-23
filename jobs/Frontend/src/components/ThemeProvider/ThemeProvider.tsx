'use client'

import { ReactNode } from 'react'
import { ThemeProvider as StyledThemeProvider } from 'styled-components'

import { theme } from '@/theme'

type Props = { children: ReactNode }

export const ThemeProvider = ({ children }: Props) => (
  <StyledThemeProvider theme={theme}>{children}</StyledThemeProvider>
)
