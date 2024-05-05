import { test, expect } from "@playwright/test";

test.describe("movieDetails", () => {
  test.beforeEach(async ({ page }) => {
    await page.goto("/movie/601796");
  });

  test("expected movie details are shown", async ({ page }) => {
    expect(await page.locator("img").count()).toEqual(1);
    expect(await page.locator("h1").count()).toEqual(1);
  });
});
