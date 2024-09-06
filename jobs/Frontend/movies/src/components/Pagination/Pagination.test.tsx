import { render } from "@testing-library/react";
import { expect, test, vi } from "vitest";
import Pagination from "./Pagination";
import userEvent from "@testing-library/user-event";
import React from "react";

test("Pagination renders correctly and buttons are functional", async () => {
  const onNextPage = vi.fn();
  const onPrevPage = vi.fn();

  // Render the Pagination component
  const { getByText, queryByText } = render(
    <Pagination
      currentPage={2}
      totalPages={5}
      onNextPage={onNextPage}
      onPrevPage={onPrevPage}
    />
  );

  // Check if the pagination controls and page indicator are present
  expect(queryByText(/Page 2 of 5/i)).toBeTruthy();
  expect(queryByText(/</i)).toBeTruthy();
  expect(queryByText(/>/i)).toBeTruthy();

  // Simulate clicking the next page button
  await userEvent.click(getByText(/>/i));
  expect(onNextPage).toHaveBeenCalled();

  // Simulate clicking the previous page button
  await userEvent.click(getByText(/</i));
  expect(onPrevPage).toHaveBeenCalled();
});
