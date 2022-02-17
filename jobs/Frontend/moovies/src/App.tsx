
import { Routes, Route, BrowserRouter } from "react-router-dom";
import { useEffect, useState } from "react";
import Header from "./components/Header";
import Footer from "./components/Footer";
import Detail from "./pages/Detail";
import Home from "./pages/Home";

function App() {

  const [results, setResults] = useState([])

  useEffect(() => {
    // console.log(results)
  })

  return (
    <BrowserRouter>
      <Header />
      <Routes>
        <Route path="/" element={<Home results={results} setResults={setResults} />} />
        <Route path="detail/:movieId" element={<Detail />} />
        <Route path="*" element={<Home results={results} setResults={setResults} />} />
      </Routes>
      <Footer />
    </BrowserRouter>

  );
}

export default App;
