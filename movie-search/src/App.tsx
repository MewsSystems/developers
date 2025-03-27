import {BrowserRouter, Route, Routes} from "react-router-dom"
import {SearchView} from "./views/SearchView.tsx"
import {MovieDetailView} from "./views/MovieDetailView.tsx"

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<SearchView/>}/>
                <Route path="/movie/:id" element={<MovieDetailView/>}/>
            </Routes>
        </BrowserRouter>
    )
}

export default App
