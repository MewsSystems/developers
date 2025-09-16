import { ChakraProvider, defaultSystem } from "@chakra-ui/react"
import { ThemeProvider } from "next-themes"
import {
    QueryClient,
    QueryClientProvider,
} from '@tanstack/react-query'
import { AuthProvider } from "@/entities/auth/api/providers/AuthProvider"

const queryClient = new QueryClient()

export function Provider({ children }: React.PropsWithChildren) {
    return <ChakraProvider value={defaultSystem}>
            <ThemeProvider attribute="class" disableTransitionOnChange enableSystem={false}>
                <QueryClientProvider client={queryClient}>
                    <AuthProvider>
                        {children}
                    </AuthProvider>
                </QueryClientProvider>
            </ThemeProvider>
    </ChakraProvider>

}