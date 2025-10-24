import "./globals.css";
import type { Metadata } from "next";

export const metadata: Metadata = {
  title: "FilmFinder",
  description: "Movie search app for the Mews assessment"
};

export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en-GB" className="dark">
      <body>
        <div className="mx-auto max-w-6xl px-4 py-6 md:px-8">
          <header className="mb-6 flex items-center justify-between">
            <h1 className="text-xl font-semibold tracking-tight">
              <span className="opacity-70">Film</span><span className="text-accent">Finder</span>
            </h1>
          </header>
          {children}
        </div>
      </body>
    </html>
  );
}