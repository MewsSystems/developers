import { test, expect } from "@playwright/test";

test.describe("searchBar", () => {
  test.beforeEach(async ({ page }) => {
    await page.goto("");
    await page.locator("input").fill("");

    await page.waitForLoadState();
  });

  test("empty search shows please search", async ({ page }) => {
    await page.locator("input").fill("");
    await page.waitForLoadState();

    await expect(page.getByText(/Search for a movie/)).toBeVisible();
  });

  test("empty search after filling shows please search", async ({ page }) => {
    await page.locator("input").fill("±");
    await page.waitForLoadState();
    await page.locator("input").fill("");
    await page.waitForLoadState();

    await expect(page.getByText(/Search for a movie/)).toBeVisible();
  });

  test("search returns no results", async ({ page }) => {
    await page.locator("input").fill("±");
    await page.waitForLoadState();

    await expect(page.getByText(/No movies found/)).toBeVisible();
  });

  test("search returns results", async ({ page }) => {
    await page.locator("input").fill("a");
    await page.waitForLoadState();

    await expect(page.locator("img").first()).toBeVisible();
  });
});
