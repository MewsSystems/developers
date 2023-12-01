import { render, screen, waitFor } from "@testing-library/react";
import Paging from "../Paging";

describe("Paging", () => {
    test("renders the correct small number of pages", () => {
        render(<Paging current={1} total={5} />);

        const pages = screen.getAllByRole("link");
        expect(pages).toHaveLength(5);
    });

    test("renders the correct large number of pages", () => {
        render(<Paging current={2} total={8} />);

        const pages = screen.getAllByRole("link");
        expect(pages).toHaveLength(4);
    });

    test("renders the current page as active", () => {
        render(<Paging current={3} total={5} />);

        const activePage = screen.getByText("3");

        expect(activePage).toHaveAccessibleName("Selected page");
    });
});
