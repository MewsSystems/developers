import { test, expect } from "@playwright/test";

test.describe("searchBar", () => {
  test.beforeEach(async ({ page }) => {
    await page.goto("");
    await page.locator("input").fill("");
  });

  test("empty search shows please search", async ({ page }) => {
    await page.locator("input").fill("");

    await expect(page.getByText(/Search for a movie/)).toBeVisible();
  });

  test("empty search after filling shows please search", async ({ page }) => {
    await page.locator("input").fill("±");
    await page.waitForTimeout(1000);
    await page.locator("input").fill("");
    await page.waitForTimeout(1000);

    await expect(page.getByText(/Search for a movie/)).toBeVisible();
  });

  test("search returns no results", async ({ page }) => {
    await page.locator("input").fill("±");
    await page.waitForTimeout(1000);

    await expect(page.getByText(/No movies found/)).toBeVisible();
  });

  test("search returns results", async ({ page }) => {
    await page.locator("input").fill("a");
    await page.waitForTimeout(1000);

    expect(await page.locator("img").count()).toBeGreaterThan(0);
  });
});
