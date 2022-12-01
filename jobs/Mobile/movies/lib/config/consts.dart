class Api {
  static const key = '03b8572954325680265531140190fd2a';
  static const baseUrl = 'api.themoviedb.org';
  static const searchMoviesPath = '/3/search/movie/';
  static const detailedMoviesPath = '/3/movie/';
  static const locale = 'en-US';
  static const includeAdult = false;
}

class Pagination {
  static const timeout = Duration(milliseconds: 500);
}

class Images {
  static const w780 = 'https://image.tmdb.org/t/p/w780/';
  static const w300 = 'https://image.tmdb.org/t/p/w300/';
}
