import { render, screen, fireEvent } from "@testing-library/react";
import { QueryClient, QueryClientProvider } from "react-query";
import { Homepage } from "./Homepage";

const queryClient = new QueryClient();

const MockHomepage = () => (
    <QueryClientProvider client={queryClient}>
        <Homepage />
    </QueryClientProvider>
);

describe("Homepage", () => {
    test("renders Homepage with SearchForm, MovieGrid, Header, and Footer", () => {
        render(<MockHomepage />);

        expect(screen.getByRole("heading", { name: /Mewsvies/i })).toBeInTheDocument();
        expect(screen.getByRole("navigation")).toBeInTheDocument();
        expect(screen.getByRole("contentinfo")).toBeInTheDocument();
        expect(screen.getByText("David Portilla")).toBeInTheDocument();
    });

    test("updates searchTerm and page when handleSearch is called", () => {
        render(<MockHomepage />);

        const searchInput = screen.getByPlaceholderText("Search movies ...");
        fireEvent.change(searchInput, { target: { value: "Star Wars" } });
        fireEvent.submit(screen.getByTestId("search-form"));

        expect(queryClient.getQueryData("searchTerm")).toBe("Star Wars");
        expect(queryClient.getQueryData("page")).toBe(1);
    });
});
