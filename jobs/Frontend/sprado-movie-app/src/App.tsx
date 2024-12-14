import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { SearchPage } from "./pages/SearchPage";
import { DetailPage } from "./pages/DetailPage";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<SearchPage />} />
        <Route path="/detail" element={<DetailPage />} />
      </Routes>
    </Router>
  );
}

export default App;
