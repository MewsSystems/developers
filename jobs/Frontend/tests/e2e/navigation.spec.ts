import { test, expect } from "@playwright/test";

test.describe("navigation", () => {
  test("homepage title is set correctly", async ({ page }) => {
    await page.goto("");
    await page.waitForURL("**/");
    expect(await page.title()).toBe("Search");
  });

  test("details page title is set correctly", async ({ page }) => {
    await page.goto("/movie/601796");
    await page.waitForURL("**/");
    expect(await page.title()).toBe("Details");
  });

  test("can click on and go to home page when queryParams are undefined", async ({
    page,
  }) => {
    await page.goto("/movie/601796");

    await page.getByText("Home").click();
    await page.waitForURL("**/");

    await expect(page).toHaveTitle(/Search/);
  });

  test("can click on and go to home page when queryParams are set", async ({
    page,
  }) => {
    await page.goto("/?query=a&page=1");
    await page.goto("/movie/601796?query=a&page=1");

    await page.locator("nav").locator("button").first().click();
    await page.waitForURL("**/");

    await expect(page).toHaveTitle(/Search/);
    await expect(page).toHaveURL(/.*?query=a&page=1/);
  });
});
