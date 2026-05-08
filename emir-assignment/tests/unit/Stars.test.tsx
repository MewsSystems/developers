import { render, screen } from "@testing-library/react";
import Stars from "../../src/components/Stars";
import { describe, it, expect } from "vitest";

describe("Stars", () => {
    it("renders 5 full stars for a rating of 10", () => {
        render(<Stars value={10} />);
        expect(screen.getAllByTestId("full-star")).toHaveLength(5);
        expect(screen.queryByTestId("half-star")).toBeNull();
        expect(screen.queryAllByTestId("empty-star")).toHaveLength(0);
    });

    it("renders 3 full stars and 1 half star for a rating of 7", () => {
        render(<Stars value={7} />);
        expect(screen.getAllByTestId("full-star")).toHaveLength(3);
        expect(screen.getByTestId("half-star")).toBeDefined();
        expect(screen.getAllByTestId("empty-star")).toHaveLength(1);
    });

    it("renders 2 full stars for a rating of 4", () => {
        render(<Stars value={4} />);
        expect(screen.getAllByTestId("full-star")).toHaveLength(2);
        expect(screen.queryByTestId("half-star")).toBeNull();
        expect(screen.getAllByTestId("empty-star")).toHaveLength(3);
    });

    it("renders 0 stars for 0 rating", () => {
        render(<Stars value={0} />);
        expect(screen.queryAllByTestId("full-star")).toHaveLength(0);
        expect(screen.queryByTestId("half-star")).toBeNull();
        expect(screen.getAllByTestId("empty-star")).toHaveLength(5);
    });

    it("does not exceed 5 stars even for a value above 10", () => {
        render(<Stars value={15} />);
        expect(screen.getAllByTestId("full-star")).toHaveLength(5);
        expect(screen.queryByTestId("half-star")).toBeNull();
        expect(screen.queryAllByTestId("empty-star")).toHaveLength(0);
    });
});
