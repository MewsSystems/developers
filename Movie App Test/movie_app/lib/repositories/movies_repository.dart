import 'package:movie_app/data/api_services.dart';
import 'package:movie_app/models/movie.dart';

class TheMovieDbRepository {
  final TheMovieDbService theMovieDbService;

  TheMovieDbRepository({
    required this.theMovieDbService,
  });

  /// Get list of popular movies (paginated set to 1 default)
  Future<List<Movie>?> getPopularMovies(int page) async {
    return await theMovieDbService.getListOfPopularMovies(page: page);
  }
}
