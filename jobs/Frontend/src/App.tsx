import { RouterProvider, createRouter } from '@tanstack/react-router'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'

import { routeTree } from './routeTree.gen'

const router = createRouter({
	routeTree
})

const queryClient = new QueryClient()

declare module '@tanstack/react-router' {
	interface Register {
		router: typeof router
	}
}

function App() {
	return (
		<QueryClientProvider client={queryClient}>
			<RouterProvider router={router} />
		</QueryClientProvider>
	)
}

export default App
