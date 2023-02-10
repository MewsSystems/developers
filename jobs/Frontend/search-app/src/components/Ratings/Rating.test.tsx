import React from "react";
import { render } from "@testing-library/react";
import { Rating } from "./Rating";

describe("Ratings component", () => {
  test("it should display passed values", () => {
    const { baseElement } = render(
      <Rating label="Title" isHighlighted={false} iconName="">
        8.5
      </Rating>
    );

    const title = baseElement.querySelector("[data-test='rating'] div");
    const span = baseElement.querySelector("span");
    expect(title?.textContent).toBe("Title");
    expect(span?.textContent).toBe("8.5");
  });

  test("it should add highlighted class on high popularity", () => {
    const { baseElement } = render(
      <Rating label="Title" isHighlighted={true} iconName="">
        {" "}
        8.5{" "}
      </Rating>
    );

    const highlighted = baseElement.querySelectorAll(".highlighted");
    expect(highlighted.length).toBe(1);
  });
});
