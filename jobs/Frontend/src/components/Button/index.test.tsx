import { render, screen, fireEvent } from "@testing-library/react"
import "@testing-library/jest-dom"
import Button from "./index"

describe("Button component", () => {
  test("renders with default props", () => {
    render(<Button>Click Me</Button>)
    const buttonElement = screen.getByRole("button", { name: /click me/i })
    expect(buttonElement).toBeInTheDocument()
    expect(buttonElement).toHaveClass(
      "flex items-center rounded bg-primary px-3 py-2 text-white hover:bg-sky-600",
    )
  })

  test("renders with custom className", () => {
    render(<Button className="custom-class">Click Me</Button>)
    const buttonElement = screen.getByRole("button", { name: /click me/i })
    expect(buttonElement).toHaveClass("custom-class")
  })

  test("renders with icon", () => {
    render(<Button icon={<span data-testid="icon">icon</span>}>Click Me</Button>)
    const iconElement = screen.getByTestId("icon")
    expect(iconElement).toBeInTheDocument()
    expect(iconElement).toHaveTextContent("icon")
  })

  test("renders with icon only", () => {
    render(<Button icon={<span data-testid="icon">icon</span>} />)
    const iconElement = screen.getByTestId("icon")
    expect(iconElement).toBeInTheDocument()
    expect(iconElement).toHaveTextContent("icon")
  })

  test("handles click event", () => {
    const handleClick = jest.fn()
    render(<Button onClick={handleClick}>Click Me</Button>)
    const buttonElement = screen.getByRole("button", { name: /click me/i })
    fireEvent.click(buttonElement)
    expect(handleClick).toHaveBeenCalledTimes(1)
  })

  test("renders with correct type", () => {
    render(<Button type="submit">Submit</Button>)
    const buttonElement = screen.getByRole("button", { name: /submit/i })
    expect(buttonElement).toHaveAttribute("type", "submit")
  })
})
