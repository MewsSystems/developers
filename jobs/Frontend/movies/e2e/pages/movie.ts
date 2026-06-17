import { Locator, Page, expect } from "@playwright/test";

export class MoviePage {
  readonly page: Page;
  readonly title: Locator;
  readonly rating: Locator;
  readonly runtime: Locator;
  readonly gender: Locator;
  readonly overview: Locator;
  readonly production: Locator;
  readonly backButton: Locator;

  constructor(page: Page) {
    this.page = page;
    this.title = page.getByTestId("movieDetailTitle");
    this.rating = page.getByTestId("movieDetailRating");
    this.runtime = page.getByTestId("movieDetailRuntime");
    this.gender = page.getByTestId("movieDetailGender");
    this.overview = page.getByTestId("movieDetailOverview");
    this.production = page.getByTestId("movieDetailProduction");
    this.backButton = page.getByRole("button", { name: /back/i });
  }

  async goto(movieId: string) {
    await this.page.goto(`/movie/${movieId}`);
  }

  async assertTitle(title: string) {
    const content = await this.title.textContent();
    expect(content).toMatch(new RegExp(title, "i"));
  }

  async assertGenders(genders: string[]) {
    for (let index = 0; index < genders.length; index++) {
      const gender = genders[index];
      const content = await this.gender.nth(index).textContent();
      expect(content).toMatch(new RegExp(gender, "i"));
    }
  }
}
