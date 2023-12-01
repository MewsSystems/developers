"use client";

import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";
import { ApiProvider } from "@reduxjs/toolkit/query/react";
import { api } from "@/features/movies/api/api";
import StyledComponentsRegistry from "@/lib/registry";

const inter = Inter({ subsets: ["latin"] });

// export const metadata: Metadata = {
//     title: "Mews movie app",
//     description: "Task for Mews frontend job application",
// };

export default function RootLayout({
    children,
}: {
    children: React.ReactNode;
}) {
    return (
        <html lang="en">
            <body className={inter.className}>
                <ApiProvider api={api}>
                    <StyledComponentsRegistry>
                        {children}
                    </StyledComponentsRegistry>
                </ApiProvider>
            </body>
        </html>
    );
}
