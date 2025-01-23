import { test, expect } from "@playwright/test";

test.describe("pagination", () => {
  test.beforeEach(async ({ page }) => {
    await page.goto("");
    await page.waitForURL("**/");
  });

  test("pagination is not visible before search", async ({ page }) => {
    await expect(page.locator("[data-testid='pagination']")).toBeHidden();
  });

  test("pagination is visible after search", async ({ page }) => {
    await page.goto("/?query=a&page1");
    await page.waitForURL("**/?query=a&page1");

    expect(await page.locator("[data-testid='pagination']").count()).toEqual(1);
  });

  test("pagination previous button is disabled on first page", async ({
    page,
  }) => {
    await page.goto("/?query=a&page=1");
    await page.waitForURL("**/");

    expect(page.url()).toContain("page=1");
    await expect(
      page.locator("[data-testid='pagination'] button").first(),
    ).toBeDisabled();
  });

  test("pagination next button is disabled on last page", async ({ page }) => {
    await page.goto("/?query=a&page=1");
    await page.waitForURL("**/");

    const pageNumberText = await page
      .locator("[data-testid='pageNumbers']")
      .first()
      .innerText();

    const splitValues = pageNumberText.split(" ");
    const lastPageNumber = splitValues[splitValues.length - 1];

    await page.goto(`/?query=a&page=${lastPageNumber}`);
    await page.waitForURL("**/");

    expect(page.url()).toContain(`page=${lastPageNumber}`);
    await expect(
      page.locator("[data-testid='pagination'] button").last(),
    ).toBeDisabled();
  });

  test("pagination next page works", async ({ page }) => {
    await page.goto("/?query=a&page=1");
    await page.waitForURL("**/");

    await page.locator("[data-testid='pagination'] button").last().click();
    await page.waitForURL("**/?query=a&page=2");

    expect(page.url()).toContain("page=2");
  });

  test("pagination previous page works", async ({ page }) => {
    await page.goto("/?query=a&page=2");
    await page.waitForURL("**/");

    await page.locator("[data-testid='pagination'] button").first().click();
    await page.waitForURL("**/?query=a&page=1");

    expect(page.url()).toContain("page=1");
  });
});
