import { defaultSystem } from "@chakra-ui/react";
import { render, screen } from "@testing-library/react";
import {
  expect,
  test,
  vi,
  type Mock,
  describe,
  type MockInstance,
  beforeEach,
} from "vitest";
import { MovieDetailsRouteComponent } from "../index";
import { ChakraProvider } from "@chakra-ui/react";
import "@testing-library/jest-dom/vitest";
import { useParams } from "@tanstack/react-router";
import { useQueryMovieDetails } from "@/pages/movie-details/hooks/useQueryMovieDetails";
import { data as dataMock } from "./data.mock";
import { AuthContext } from "@/entities/auth/api/providers/AuthProvider";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";

describe("MovieDetailsRouteComponent", async () => {
  let notifyResizeObserverChange: (arg: unknown) => void,
    consoleWarnSpy: MockInstance<(...args: any[]) => void>,
    resizeObserverMock: Mock<(arg: any) => any>;
  beforeEach(() => {
    //copied from https://github.com/recharts/recharts/blob/0525504e798ccf968eeed0faf9456001fb04ba08/test/component/ResponsiveContainer.spec.tsx#L25
    /**
     * ResizeObserver is not available, so we have to create a mock to avoid error coming
     * from `react-resize-detector`.
     * @see https://github.com/maslianok/react-resize-detector/issues/145
     *
     * This mock also allow us to use {@link notifyResizeObserverChange} to fire changes
     * from inside our test.
     */
    resizeObserverMock = vi.fn().mockImplementation((callback) => {
      notifyResizeObserverChange = callback;

      return {
        observe: vi.fn(),
        unobserve: vi.fn(),
        disconnect: vi.fn(),
      };
    });
    consoleWarnSpy = vi
      .spyOn(console, "warn")
      .mockImplementation((): void => undefined);

    delete (window as any)["ResizeObserver"];

    window.ResizeObserver = resizeObserverMock;
  });
  test("error", () => {
    vi.mock("@tanstack/react-router", { spy: true });
    vi.mock("@/pages/movie-details/hooks/useQueryMovieDetails", { spy: true });
    vi.spyOn(console, "error").mockImplementation(() => {});
    (useParams as Mock).mockImplementation(() => ({ movieId: "123" }));
    (useQueryMovieDetails as Mock).mockImplementation(() => ({
      data: undefined,
      isLoading: false,
      isError: true,
      error: "some error",
    }));

    render(
      <TestProvider>
        <MovieDetailsRouteComponent />
      </TestProvider>
    );

    expect(screen.getByText(/Sorry, error/)).toBeInTheDocument();
    expect(console.error).toHaveBeenCalledWith("some error");
  });

  test("render", () => {
    vi.mock("@tanstack/react-router", { spy: true });
    vi.mock("@/pages/movie-details/hooks/useQueryMovieDetails", { spy: true });
    vi.spyOn(console, "error").mockImplementation(() => {});
    (useParams as Mock).mockImplementation(() => ({ movieId: "123" }));
    (useQueryMovieDetails as Mock).mockImplementation(() => ({
      data: dataMock,
      isLoading: false,
      isError: false,
      error: undefined,
    }));

    render(
      <TestProvider>
        <MovieDetailsRouteComponent />
      </TestProvider>
    );

    expect(screen.getByText(/Lana Wachowski/)).toBeInTheDocument();
    expect(screen.getByText(/Keanu Reeves/)).toBeInTheDocument();
    expect(screen.getByText(/Recommendations/)).toBeInTheDocument();
    expect(screen.queryAllByText(/The Matrix Revolutions/)[1]).toBeInTheDocument();
    expect(screen.getByText(/Part of The Matrix Collection/)).toBeInTheDocument();
    
  });
});

function TestProvider({ children }: React.PropsWithChildren) {
  const queryClient = new QueryClient();
  return (
    <QueryClientProvider client={queryClient}>
      <AuthContext.Provider
        value={{
          isAuthenticated: true,
          accountId: 1,
          sessionId: "aaa",
          createSession: async (_requestToken) => {
            return true;
          },
          login: async () => {},
          logout: async () => {},
        }}
      >
        <ChakraProvider value={defaultSystem}>{children}</ChakraProvider>
      </AuthContext.Provider>
    </QueryClientProvider>
  );
}
