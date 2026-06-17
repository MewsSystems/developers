import { render } from "@/tests";
import { Rating } from "./Rating";

describe("Rating", () => {
  it("should render correct icons", () => {
    const { queryAllByTestId } = render(<Rating value={3.5} />);

    expect(queryAllByTestId("filled-star-icon")).toHaveLength(3);
    expect(queryAllByTestId("half-star-icon")).toHaveLength(1);
    expect(queryAllByTestId("empty-star-icon")).toHaveLength(1);
  });
});
