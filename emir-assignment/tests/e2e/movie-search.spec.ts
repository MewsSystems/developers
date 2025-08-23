import { test, expect } from "@playwright/test";

test("User searches and navigates to a popular movie detail page", async ({
    page,
}) => {
    const movieTitle = "The Dark Knight";
    const movieYear = "2008";

    await page.goto("http://localhost:5173");

    const input = page.getByPlaceholder("Search for a movie (e.g., Parasite)…");
    await expect(input).toBeVisible();
    await input.fill(`${movieTitle} ${movieYear}`);

    // Wait for results to appear
    await expect(
        page.getByRole("link", { name: `${movieTitle} (${movieYear})` })
    ).toBeVisible();

    // Click the correct result
    await page
        .getByRole("link", { name: `${movieTitle} (${movieYear})` })
        .click();

    // Validate that the movie detail page loads
    await expect(page.getByRole("heading", { name: movieTitle })).toBeVisible();
    await expect(page.getByText("Overview")).toBeVisible();
});
