import { describe, it, expect } from "vitest";

import { render, screen } from "@/testing-utils/render.utils";
import { SearchPagination } from "../SearchPagination";

describe("SearchPagination", () => {
  const exampleLinkBuilder = (page: number) => `/?page=${page}`;

  it("renders pagination links correctly", () => {
    const availablePages = [1, 2, 3];

    render(
      <SearchPagination
        currentPage={1}
        totalPages={3}
        linkBuilder={exampleLinkBuilder}
      />,
    );

    availablePages.forEach((page) =>
      expect(
        screen.getByRole("link", { name: page.toString() }),
      ).toHaveAttribute("href", `/?page=${page}`),
    );
  });

  it("shows previous button on if previous pages are available", () => {
    const currentPage = 2;
    render(
      <SearchPagination
        currentPage={2}
        totalPages={3}
        linkBuilder={exampleLinkBuilder}
      />,
    );

    // Assert that the previous button is disabled on the first page
    const previousButton = screen.getByRole("link", { name: /Previous/i });
    expect(previousButton).toHaveAttribute("href", `/?page=${currentPage - 1}`);
  });

  it("hides previous button on first page", () => {
    render(
      <SearchPagination
        currentPage={1}
        totalPages={3}
        linkBuilder={exampleLinkBuilder}
      />,
    );

    // Assert that the previous button is disabled on the first page
    expect(
      screen.queryByRole("link", { name: /Previous/i }),
    ).not.toBeInTheDocument();
  });

  it("renders next page button if are new pages available", () => {
    const currentPage = 1;
    render(
      <SearchPagination
        currentPage={currentPage}
        totalPages={3}
        linkBuilder={exampleLinkBuilder}
      />,
    );

    // Assert that the next button is disabled on the last page
    const nextButton = screen.getByRole("link", { name: /Next/i });
    expect(nextButton).toHaveAttribute("href", `/?page=${currentPage + 1}`);
  });

  it("hides next button on last page", () => {
    render(
      <SearchPagination
        currentPage={3}
        totalPages={3}
        linkBuilder={exampleLinkBuilder}
      />,
    );

    // Assert that the next button is disabled on the last page
    expect(
      screen.queryByRole("link", { name: /Next/i }),
    ).not.toBeInTheDocument();
  });

  it("renders ellipsis when there are more than 3 pages to navigate further", () => {
    const currentPage = 1;
    render(
      <SearchPagination
        currentPage={currentPage}
        totalPages={10}
        linkBuilder={exampleLinkBuilder}
      />,
    );

    expect(
      screen.getByTestId("pagination-further-ellipsis"),
    ).toBeInTheDocument();
  });

  it("renders ellipsis when there are more than 3 pages to navigate backwards", () => {
    const currentPage = 10;
    render(
      <SearchPagination
        currentPage={currentPage}
        totalPages={10}
        linkBuilder={exampleLinkBuilder}
      />,
    );

    expect(
      screen.getByTestId("pagination-backwards-ellipsis"),
    ).toBeInTheDocument();
  });
});
