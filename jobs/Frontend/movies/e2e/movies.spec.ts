import { test, expect } from "./utils/fixtures";

test("CUJ", async ({ homePage, moviePage }) => {
  await homePage.goto();

  // check title is loaded
  await expect(homePage.title).toBeVisible();
  await expect(homePage.searchInput).toBeVisible();

  // list should be empty at the start
  await expect(homePage.movieList).toHaveCount(0);

  // search movies using the imput usign harry as query
  await homePage.searchInput.fill("harry");

  // check movie cards are loaded
  await expect(homePage.movieList).toHaveCount(20);

  // click on the first movie
  const firstMovie = homePage.movieList.nth(0);
  firstMovie.click();

  // check movie detail is loaded with the expected data
  await expect(moviePage.title).toBeVisible();
  await expect(moviePage.rating).toBeVisible();
  await expect(moviePage.runtime).toBeVisible();
  await expect(moviePage.gender.first()).toBeVisible();
  await expect(moviePage.overview).toBeVisible();
  await expect(moviePage.production).toBeVisible();

  await moviePage.assertTitle("harry potter and the philosopher");
  await moviePage.assertGenders(["adventure", "fantasy"]);

  // click back button and see we are on the previous page
  await moviePage.backButton.click();

  // check we are back to home and with the right input value
  await expect(homePage.title).toBeVisible();
  await expect(homePage.searchInput).toBeVisible();
  await expect(homePage.searchInput).toHaveValue(/harry/i);
});
