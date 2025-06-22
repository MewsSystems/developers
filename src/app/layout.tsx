import './globals.css';
import type { Metadata } from 'next';
import { Roboto } from 'next/font/google';

const roboto = Roboto({
  weight: ['200', '400', '700'],
  subsets: ['latin-ext'],
  display: 'swap',
});

export const metadata: Metadata = {
  title: 'Search for Movies',
  description: 'Discover movies with powerful search and detailed views.',
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en" className={`${roboto.className}`}>
      <body className="bg-cyan-950 text-stone-900">
        <div className="flex min-h-screen items-stretch justify-center bg-cyan-950">
          <div className="flex w-full max-w-[48rem] flex-col bg-main">
            <header className="h-20 flex items-center justify-start bg-cyan-200 text-cyan-900 text-2xl font-bold px-6">
              Search for Movies
            </header>
            <main className="bg-cyan-100 flex-1 p-6 flex flex-col">{children}</main>
          </div>
        </div>
      </body>
    </html>
  );
}
