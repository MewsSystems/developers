import { test, expect } from "@playwright/test";

test.describe("movieCard", () => {
  test.beforeEach(async ({ page }) => {
    await page.goto("");
    await page.locator("input").fill("");
  });

  test("movie cards shows expected properties", async ({ page }) => {
    await page.locator("input").fill("a");
    await page.waitForTimeout(1500);

    expect(await page.locator("img").count()).toBeGreaterThan(0);
    await expect(page.locator("a").first()).toContainText(/a/i);
    await expect(page.locator("a").first().locator("img")).toBeInViewport();
    await expect(page.locator("a").first()).toHaveAttribute("href");
    expect(await page.locator("a").first().getAttribute("href")).toMatch(
      /movie\/[0-9]*/,
    );
  });

  test("clicking a movie card goes to its' details page", async ({ page }) => {
    await page.locator("input").fill("a");
    await page.waitForTimeout(1000);

    await page.locator("a").first().click();
    await page.waitForURL("**/movie/**");

    await expect(page).toHaveTitle(/Details/);
  });
});
