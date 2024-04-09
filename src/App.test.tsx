import { render, screen } from "@testing-library/react";
import App from "./App";

test("renders David Portilla link", () => {
    render(<App />);
    const linkElement = screen.getByText(/David Portilla/i);
    expect(linkElement).toBeInTheDocument();
});
