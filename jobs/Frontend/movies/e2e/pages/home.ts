import { Locator, Page } from "@playwright/test";

export class HomePage {
  readonly page: Page;
  readonly title: Locator;
  readonly searchInput: Locator;
  readonly movieList: Locator;

  constructor(page: Page) {
    this.page = page;
    this.title = page.getByText(/Welcome to Mews/i);
    this.searchInput = page.getByPlaceholder(/search/i);
    this.movieList = page.getByTestId("movie-card");
  }

  async goto() {
    await this.page.goto("/");
  }
}
