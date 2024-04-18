import { render, screen } from "@testing-library/react";
import App from "../App";

describe("App", () => {
  function Arrange() {
    render(<App />);
  }
  it("renders the headline", async () => {
    Arrange();
    const headline = await screen.findByText("Mews Movie Search App");
    expect(headline).toBeDefined();
  });
  it("renders the input search", async () => {
    Arrange();
    const input = await screen.findByLabelText("Search a movie");
    expect(input).toBeDefined();
  });
});
