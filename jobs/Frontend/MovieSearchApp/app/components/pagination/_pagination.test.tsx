/**
 * @vitest-environment jsdom
 */

import "@testing-library/jest-dom/vitest"
import { describe, test, expect } from "vitest"
import { render, screen, cleanup } from "@testing-library/react"
import { Pagination } from "./_pagination"
import { createMemoryRouter, RouterProvider } from "react-router-dom"

describe("Pagination", () => {
  test("renders links and input disabled when total pages is 1", async () => {
    const router = createMemoryRouter(
      [
        {
          path: "/",
          element: <Pagination totalPages={1} />,
        },
      ],
      {
        initialEntries: ["/?page=1"],
        initialIndex: 1,
      }
    )

    await render(<RouterProvider router={router} />)

    const links = screen.getAllByTestId("disabled-link")
    const previousLink = links[0]
    const nextLink = links[1]
    const input = screen.getByTestId("page-input")
    expect(previousLink).toBeInTheDocument()
    expect(previousLink).toHaveTextContent("Previous")
    expect(nextLink).toBeInTheDocument()
    expect(nextLink).toHaveTextContent("Next")
    expect(input).toBeDisabled()

    cleanup()
  })

  test("renders previous link disabled when current page is 1 and total pages is grater then one", async () => {
    const router = createMemoryRouter(
      [
        {
          path: "/",
          element: <Pagination totalPages={3} />,
        },
      ],
      {
        initialEntries: ["/?page=1"],
        initialIndex: 1,
      }
    )

    await render(<RouterProvider router={router} />)

    const previousLink = screen.getByTestId("disabled-link")
    expect(previousLink).toBeInTheDocument()
    expect(previousLink).toHaveTextContent("Previous")
    const nextLink = screen.getByTestId("active-link")
    expect(nextLink).toBeInTheDocument()
    expect(nextLink).toHaveTextContent("Next")

    cleanup()
  })

  test("renders next link disabled when current page is equal to total pages", async () => {
    const router = createMemoryRouter(
      [
        {
          path: "/",
          element: <Pagination totalPages={3} />,
        },
      ],
      {
        initialEntries: ["/?page=3"],
        initialIndex: 1,
      }
    )

    await render(<RouterProvider router={router} />)

    const previousLink = screen.getByTestId("active-link")
    expect(previousLink).toBeInTheDocument()
    expect(previousLink).toHaveTextContent("Previous")
    const nextLink = screen.getByTestId("disabled-link")
    expect(nextLink).toBeInTheDocument()
    expect(nextLink).toHaveTextContent("Next")

    cleanup()
  })

  test("input value to be equal to current page", async () => {
    const router = createMemoryRouter(
      [
        {
          path: "/",
          element: <Pagination totalPages={3} />,
        },
      ],
      {
        initialEntries: ["/?page=2"],
        initialIndex: 1,
      }
    )

    await render(<RouterProvider router={router} />)

    const input = screen.getByTestId("page-input")
    expect(input).toHaveValue(2)

    cleanup()
  })
})
