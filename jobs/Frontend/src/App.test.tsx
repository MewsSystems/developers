import App from "./App";
import { render, screen } from "./test/utils";

describe("App", () => {
  it("app renders and the main input is visible", () => {
    render(<App />);
    expect(
      screen.getByPlaceholderText(/Search for a movie/i)
    ).toBeInTheDocument();
  });
});
