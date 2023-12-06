import { render, screen } from "@testing-library/react"
import { Provider } from "react-redux"
import { BrowserRouter } from "react-router-dom"
import { store } from "@/app/store"
import Root from "./Root"

test("navigation bar is shown", () => {
  render(
    <Provider store={store}>
      <BrowserRouter>
        <Root />
      </BrowserRouter>
    </Provider>,
  )

  expect(screen.getByText(/movies app/i)).toBeInTheDocument()
  expect(screen.getByRole("link", { name: /movies app/i })).toHaveAttribute(
    "href",
    "/",
  )
})
