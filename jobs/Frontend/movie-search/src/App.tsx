import "./App.css";
import { LayoutWrapper } from "./components/Layout/LayoutWrapper";
import { Input } from "./components/Form/Input";
import { MainContent } from "./components/Layout/MainContent";
import { Header } from "./components/Header/Header";
import { Footer } from "./components/Footer/Footer";
import { Grid } from "./components/Grid/Grid";
import { GridCard } from "./components/Grid/GridCard";

function App() {
  return (
    <LayoutWrapper>
      <Header>
        <Input name="searchField" />
      </Header>
      <MainContent>
        <Grid>
          <GridCard>SomeContent</GridCard>
          <GridCard>SomeContent</GridCard>
          <GridCard>SomeContent</GridCard>
          <GridCard>SomeContent</GridCard>
          <GridCard>SomeContent</GridCard>
          <GridCard>SomeContent</GridCard>
          <GridCard>SomeContent</GridCard>
          <GridCard>SomeContent</GridCard>
        </Grid>
      </MainContent>
      <Footer>Created by Petar Zayakov</Footer>
    </LayoutWrapper>
  );
}

export default App;
