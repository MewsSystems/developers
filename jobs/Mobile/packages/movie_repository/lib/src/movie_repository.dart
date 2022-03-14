import 'package:movie_repository/src/models/member.dart';
import 'package:tmdb_api/tmdb_api.dart'
    hide Movie, MoviePreview, Genre, SearchResult, Credits;
import 'package:movie_repository/src/models/models.dart';

class MovieRepository {
  MovieRepository({
    TMDbApiClient? apiClient,
  }) : _apiClient = apiClient ?? TMDbApiClient();

  final TMDbApiClient _apiClient;

  Future<MovieDetails> getMovieDetails(int movieId) async {
    final movieDetails = await _apiClient.getMovie(movieId);

    return MovieDetails(
      adult: movieDetails.adult,
      budget: movieDetails.budget,
      genres: movieDetails.genres.map((genre) => genre.name).toList(),
      id: movieDetails.id,
      title: movieDetails.title,
      posterPath: movieDetails.posterPath,
      releaseDate: movieDetails.releaseDate,
      voteAverage: movieDetails.voteAverage.toDouble(),
      voteCount: movieDetails.voteCount,
      backdrop: movieDetails.backdropPath,
      overview: movieDetails.overview,
      runtime: movieDetails.runtime,
    );
  }

  Future<SearchResult> search(String query, [int page = 1]) async {
    final searchResult = await _apiClient.searchMovie(query, page);

    return SearchResult(
      previews: searchResult.results
          .map((preview) => MoviePreview(
                id: preview.id,
                title: preview.title,
                voteAverage: preview.voteAverage.toDouble(),
                posterPath: preview.posterPath,
                releaseDate: preview.releaseDate,
                overview: preview.overview,
              ))
          .toList(),
      totalPages: searchResult.totalPages,
      totalResults: searchResult.totalResults,
    );
  }

  Future<List<Member>> getCast(int movieId) async {
    final movieCredits = await _apiClient.getMovieCredits(movieId);

    return movieCredits.cast
        .map((member) => Member(
              name: member.name,
              posterPath: member.profilePath,
            ))
        .toList();
  }
}
