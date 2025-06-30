import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { Route, Routes } from "react-router";
import { HomePage } from "./pages/HomePage";
import { DetailPage } from "./pages/DetailPage";
import { LayoutWrapper } from "./components/Layout/LayoutWrapper";
import { Footer } from "./components/Footer/Footer";
import { GlobalStyles } from "./GlobalStyles";

const queryClient = new QueryClient();
function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <GlobalStyles />
      <LayoutWrapper>
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/movie/:id" element={<DetailPage />} />
        </Routes>
        <Footer>Created by Petar Zayakov</Footer>
      </LayoutWrapper>
    </QueryClientProvider>
  );
}

export default App;
