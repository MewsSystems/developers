import { defaultSystem } from "@chakra-ui/react";
import { render, screen } from "@testing-library/react";
import { expect, test, vi } from "vitest";
import { Credits } from "./Credits";
import { ChakraProvider } from "@chakra-ui/react";
import "@testing-library/jest-dom/vitest";

test("loads and displays greeting", async () => {
  vi.mock("@uidotdev/usehooks", { spy: true });

  render(
    <TestProvider>
      <Credits
        crewDirectors={[
          {
            name: "Lana Wachowski",
            jobs: "Director and Writter",
          },
          {
            name: "Shauna Wolifson",
            jobs: "Director",
          },
        ]}
      />
    </TestProvider>
  );

  expect(screen.getByText(/Lana Wachowski/)).toBeInTheDocument();
  expect(screen.queryByText(/Shauna Wolifson/)).toBeInTheDocument();
});

function TestProvider({ children }: React.PropsWithChildren) {
  return <ChakraProvider value={defaultSystem}>{children}</ChakraProvider>;
}
