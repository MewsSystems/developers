import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";
import RootProviders from "@/components/RootProviders";

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
    title: "Movie app",
    description: "Frontend job task for Mews",
};

export default function RootLayout({
    children,
}: {
    children: React.ReactNode;
}) {
    return (
        <html lang="en">
            <body className={inter.className}>
                <RootProviders>{children}</RootProviders>
            </body>
        </html>
    );
}
