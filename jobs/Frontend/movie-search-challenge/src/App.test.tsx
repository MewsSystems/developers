import { screen } from "@testing-library/react"
import { renderWithProviders } from "./utils/test-utils"
import App from "./App"

describe("App component test", () => {
  it("should have correct initial render", () => {
    renderWithProviders(<App />)

    // The app should be rendered correctly
    expect(screen.getByPlaceholderText("Search")).toBeInTheDocument()
  })
})
