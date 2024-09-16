import "@testing-library/jest-dom";
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import Home, { getServerSideProps } from "@/pages/index";
import { movieService } from "@/services/movieService";
import { AppProvider } from "@/context";
import { TestReq, TestRes } from "@/types";

jest.mock("next/router", () => ({
  push: jest.fn(),
}));

jest.mock("../../../services/movieService");

const defaultResults = {
  page: 1,
  results: [],
};

function TestComponent() {
  return (
    <AppProvider>
      <Home />
    </AppProvider>
  );
}

const renderComponent = () => waitFor(() => render(<TestComponent />));

describe("Home", () => {
  it("displays search field", async () => {
    await renderComponent();

    const field = screen.getByRole("textbox");

    expect(field).toBeInTheDocument();
  });

  it("displays search loader when user types", async () => {
    await renderComponent();

    const field = screen.getByRole("textbox");

    const delayedPromise = new Promise((resolve) => setTimeout(resolve, 300));
    (movieService.search as jest.MockedFunction<any>).mockImplementation(
      () => delayedPromise
    );

    await userEvent.type(field, "dune");

    const spinner = screen.getByRole("progressbar");

    expect(spinner).toBeInTheDocument();
  });

  it("hides search loader when user clears", async () => {
    const user = userEvent.setup({ delay: null });
    jest.useFakeTimers();

    await renderComponent();

    const field = screen.getByRole("textbox");

    const delayedPromise = new Promise((resolve) => setTimeout(resolve, 300));
    (movieService.search as jest.MockedFunction<any>).mockImplementation(
      () => delayedPromise
    );

    await user.type(field, "dune");

    expect(screen.queryByRole("progressbar")).toBeInTheDocument();

    await user.clear(field);

    await waitFor(() => jest.advanceTimersByTime(500));

    await waitFor(() => {
      expect(screen.queryByRole("progressbar")).not.toBeInTheDocument();
    });

    jest.useRealTimers();
  });

  it("displays loader after minimum characters typed", async () => {
    await renderComponent();

    const field = screen.getByRole("textbox");

    await userEvent.type(field, "du");

    const delayedPromise = new Promise((resolve) => setTimeout(resolve, 300));
    (movieService.search as jest.MockedFunction<any>).mockImplementation(
      () => delayedPromise
    );

    let spinner = screen.queryByRole("progressbar");

    expect(spinner).not.toBeInTheDocument();

    await userEvent.type(field, "dun");

    spinner = screen.queryByRole("progressbar");

    expect(spinner).toBeInTheDocument();
  });

  it("should return empty when search term or number not given", async () => {
    const context = {
      req: jest.fn() as TestReq,
      res: jest.fn() as TestRes,
      query: {
        query: "",
        page: "",
      },
      resolvedUrl: "",
    };

    (movieService.search as jest.Mock<any>).mockResolvedValue(defaultResults);

    const props = await getServerSideProps(context);

    expect(props).toEqual({
      props: {
        searchTerm: "",
        searchResult: {},
      },
    });
  });

  it("should fetch results when search term and number given", async () => {
    const context = {
      req: jest.fn() as TestReq,
      res: jest.fn() as TestRes,
      query: {
        query: "test",
        page: "1",
      },
      resolvedUrl: "",
    };

    (movieService.search as jest.Mock<any>).mockResolvedValue(defaultResults);

    const props = await getServerSideProps(context);

    expect(props).toEqual({
      props: {
        searchTerm: "test",
        searchResult: {
          page: 1,
          results: [],
        },
      },
    });
  });
});
