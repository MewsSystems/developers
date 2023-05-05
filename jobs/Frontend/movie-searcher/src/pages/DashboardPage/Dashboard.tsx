import { Divider } from "antd";
import { MovieSearchInput } from "./MovieSearchInput";
import { MoviesList } from "./MoviesList";

const DashboardPage = () => (
  <div>
    <MovieSearchInput />
    <Divider />
    <MoviesList />
  </div>
);

export { DashboardPage };
