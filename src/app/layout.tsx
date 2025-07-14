import './globals.css';
import type { Metadata } from 'next';
import { Roboto } from 'next/font/google';
import { ReactQueryProvider } from '@/providers/ReactQueryProvider';
import { BsCameraReelsFill } from 'react-icons/bs';

const roboto = Roboto({
  weight: ['200', '400', '700'],
  subsets: ['latin-ext'],
  display: 'swap',
});

export const metadata: Metadata = {
  description: 'Discover movies with powerful search and detailed views.',
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en" className={`overflow-y-scroll ${roboto.className}`}>
      <body className="bg-cyan-950 text-stone-900">
        <ReactQueryProvider>
          <div className="flex min-h-screen items-stretch justify-center bg-cyan-950">
            <div className="flex w-full max-w-[48rem] flex-col bg-main [&:has(.requires-wide-layout)]:md:max-w-[64rem]">
              <header className="h-20 flex items-center justify-start bg-cyan-50 text-cyan-950 text-3xl font-bold px-6 pt-2 mt-4 rounded-t-xl">
                <BsCameraReelsFill className="-translate-y-2 mr-3" size="40px" aria-hidden />{' '}
                MovieSearch
              </header>
              <main className="bg-white flex-1 p-2 flex flex-col sm:p-6">{children}</main>
            </div>
          </div>
        </ReactQueryProvider>
      </body>
    </html>
  );
}
