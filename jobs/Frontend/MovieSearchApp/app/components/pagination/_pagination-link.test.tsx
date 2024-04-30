/**
 * @vitest-environment jsdom
 */

import "@testing-library/jest-dom/vitest"
import { describe, test, expect } from "vitest"
import { render, screen } from "@testing-library/react"
import { PaginationLink } from "./_pagination-link"
import { BrowserRouter } from "react-router-dom"

describe("PaginationLink", () => {
  test("renders active link", async () => {
    await render(
      <PaginationLink to="/?page=2" disabled={false}>
        Next
      </PaginationLink>,
      { wrapper: BrowserRouter }
    )
    const link = screen.getByTestId("active-link")
    expect(link).toBeInTheDocument()
  })

  test("renders disabled link", async () => {
    await render(
      <PaginationLink to="/?page=2" disabled={true}>
        Next
      </PaginationLink>,
      { wrapper: BrowserRouter }
    )
    const link = screen.getByTestId("disabled-link")
    expect(link).toBeInTheDocument()
  })
})
