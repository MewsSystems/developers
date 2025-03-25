import React from 'react';
import ReactDOM from "react-dom/client";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import './index.css'
import { App } from './App.jsx'
import { MovieDetailPage } from './MovieDetailPage/MovieDetailPage.jsx';

const root = document.getElementById("root");

ReactDOM.createRoot(root).render(
  <React.StrictMode>
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<App />} />
        <Route path={`/:title`} element={<MovieDetailPage/>}/>
      </Routes>
    </BrowserRouter>
  </React.StrictMode>
);