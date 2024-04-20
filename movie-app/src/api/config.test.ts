import axios from "axios";
import { beforeAll, describe, expect, it, vi } from "vitest";

import { fetchAPI } from "@/api/config.ts";

// // Mock axios

describe("fetchAPI", () => {
  beforeAll(() => {
    vi.mock("axios", async () => ({
      create: vi.fn(() => ({
        get: vi.fn(() => Promise.resolve({ data: {} })),
      })),
    }));
  });
  it("should call axios.get with correct URL", async () => {
    process.env.VITE_MOVIEDB_API_KEY = "test-key";

    // Call fetchAPI
    await fetchAPI("search/movie", { params: { query: "test", page: 1 } });

    // Assert axios.get is called with correct URL
    expect(axios.create().get).toHaveBeenCalledWith("search/movie", {
      baseURL: "https://api.themoviedb.org/3/",
      params: { query: "test", page: 1, api_key: "test-key" },
    });
  });
});
