import { defaultSystem } from "@chakra-ui/react";
import { render, screen } from "@testing-library/react";
import { expect, test, vi } from "vitest";
import { Credits } from "./Credits";
import type { MovieCredits } from "@/entities/movie/types";
import { usePreferredLanguage } from "@uidotdev/usehooks";
import { ChakraProvider } from "@chakra-ui/react";
import "@testing-library/jest-dom/vitest";

test("loads and displays greeting", async () => {
  vi.mock("@uidotdev/usehooks", { spy: true });

  (usePreferredLanguage as any).mockImplementation(() => "en-GB");

  render(
    <TestProvider>
      <Credits credits={getCredits()} />
    </TestProvider>
  );

  expect(screen.getByText(/Lana Wachowski/)).toBeInTheDocument();
  expect(screen.queryByText(/Shauna Wolifson/)).toBeNull();
});

function getCredits(): MovieCredits {
  return {
    cast: [],
    crew: [
      {
        adult: false,
        gender: 0,
        id: 9342,
        known_for_department: "Production",
        name: "Shauna Wolifson",
        original_name: "Shauna Wolifson",
        popularity: 0.0214,
        profile_path: null,
        credit_id: "52fe425cc3a36847f801831d",
        department: "Production",
        job: "Casting",
      },
      {
        adult: false,
        gender: 1,
        id: 9340,
        known_for_department: "Directing",
        name: "Lana Wachowski",
        original_name: "Lana Wachowski",
        popularity: 0.6725,
        profile_path: "/5KuRHnoH8UkSCFHMKf4YjKOvzOM.jpg",
        credit_id: "52fe425cc3a36847f80183c1",
        department: "Directing",
        job: "Director",
      },
    ],
  };
}

function TestProvider({ children }: React.PropsWithChildren) {
  return <ChakraProvider value={defaultSystem}>{children}</ChakraProvider>;
}
