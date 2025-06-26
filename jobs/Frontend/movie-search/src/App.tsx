import "./App.css";
import { LayoutWrapper } from "./components/Button/Layout/LayoutWrapper";
import { Input } from "./components/Form/Input";
import { MainContent } from "./components/Button/Layout/MainContent";
import { Header } from "./components/Button/Header/Header";
import { Footer } from "./components/Footer/Footer";

function App() {
  return (
    <LayoutWrapper>
      <Header>
        <Input name="searchField" />
      </Header>
      <MainContent>Some content</MainContent>
      <Footer>Created by Petar Zayakov</Footer>
    </LayoutWrapper>
  );
}

export default App;
