import { render } from "@testing-library/react";
import { describe, expect, it } from "vitest";

import MovieDetailInformation from "./MovieDetailInformation";

describe("MovieDetailInformation", () => {
  it("renders MovieDetailInformation component with provided props", () => {
    const props = {
      genres: [
        { id: 1, name: "Action" },
        { id: 2, name: "Adventure" },
      ],
      original_language: "en",
      original_title: "Original Title",
      release_date: "2022-01-01",
    };

    const { getByText } = render(<MovieDetailInformation {...props} />);

    expect(getByText("Action / Adventure")).toBeInTheDocument();
    expect(getByText("2022")).toBeInTheDocument();
    expect(getByText("Origins: Original Title (English)")).toBeInTheDocument();
  });

  it("renders movie information with correct language name for French", () => {
    const props = {
      genres: [
        { id: 3, name: "Action" },
        { id: 4, name: "Adventure" },
      ],
      original_language: "fr",
      original_title: "Titre Original",
      release_date: "2022-01-01",
    };

    const { getByText } = render(<MovieDetailInformation {...props} />);

    expect(getByText("Origins: Titre Original (French)")).toBeInTheDocument();
  });

  it("renders only the year of the release date", () => {
    const props = {
      genres: [
        { id: 2, name: "Adventure" },
        { id: 3, name: "Sci-Fi" },
      ],
      original_language: "en",
      original_title: "Original Title",
      release_date: "2022-01-01",
    };

    const { getByText, queryByText } = render(
      <MovieDetailInformation {...props} />,
    );

    expect(getByText("2022")).toBeInTheDocument(); // Check that the year is visible
    expect(queryByText("Jan")).toBeNull(); // Check that the month is not visible
    expect(queryByText("01")).toBeNull(); // Check that the day is not visible
  });
});
