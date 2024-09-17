import { http, HttpResponse } from "msw";
import { BASE_URL } from "../constants";
import { movieDetailsMockData } from "./data/movieDetails";
import { searchMovieMockData } from "./data/searchMovie";

export const movieSearchHandlers = {
  defaultHandler: http.get(`${BASE_URL}/search/movie`, () => {
    return HttpResponse.json(searchMovieMockData);
  }),
  noDataHandler: http.get(`${BASE_URL}/search/movie`, () => {
    return HttpResponse.json(null);
  }),
  errorHandler: http.get(`${BASE_URL}/search/movie`, () => {
    return HttpResponse.json("Something went wrong", { status: 500 });
  }),
};

export const movieDetailHandlers = {
  defaultHandler: http.get(`${BASE_URL}/movie/:id`, () => {
    return HttpResponse.json(movieDetailsMockData);
  }),
  noDataHandler: http.get(`${BASE_URL}/movie/:id`, () => {
    return HttpResponse.json(null);
  }),
  errorHandler: http.get(`${BASE_URL}/movie/:id`, () => {
    return HttpResponse.json("Something went wrong", { status: 500 });
  }) 
};

export const handlers = [
  movieSearchHandlers.defaultHandler,
  movieDetailHandlers.defaultHandler,
];
