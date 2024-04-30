/**
 * @vitest-environment jsdom
 */

import "@testing-library/jest-dom/vitest"
import { describe, test, expect } from "vitest"
import { render, screen } from "@testing-library/react"
import { Poster } from "./_poster"

// Test Poster component with image srouce and without (undefined), use data-testid="poster-image" for image and data-testid="poster-placeholder" for placeholder
describe("Poster", () => {
  test("renders with image", async () => {
    await render(
      <Poster src="https://example.com/image.jpg" alt="Test image" />
    )
    const poster = screen.getByTestId("poster-image")
    expect(poster).toBeInTheDocument()
  })

  test("renders without image", async () => {
    await render(<Poster src={null} alt="Test image" />)
    const poster = screen.getByTestId("poster-placeholder")
    expect(poster).toBeInTheDocument()
  })
})
