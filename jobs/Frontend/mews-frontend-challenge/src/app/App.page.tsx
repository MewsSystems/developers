import { useMemo } from "react";

import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

import { MOVIE_DETAIL_ROUTE, SEARCH_ROUTE } from "./AppRouter.utils";
import { MovieDetail } from "./pages/MovieDetail/MovieDetail.page";
import { Search } from "./pages/Search/Search.page";
import { Route, Switch, BrowserRouter as Router } from "react-router-dom";
import { AppLayout } from "./AppLayout";

function App() {
  const queryClient = useMemo(() => new QueryClient(), []);

  return (
    <Router>
      <QueryClientProvider client={queryClient}>
        <AppLayout>
          <Switch>
            <Route exact path={SEARCH_ROUTE} component={Search} />
            <Route path={MOVIE_DETAIL_ROUTE} component={MovieDetail} />
            {/* Default route in a switch */}
            <Route>404: No such page!</Route>
          </Switch>
        </AppLayout>
      </QueryClientProvider>
    </Router>
  );
}

export default App;
