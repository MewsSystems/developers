import { render } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import App from "./App";

it("should render App", () => {
  render(
    <MemoryRouter>
      <App />
    </MemoryRouter>,
  );
});
