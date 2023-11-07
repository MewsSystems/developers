import { render } from "@/tests";
import { Rating, getStarIcon } from "./Rating";

describe("Rating", () => {
  it("getStarIcon helper", () => {
    expect(getStarIcon(3.5, 4)).toContain("half-filled-star.svg");
    expect(getStarIcon(3.5, 3)).toContain("filled-star.svg");
    expect(getStarIcon(3.5, 5)).toContain("empty-star.svg");
    expect(getStarIcon(3.2, 4)).toContain("empty-star.svg");
    expect(getStarIcon(-2, 3)).toContain("empty-star.svg");
  });

  it("should render correct icons", () => {
    const { queryAllByTestId } = render(<Rating value={3.5} />);

    const starElems = queryAllByTestId("star-icon");

    starElems.forEach((starElem, index) => {
      const { backgroundImage } = window.getComputedStyle(starElem);
      expect(backgroundImage).toContain(getStarIcon(3.5, index + 1));
    });
  });
});
