import { test, expect } from "@playwright/test";

test("home loads and search shows results", async ({ page }) => {
    // Intercept TMDB search and return fixture
    await page.route("**/3/search/movie**", async (route) => {
        const json = {
            page: 1,
            results: [
                {
                    id: 1,
                    title: "Fake Movie",
                    poster_path: null,
                    backdrop_path: null,
                    overview: "Hello",
                    vote_average: 8.0,
                    vote_count: 100,
                    release_date: "2020-01-01",
                },
            ],
            total_pages: 1,
            total_results: 1,
        };
        await route.fulfill({
            status: 200,
            contentType: "application/json",
            body: JSON.stringify(json),
        });
    });

    await page.goto("/");

    // Find the search box and type a query
    const input = page.getByRole("textbox", { name: /search movies/i });
    await input.fill("fake");
    // debounce delay + fetch time
    await page.waitForTimeout(700);

    // Expect our fake card to appear
    await expect(page.getByRole("link", { name: /fake movie/i })).toBeVisible();
});
