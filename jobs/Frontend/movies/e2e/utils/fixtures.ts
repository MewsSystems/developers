import { test as baseTest } from "@playwright/test";
import { HomePage } from "../pages/home";
import { MoviePage } from "../pages/movie";

export const test = baseTest.extend<{
  homePage: HomePage;
  moviePage: MoviePage;
}>({
  homePage: async ({ page }, use) => {
    await use(new HomePage(page));
  },
  moviePage: async ({ page }, use) => {
    await use(new MoviePage(page));
  },
});

export { expect } from "@playwright/test";
