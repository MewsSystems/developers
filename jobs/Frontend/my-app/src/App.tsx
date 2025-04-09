import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import SearchView from './views/SearchView';
import DetailsView from './views/DetailsView';

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<SearchView />} />
        <Route path="/details/:id" element={<DetailsView />} />
      </Routes>
    </Router>
  );
}

export default App;
