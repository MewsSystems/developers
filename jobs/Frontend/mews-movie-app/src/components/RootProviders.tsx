"use client";

import { api } from "@/features/movies/api/api";
import StyledComponentsRegistry from "@/lib/registry";
import { ApiProvider } from "@reduxjs/toolkit/dist/query/react";

type Props = {
    children: React.ReactNode;
};

export default function RootProviders({ children }: Props) {
    return (
        <ApiProvider api={api}>
            <StyledComponentsRegistry>{children}</StyledComponentsRegistry>
        </ApiProvider>
    );
}
