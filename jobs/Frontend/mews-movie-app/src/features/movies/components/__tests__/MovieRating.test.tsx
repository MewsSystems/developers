import { render, screen } from "@testing-library/react";
import MovieRating from "../MovieRating";

describe("MovieRating", () => {
    it("renders the rating correctly rounded to 1 decimal place", () => {
        const rating = 4.5576;
        render(<MovieRating rating={rating} />);
        const ratingElement = screen.getByText("4.6");

        expect(ratingElement).toBeInTheDocument();
    });

    it("renders '?' when rating is not provided", () => {
        render(<MovieRating />);
        const element = screen.getByText("?");

        expect(element).toBeInTheDocument();
    });
});
