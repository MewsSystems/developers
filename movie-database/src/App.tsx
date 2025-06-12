import { Outlet } from "react-router";

const App = () => (
  <div className="mx-4 flex flex-col items-center h-dvh">
    <header className="w-full text-center py-8">
      <h1>Movie database</h1>
    </header>
    <main className="flex flex-col items-center w-full">
      <Outlet />
    </main>
  </div>
);

export default App;
