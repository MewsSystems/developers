'use client'

import cacheConfig from '@/const/cache'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import React from 'react'

export const QueryProvider = ({ children }: { children: React.ReactNode }) => {
    const queryClient = new QueryClient({
        defaultOptions: cacheConfig.DEFAULT_QUERY_OPTIONS,
    })

    return (
        <QueryClientProvider client={queryClient}>
            {children}
        </QueryClientProvider>
    )
}
