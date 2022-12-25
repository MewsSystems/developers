

import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:movies/src/api/movie_search_api.dart';
import 'package:movies/src/model/movie.dart';

part 'search_state.freezed.dart';

@freezed
class SearchState with _$SearchState {
  const factory SearchState.result(List<Movie> movies) = Result;
  const factory SearchState.error(MovieSearchError error) = Error;
}