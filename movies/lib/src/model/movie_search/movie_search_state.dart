import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:movies/src/api/movie_search_api.dart';
import 'package:movies/src/blocs/movie_search_bloc.dart';
import 'package:movies/src/model/movie/movie.dart';

part 'movie_search_state.freezed.dart';

/// Represent the possible states emitted by a [MovieSearchBloc]
@freezed
class MovieSearchState with _$MovieSearchState {
  const factory MovieSearchState.result(
    String query,
    List<Movie> movies,
    bool isLastPage,
  ) = Result;
  const factory MovieSearchState.error(MovieSearchError error) = Error;
  static const emptyResult = MovieSearchState.result('', [], true);
}
