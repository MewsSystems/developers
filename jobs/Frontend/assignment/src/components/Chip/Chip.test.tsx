import { render } from "@/tests";
import { Chip } from "./Chip";

describe("Chip", () => {
  it("displays the correct label text", () => {
    const { getByText } = render(<Chip label="Chip label" />);

    expect(getByText("Chip label")).toBeInTheDocument();
  });

  it("displays the correct image when an image path is provided", () => {
    const { getByTestId } = render(<Chip label="Chip label" imagePath="image.png" />);

    const chipElem = getByTestId("chip-image");

    expect(chipElem).toHaveStyle("background-image: url(image.png)");
  });
});
