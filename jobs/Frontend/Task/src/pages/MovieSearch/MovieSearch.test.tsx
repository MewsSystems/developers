import React from "react";
import { render, screen, waitFor } from "@testing-library/react";
import { userEvent } from "@testing-library/user-event";
import { Wrapper } from "../../mocks/Wrapper";
import { server } from "../../mocks/server";
import { movieSearchHandlers } from "../../mocks/handlers";
import { MovieSearch } from "./MovieSearch";

describe("<MovieSearch />", () => {
  it("should render the component correctly when no search query has been entered", async () => {
    render(<MovieSearch />, { wrapper: Wrapper });

    await waitFor (() => {
      expect(screen.getByText("Are you looking for a movie?")).toBeInTheDocument();
    });
  });
  
  it("should render the component correctly when no data is returned after searching", async () => {
    server.use(movieSearchHandlers.noDataHandler);

    render(<MovieSearch />, { wrapper: Wrapper });

    await userEvent.type(screen.getByRole("textbox"), "test");

    await waitFor (() => {
      expect(screen.getByText("No movies titles found...")).toBeInTheDocument();
    });
  });
  
  it("should render the component correctly when data is returned after searching", async () => {
    render( <MovieSearch />, { wrapper: Wrapper });

    await userEvent.type(screen.getByRole("textbox"), "test");

    await waitFor (() => {
      expect(screen.getByRole("cell", { 
        name: "The Lord of the Rings: The Two Towers" 
      })).toBeInTheDocument();
    });

    expect(screen.getByRole("cell", { name: "2002-12-18" })).toBeInTheDocument();
    expect(screen.getByRole("cell", { name: "They are taking the Hobbits to Isengard" })).toBeInTheDocument();
    expect(screen.getByRole("cell", { name: "English" })).toBeInTheDocument();
  });

  it("should render images instead of a table after selecting the image button", async () => {
    render( <MovieSearch />, { wrapper: Wrapper });

    await userEvent.click(screen.getByRole("button", {  name: /image/i}));
    await userEvent.type(screen.getByRole("textbox"), "test");

    await waitFor (() => {
      expect(screen.getByRole("img", {  
        name: "The Lord of the Rings: The Two Towers"
      })).toBeInTheDocument();
    });
  });

  it("should render the component correctly when an error is returned", async () => {
    server.use(movieSearchHandlers.errorHandler);

    render(<MovieSearch />, { wrapper: Wrapper });

    await waitFor (() => {
      expect(screen.getByText("Something went wrong")).toBeInTheDocument();
    });
  });
});
