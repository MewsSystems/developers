import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { SearchView } from './pages/search-view';
import { DetailsView } from './pages/details-view';

const App = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<SearchView />} />
        <Route path="/movie/:id" element={<DetailsView />} />
      </Routes>
    </Router>
  );
};

export default App;