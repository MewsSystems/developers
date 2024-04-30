interface Resources {
  translation: {
    common: {
      appTitle: 'Movie Search';
      yes: 'Yes';
      no: 'No';
    };
    movieDetails: {
      appTitle: '{{movieTitle}} — Movie Search';
      poster: 'Poster';
      adult: 'R-18';
      overview: 'Overview';
      releaseDate: 'Release Date';
      originalTitle: 'Original Title';
      originalLanguage: 'Original Language';
      title: 'Title';
      popularity: 'Popularity';
      voteCount: 'Vote Count';
      voteAverage: 'Vote Average';
      status: 'Status';
    };
    error: {
      failedMoviesFetch: 'Failed to fetch movies.';
      noMoviesMatch: 'No matching movie(s) found.';
    };
  };
}

export default Resources;
