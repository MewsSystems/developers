import { LayoutWrapper } from "./components/Layout/LayoutWrapper";

import { Footer } from "./components/Footer/Footer";
import { HomePage } from "./pages/HomePage";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

function App() {
  const queryClient = new QueryClient();

  return (
    <QueryClientProvider client={queryClient}>
      <LayoutWrapper>
        <HomePage />
        <Footer>Created by Petar Zayakov</Footer>
      </LayoutWrapper>
    </QueryClientProvider>
  );
}

export default App;
