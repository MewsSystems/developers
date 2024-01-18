import type { MoviesDetailsFull } from "../../../src/lib/types";
import { faker } from "@faker-js/faker";

export default class MoviesMother {
  private poster_path: string;
  private original_title: string;
  private release_date: string;
  private vote_average: number;
  private id: number;
  private overview: string;

  constructor() {
    this.id = faker.number.int();
    this.poster_path = faker.image.url();
    this.original_title = faker.string.sample();
    this.release_date = "2011-12-13";
    this.vote_average = faker.number.float();
    this.overview = faker.lorem.paragraph();
  }

  withId(id: number): MoviesMother {
    this.id = id;
    return this;
  }

  withPosterPath(poster_path: string): MoviesMother {
    this.poster_path = poster_path;
    return this;
  }

  withReleaseDate(release_date: string): MoviesMother {
    this.release_date = release_date;
    return this;
  }

  withOriginalTitle(original_title: string): MoviesMother {
    this.original_title = original_title;
    return this;
  }

  withVoteAvergae(vote_average: number) {
    this.vote_average = vote_average;
    return this;
  }

  withOverview(overview: string) {
    this.overview = overview;
    return this;
  }
  build(): MoviesDetailsFull {
    return {
      poster_path: this.poster_path,
      original_title: this.original_title,
      release_date: this.release_date,
      vote_average: this.vote_average,
      id: this.id,
      overview: this.overview,
    };
  }

  static random(): MoviesDetailsFull {
    return new MoviesMother().build();
  }
}
