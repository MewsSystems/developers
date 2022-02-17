
import { Routes, Route, BrowserRouter } from "react-router-dom";
import Header from "./components/Header";
import Footer from "./components/Footer";
import Detail from "./pages/Detail";
import Home from "./pages/Home";

function App() {

  return (
    <BrowserRouter>
      <Header />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="detail/:movieId" element={<Detail />} />
      </Routes>
      <Footer />
    </BrowserRouter>

  );
}

export default App;
