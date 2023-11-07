import { RouterProvider, createBrowserRouter } from "react-router-dom";
import { Search, Details } from "./pages";

const router = createBrowserRouter([
  {
    path: "/",
    element: <Search />,
  },
  {
    path: "/details/:id",
    element: <Details />,
  },
]);

function App() {
  return <RouterProvider router={router} />;
}

export default App;
