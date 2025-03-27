import {QueryClient, QueryClientProvider} from "@tanstack/react-query"
import {BrowserRouter, Route, Routes} from "react-router-dom"
import {SearchView} from "./views/SearchView.tsx"
import {MovieDetailView} from "./views/MovieDetailView.tsx"
import {GlobalStyles} from "./styles/GlobalStyles.tsx";

function App() {
    const queryClient = new QueryClient()

    return (
        <>
            <GlobalStyles/>
            <QueryClientProvider client={queryClient}>
                <BrowserRouter>
                    <Routes>
                        <Route path="/" element={<SearchView/>}/>
                        <Route path="/movie/:id" element={<MovieDetailView/>}/>
                    </Routes>
                </BrowserRouter>
            </QueryClientProvider>
        </>
    )
}

export default App
