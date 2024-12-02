import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'

const AppQueryProvider = ({ children }: React.PropsWithChildren<unknown>) => {
    const client = new QueryClient({})

    return (
        <QueryClientProvider client={client}>
            {children}
            <ReactQueryDevtools />
        </QueryClientProvider>
    )
}

export default AppQueryProvider
