"use client"
import { Inter } from "next/font/google"

import "./globals.css"
import Providers from "@/app/providers"
import FrontendTracer from "@/FrontendTracer"

const inter = Inter({ subsets: ["latin"] })

if (typeof window !== "undefined") {
  FrontendTracer()
}

export default function RootLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <Providers>
      <html lang="en">
        <body className={inter.className}>
          <main className="mx-auto min-h-screen max-w-6xl space-y-8 p-4 lg:p-16">
            {children}
          </main>
        </body>
      </html>
    </Providers>
  )
}
