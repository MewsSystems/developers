import "./App.css";
import { LayoutWrapper } from "./components/Layout/LayoutWrapper";

import { Footer } from "./components/Footer/Footer";
import { HomePage } from "./pages/HomePage";

function App() {
  return (
    <LayoutWrapper>
      <HomePage />
      <Footer>Created by Petar Zayakov</Footer>
    </LayoutWrapper>
  );
}

export default App;
