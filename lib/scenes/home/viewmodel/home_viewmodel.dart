import 'package:equatable/equatable.dart';

import '../models/movie.dart';

class HomeViewModel extends Equatable {
  final List<Movie> movies;

  const HomeViewModel({required this.movies});

  HomeViewModel copyWith({List<Movie>? movies}) => HomeViewModel(
        movies: movies ?? this.movies,
      );

  @override
  List<Object?> get props => [movies];
}
