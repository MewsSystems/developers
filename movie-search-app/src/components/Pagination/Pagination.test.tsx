import { describe, it, expect, vi } from "vitest";
import { render, screen, fireEvent } from "@testing-library/react";
import { Pagination } from "./Pagination";
import { MemoryRouter } from "react-router-dom";

describe("Pagination component", () => {
  it("renders current page and total pages", () => {
    render(
      <MemoryRouter>
        <Pagination currentPage={2} totalPages={5} onPageChange={() => {}} />
      </MemoryRouter>
    );

    expect(screen.getByText(/Page 2 of 5/i)).toBeInTheDocument();
  });

  it("calls onPageChange with correct page when next button is clicked", () => {
    const onPageChange = vi.fn();

    render(
      <MemoryRouter>
        <Pagination
          currentPage={2}
          totalPages={5}
          onPageChange={onPageChange}
        />
      </MemoryRouter>
    );

    fireEvent.click(screen.getByText(/Next/i));
    expect(onPageChange).toHaveBeenCalledWith(3);
  });

  it("calls onPageChange with correct page when previous button is clicked", () => {
    const onPageChange = vi.fn();

    render(
      <MemoryRouter>
        <Pagination
          currentPage={3}
          totalPages={5}
          onPageChange={onPageChange}
        />
      </MemoryRouter>
    );

    fireEvent.click(screen.getByText(/Previous/i));
    expect(onPageChange).toHaveBeenCalledWith(2);
  });

  it("disables previous button on first page", () => {
    render(
      <MemoryRouter>
        <Pagination currentPage={1} totalPages={5} onPageChange={() => {}} />
      </MemoryRouter>
    );

    expect(screen.getByText(/Previous/i)).toBeDisabled();
  });

  it("disables next button on last page", () => {
    render(
      <MemoryRouter>
        <Pagination currentPage={5} totalPages={5} onPageChange={() => {}} />
      </MemoryRouter>
    );

    expect(screen.getByText(/Next/i)).toBeDisabled();
  });
});
