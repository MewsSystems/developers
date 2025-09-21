import { defaultSystem } from "@chakra-ui/react";
import { render, screen, waitFor } from "@testing-library/react";
import {
  expect,
  test,
  vi,
  type Mock,
  describe,
  type MockInstance,
  beforeEach,
  afterEach,
} from "vitest";
import { MovieDetailsRouteComponent } from "../index";
import { ChakraProvider } from "@chakra-ui/react";
import "@testing-library/jest-dom/vitest";
import { useParams } from "@tanstack/react-router";
import { AuthContext } from "@/entities/auth/api/providers/AuthProvider";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import nock from "nock";
import { mockGetMovieAppended } from "./dataGetMovieAppended.mock";
import { mockGetMovieCollection } from "./dataGetMovieCollection.mock";
import { mockGetConfiguration } from "./dataGetConfiguration.mock";
import { mockGetMovieImages } from "./dataGetMovieImages.mock";

const MOCK_API_KEY = "XXXXXXXXXXXXXXXXXXXXX";

describe("MovieDetailsRouteComponent", async () => {
  afterEach(() => {
    nock.cleanAll();
  });
  let notifyResizeObserverChange: (arg: unknown) => void,
    consoleWarnSpy: MockInstance<(...args: any[]) => void>,
    resizeObserverMock: Mock<(arg: any) => any>;

  beforeEach(() => {
    vi.mock("@/shared/api/config", () => {
      return {
        getApiKey: () => MOCK_API_KEY,
      };
    });

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
  test("error", async () => {
    vi.mock("@tanstack/react-router", { spy: true });
    vi.spyOn(console, "error").mockImplementation(() => {});
    (useParams as Mock).mockImplementation(() => ({ movieId: "123" }));

    nock("https://api.themoviedb.org")
      .get("/3/movie/123")
      .once()
      .query({
        api_key: MOCK_API_KEY,
        session_id: "aaa",
        language: "en-US",
        append_to_response: [
          "keywords",
          "videos",
          "reviews",
          "recommendations",
          "credits",
          "account_states",
        ].join(","),
      })
      .reply(500, { error: "some error" });

    render(
      <TestProvider>
        <MovieDetailsRouteComponent />
      </TestProvider>
    );

    expect(screen.getByText(/LOADING/)).toBeInTheDocument();
    await waitFor(() => {
      expect(screen.getByText(/Sorry, error/)).toBeInTheDocument();
    });
  });

  test("render", async () => {
    vi.mock("@tanstack/react-router", { spy: true });
    (useParams as Mock).mockImplementation(() => ({ movieId: "123" }));

    nock("https://api.themoviedb.org")
      .get("/3/movie/123")
      .once()
      .query({
        api_key: MOCK_API_KEY,
        session_id: "aaa",
        language: "en-US",
        append_to_response: [
          "keywords",
          "videos",
          "reviews",
          "recommendations",
          "credits",
          "account_states",
        ].join(","),
      })
      .reply(200, mockGetMovieAppended);

    nock("https://api.themoviedb.org/")
      .get("/3/collection/2344")
      .once()
      .query({
        api_key: MOCK_API_KEY,
        language: "en-US",
      })
      .reply(200, mockGetMovieCollection);

    nock("https://api.themoviedb.org/")
      .get("/3/configuration")
      .once()
      .query({
        api_key: MOCK_API_KEY,
      })
      .reply(200, mockGetConfiguration);

    nock("https://api.themoviedb.org/")
      .get("/3/movie/123/images")
      .once()
      .query({
        api_key: MOCK_API_KEY,
      })
      .reply(200, mockGetMovieImages);

    render(
      <TestProvider>
        <MovieDetailsRouteComponent />
      </TestProvider>
    );

    await waitFor(() => {
      expect(screen.getByText(/Lana Wachowski/)).toBeInTheDocument();
      expect(screen.getByText(/Keanu Reeves/)).toBeInTheDocument();
      expect(screen.getByText(/Recommendations/)).toBeInTheDocument();
      expect(
        screen.queryAllByText(/The Matrix Revolutions/)[1]
      ).toBeInTheDocument();
      expect(
        screen.getByText(/Part of The Matrix Collection/)
      ).toBeInTheDocument();
    });
  });
});

function TestProvider({ children }: React.PropsWithChildren) {
  const queryClient = new QueryClient({
    defaultOptions: {
      queries: {
        retry: false,
      },
    },
  });
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
