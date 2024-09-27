import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "Welcome to Movie Lab",
  description: "Search and find your favorite movies.",
};

export default function RootLayout({
                                     children,
                                   }: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
    <head>
      <title>{metadata.title as string}</title>
      <meta name="description" content={metadata.description as string} />
      <link
        href={`https://fonts.googleapis.com/css2?family=Inter:wght@400;700&display=swap`}
        rel="stylesheet"
      />
    </head>
    <body className={inter.className}>{children}</body>
    </html>
  );
}
