'use client'

import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import "./globals.css";
import Link from 'next/link';
import Image from 'next/image';

const queryClient = new QueryClient();

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
        <Link href="/">
          <Image id="mewslifx-logo" src="/mewsflix.png" alt="Logo" width={200} height={50} className="logo" />
        </Link>
        <QueryClientProvider client={queryClient}>
          {children}
        </QueryClientProvider>
      </body>
    </html>
  );
}
