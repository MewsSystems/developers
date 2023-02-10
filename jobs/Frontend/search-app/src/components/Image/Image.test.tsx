import React from "react";
import { render } from "@testing-library/react";
import { Image } from "./Image";

describe("Image component", () => {
  test("it should render the placeholder image if src is missing", () => {
    const { baseElement } = render(<Image src={null} alt="Abc Image" />);

    const img = baseElement.querySelector("[src='placeholder.png']");
    expect(img).toBeInTheDocument();
  });

  test("it should render the image if src attribute is provided", () => {
    const { baseElement } = render(<Image src="abc.jpg" alt="Abc Image" />);

    const img = baseElement.querySelector("[src*='abc.jpg']");
    expect(img).toBeInTheDocument();
  });
});
