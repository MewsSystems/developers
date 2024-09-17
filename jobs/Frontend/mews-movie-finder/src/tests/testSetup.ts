import { setupServer } from "msw/node";
import { afterAll, afterEach, beforeAll } from "vitest";

import '@testing-library/jest-dom'
import { movieHandlers } from "./handlers/movieHandlers";

export const handlers = [
  ...movieHandlers
];

const server = setupServer(...handlers);

beforeAll(() => server.listen({ onUnhandledRequest: "bypass" }));
afterAll(() => server.close());
afterEach(() => server.resetHandlers());
