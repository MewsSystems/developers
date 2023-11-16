import './App.css'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { MoviesSearch } from './containers/MoviesSearch'

function App() {
  const queryClient = new QueryClient() // Could add configs here if needed
  return (
    <>
      <QueryClientProvider client={queryClient}>
        <MoviesSearch/>
      </QueryClientProvider>
    </>
  )
}

export default App
