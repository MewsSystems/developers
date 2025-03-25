import { Outlet } from "react-router";

const App = () => (
  <div className="mx-4">
    <header className="w-full text-center py-8">
      <h1>Movie database</h1>
    </header>
    <main>
      <Outlet />
    </main>
  </div>
);

export default App;
