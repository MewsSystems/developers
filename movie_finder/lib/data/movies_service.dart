import 'package:movie_finder/data/api_client.dart';
import 'package:movie_finder/data/model/movies/movie_details_request.dart';
import 'package:movie_finder/data/model/movies/movie_details_result.dart';
import 'package:movie_finder/data/model/movies/movies_search_request.dart';
import 'package:movie_finder/data/model/movies/movies_search_result.dart';

class MoviesService {
  MoviesService() : _apiClient = ApiClient.instance;

  final ApiClient _apiClient;

  Future<MoviesSearchResult> searchMovies(MoviesSearchRequest request) {
    return _apiClient.get(
      'https://api.themoviedb.org/3/search/movie',
      queryParams: request.toJson(),
      resultFactory: MoviesSearchResult.fromJson,
    );
  }

  Future<MovieDetailsResult> getMovieDetails(MovieDetailsRequest request) {
    return _apiClient.get(
      'https://api.themoviedb.org/3/movie/${request.id}',
      resultFactory: MovieDetailsResult.fromJson,
    );
  }
}
