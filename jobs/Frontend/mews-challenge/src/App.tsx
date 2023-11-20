import Box from './app/elements/Box';
import { FontSize, Spacing } from './app/types';
import Text from './app/elements/Text';
import SearchPage from './app/components/SearchPage/SearchPage';
import {
  BrowserRouter as Router,
  Routes,
  Route,
} from "react-router-dom";
import MovieDetailsPage from './app/components/MovieDetailsPage/MovieDetailsPage';
import GlobalStyle from './globalStyle';
import { StyleSheetManager } from 'styled-components';



function App() {
  const isMobile = window.innerWidth < 600;
  const width = 'calc('+(isMobile ? '100vw' : '50vw') + ' - 50px)';

  return (
    <StyleSheetManager shouldForwardProp={(prop)=>!['textAlign','inline'].includes(prop)}>
    <Box mx={Spacing.auto} width={width} style={{minWidth: '300px'}}>
      <GlobalStyle />
      <Router>
        <Routes>
          <Route path="/" element={<SearchPage />} />
          <Route path="/movie" element={<MovieDetailsPage />} />
        </Routes>
      </Router>
    </Box>
    </StyleSheetManager>
  );
}

export default App;
