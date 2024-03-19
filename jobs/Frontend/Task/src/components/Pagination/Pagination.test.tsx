import React from "react";
import { render, screen } from "@testing-library/react";
import { userEvent } from "@testing-library/user-event";
import { Pagination } from "./Pagination";

describe("<Pagination />", () => {
  it("should display the pagination component if count is higher than 1", () => {
    render(
      <Pagination
        count={2}
        onChange={jest.fn()}
        page={1}
      />
    );
  
    expect(screen.getByRole("navigation", {  
      name: /pagination navigation/i
    })).toBeInTheDocument();
  });

  it("should not display the pagination component if count equals 1", () => {
    render(
      <Pagination
        count={1}
        onChange={jest.fn()}
        page={1}
      />
    );
  
    expect(screen.queryByRole("navigation", {  
      name: /pagination navigation/i
    })).not.toBeInTheDocument();
  });

  it("should not display the pagination component if count is below 0", () => {
    render(
      <Pagination
        count={0}
        onChange={jest.fn()}
        page={0}
      />
    );
  
    expect(screen.queryByRole("navigation", {  
      name: /pagination navigation/i
    })).not.toBeInTheDocument();
  });

  it("should not display the pagination component if count is undefined", () => {
    render(
      <Pagination
        count={undefined}
        onChange={jest.fn()}
        page={undefined}
      />
    );
  
    expect(screen.queryByRole("navigation", {  
      name: /pagination navigation/i
    })).not.toBeInTheDocument();
  });

  it("should call the onChange callback with the correct argument", async () => {
    const mockOnChange = jest.fn();

    render(
      <Pagination
        count={3}
        onChange={mockOnChange}
        page={1}
      />
    );

    await userEvent.click(screen.getByRole("button", {  name: /go to page 2/i}));
  
    expect(mockOnChange).toHaveBeenCalledWith(2);
  });
});
