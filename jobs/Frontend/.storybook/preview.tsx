import type { Preview } from "@storybook/react-vite";
import { initialize, mswLoader } from "msw-storybook-addon";
import { worker } from "../src/mocks/browser";
import { MemoryRouter } from "react-router-dom";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import "../src/styles/globals.less";

const queryClient = new QueryClient();

initialize();

// Start MSW before rendering stories
if (typeof window !== "undefined") {
  worker.start({
    onUnhandledRequest: "bypass",
  });
}

const preview: Preview = {
  parameters: {
    controls: {
      matchers: {
        color: /(background|color)$/i,
        date: /Date$/i,
      },
    },
  },
  loaders: [mswLoader],
  decorators: [
    (Story) => (
      <>
        <QueryClientProvider client={queryClient}>
          <MemoryRouter initialEntries={["/"]}>
            <Story />
          </MemoryRouter>
        </QueryClientProvider>
      </>
    ),
  ],
};

export default preview;
