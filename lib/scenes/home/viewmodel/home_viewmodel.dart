import 'package:equatable/equatable.dart';

import '../models/movie.dart';

class HomeViewModel extends Equatable {
  final List<Movie> movies;
  final bool? didFailToLoadMoreRecords;

  const HomeViewModel({required this.movies, this.didFailToLoadMoreRecords});

  HomeViewModel copyWith(
          {List<Movie>? movies, bool? didFailToLoadMoreRecords}) =>
      HomeViewModel(
        movies: movies ?? this.movies,
        didFailToLoadMoreRecords: didFailToLoadMoreRecords,
      );

  @override
  List<Object?> get props => [movies, didFailToLoadMoreRecords];
}
