import React from "react";
import { render, screen } from "@testing-library/react";
import { Drawer } from "./Drawer";

describe("<Drawer />", () => {
  it("should display the children being passed", () => {
    render(
      <Drawer anchor='right' isOpen={true} onClose={jest.fn}>
        This is a test
      </Drawer>
    );
    
    expect(screen.getByText(/this is a test/i)).toBeInTheDocument();
  });
});
