import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "react-query";
import { ReactQueryDevtools } from "react-query/devtools";

import { Homepage } from "./pages/Homepage";
import { MovieDetails } from "./pages/MovieDetails";

import "./App.css";

const queryClient = new QueryClient();

function App() {
    return (
        <div className="App">
            <QueryClientProvider client={queryClient}>
                <Router>
                    <Routes>
                        <Route path="/" element={<Homepage />} />
                        <Route path="/movie/:id" element={<MovieDetails />} />
                    </Routes>
                </Router>
                {/* React Query Devtools for debugging */}
                <ReactQueryDevtools initialIsOpen={false} />
            </QueryClientProvider>
        </div>
    );
}

export default App;
