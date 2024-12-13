import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'

import Header from './components/header/Header'
import Home from './pages/Home'
import MovieDetails from './pages/MovieDetails'
import Footer from './components/footer/Footer'
import NotFound from './pages/NotFound'

function App() {
  return (
    <>
      <Header />
      <Router>
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/movie/:id" element={<MovieDetails />} />
          <Route path="/*" element={<NotFound />} />
        </Routes>
      </Router>
      <Footer />
    </>
  )
}

export default App
