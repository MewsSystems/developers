import { test, expect } from "@playwright/test"

test("Search and detail flow", async ({ page }) => {
  await page.goto("http://127.0.0.1:5173/")
  await page.getByTestId("searchInput").click()
  await page.getByTestId("searchInput").fill("Batman")
  await page.getByTitle("3", { exact: true }).click()
  await page.getByTestId("detailLink").first().click()
  await page.getByTestId("backLink").click()
  await expect(page.getByTestId("searchInput")).toHaveValue("Batman")
})
