import { render } from "@/tests";
import { Typography } from "./Typography";
import { baseTheme } from "@/theme/base-theme";

describe("Typography", () => {
  it("should render the correct text", () => {
    const { getByText } = render(<Typography variant="bodyLarge">Hello</Typography>);

    expect(getByText("Hello")).toBeInTheDocument();
  });

  it("should render the correct variant", () => {
    const { getByText } = render(<Typography variant="bodyLarge">Hello</Typography>);

    const typographyElem = getByText("Hello");

    expect(typographyElem).toHaveStyle(`font-size: ${baseTheme.fonts.bodyLarge.fontSize}`);
  });

  it("should render the correct HTML element", () => {
    const { getByText } = render(
      <>
        <Typography variant="bodyLarge">p</Typography>
        <Typography variant="bodyLarge" element="h1">
          h1
        </Typography>
      </>,
    );

    const paragraphElem = getByText("p"); // p should render by default
    const headingElem = getByText("h1");

    expect(paragraphElem.tagName).toBe("P");
    expect(headingElem.tagName).toBe("H1");
  });

  it("should render bold text when the bold prop is set to true", () => {
    const { getByText } = render(
      <Typography variant="bodyLarge" bold>
        Hello
      </Typography>,
    );

    const typographyElem = getByText("Hello");

    expect(typographyElem).toHaveStyle(`font-weight: bold`);
  });
});
