import '../models/movie.dart';

class HomeViewModel {
  final List<Movie> movies;

  HomeViewModel({required this.movies});

  HomeViewModel copyWith({List<Movie>? movies}) => HomeViewModel(
        movies: movies ?? this.movies,
      );
}
