import { MoviesProvider } from '@/provider/MoviesProvider'
import { QueryProvider } from '@/provider/QueryProvider'
import type { Metadata } from 'next'
import { Geist, Geist_Mono } from 'next/font/google'
import './globals.css'

const geistSans = Geist({
    variable: '--font-geist-sans',
    subsets: ['latin'],
})

const geistMono = Geist_Mono({
    variable: '--font-geist-mono',
    subsets: ['latin'],
})

export const metadata: Metadata = {
    title: 'Movie Search',
    description: 'Search for movies',
}

export default function RootLayout({
    children,
}: Readonly<{
    children: React.ReactNode
}>) {
    return (
        <html lang="en">
            <body
                className={`${geistSans.variable} ${geistMono.variable} bg-background antialiased`}
            >
                <QueryProvider>
                    <MoviesProvider>
                        <div className="flex h-screen flex-col px-4 py-8 md:px-16 md:py-16">
                            {children}
                        </div>
                    </MoviesProvider>
                </QueryProvider>
            </body>
        </html>
    )
}
