import { BrowserRouter as Router, Route, Routes } from "react-router-dom"
import { QueryClient, QueryClientProvider } from "@tanstack/react-query"
import Layout from "@containers/Layout"
import HomePage from "@pages/Home"
import MoviePages from "@pages/Movie"
import NotFound from "@containers/NotFound"

const queryClient = new QueryClient()

export const App = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <Router>
        <Layout>
          <Routes>
            <Route path="/" Component={HomePage} />
            <Route path="/movies/*" Component={MoviePages} />
            <Route path="*" Component={NotFound} />
          </Routes>
        </Layout>
      </Router>
    </QueryClientProvider>
  )
}

export default App
