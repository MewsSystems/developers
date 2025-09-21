import { defaultSystem } from "@chakra-ui/react";
import { render, screen } from "@testing-library/react";
import { expect, test } from "vitest";
import { Credits } from "@/features/crewDirectors/ui/Credits";
import { ChakraProvider } from "@chakra-ui/react";
import "@testing-library/jest-dom/vitest";

test("Credits crew directors", async () => {
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
