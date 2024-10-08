"use client"

import { QueryClientProvider as QCP } from "@tanstack/react-query"

import { queryClient } from "@/app/lib/react-query"

function QueryClientProvider({ children }: React.PropsWithChildren) {
  return <QCP client={queryClient}>{children}</QCP>
}

export default QueryClientProvider
