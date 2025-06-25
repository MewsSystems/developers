import { render, screen } from "@testing-library/react"
import { ThemeProvider } from "styled-components"
import { describe, expect, it } from "vitest"
import { theme } from "@/styles/theme"
import { ErrorMessage } from "./ErrorMessage"

const renderErrorMessage = (message: string) => {
  return render(
    <ThemeProvider theme={theme}>
      <ErrorMessage message={message} />
    </ThemeProvider>
  )
}

describe("ErrorMessage", () => {
  it("renders error message correctly", () => {
    const message = "Something went wrong"
    renderErrorMessage(message)

    expect(screen.getByText(message)).toBeInTheDocument()
  })

  it("renders error icon", () => {
    const { container } = renderErrorMessage("Test error")

    const errorIcon = container.querySelector("svg.lucide-triangle-alert")
    expect(errorIcon).toBeInTheDocument()
  })
})
