import { test, expect } from "@playwright/test";

test("User searches and navigates to a popular movie detail page", async ({
    page,
}) => {
    const movieTitle = "The Dark Knight";
    const movieYear = "2008";

    // use baseURL from playwright.config, so just "/"
    await page.goto("/");

    const input = page.getByPlaceholder(
        "Search for a movie (e.g., Parasite)..."
    );
    await expect(input).toBeVisible();
    await input.fill(`${movieTitle} ${movieYear}`);

    const result = page.getByRole("link", {
        name: `${movieTitle} (${movieYear})`,
    });
    await expect(result).toBeVisible();

    await Promise.all([
        // ensure SPA navigation completes
        page.waitForURL(/\/movie\/\d+(?:\?.*)?$/),
        result.click(),
    ]);

    // Disambiguate: only the hero uses H1
    await expect(
        page.getByRole("heading", { level: 1, name: movieTitle })
    ).toBeVisible();

    // Verify the Overview tab exists on the detail page
    await expect(page.getByRole("tab", { name: "Overview" })).toBeVisible();
});
