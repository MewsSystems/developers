import type { Metadata } from 'next'
import { Inter } from 'next/font/google'

import {
  StoreProvider,
  StyledComponentsRegistry,
  ThemeProvider,
} from '@/components'
import './globals.css'

const inter = Inter({
  subsets: ['latin'],
})

export const metadata: Metadata = {
  title: 'Movies',
  description: 'Mews frontend developer task',
}

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <html lang="en">
      <body className={inter.className}>
        <StoreProvider>
          <StyledComponentsRegistry>
            <ThemeProvider>{children}</ThemeProvider>
          </StyledComponentsRegistry>
        </StoreProvider>
      </body>
    </html>
  )
}
