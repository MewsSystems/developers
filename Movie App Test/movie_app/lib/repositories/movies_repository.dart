import 'package:movie_app/data/api_services.dart';
import 'package:movie_app/models/movie.dart';
import 'package:movie_app/models/movie_info.dart';

class TheMovieDbRepository {
  final TheMovieDbService theMovieDbService;

  TheMovieDbRepository({
    required this.theMovieDbService,
  });

  /// Get list of popular movies (paginated set to 1 default)
  Future<List<Movie>?> getPopularMovies(int page) async {
    return await theMovieDbService.getListOfPopularMovies(page: page);
  }

  /// Get info about the movie by giving id
  Future<MovieInfo?> getMovieInfo(int movieId) async {
    return await theMovieDbService.getMovieInfo(movieId);
  }

  /// Get info about the movie by giving id
  Future<List<Movie>?> searchMovie(String query) async {
    return await theMovieDbService.getSearchMovies(query);
  }
}
