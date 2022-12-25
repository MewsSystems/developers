import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:movies/src/api/movie_details_api.dart';
import 'package:movies/src/model/movie_details/movie_details.dart';

part 'movie_details_state.freezed.dart';

@freezed
class MovieDetailsState with _$MovieDetailsState  {
  const factory MovieDetailsState.noSelection() = NoSelection;
  const factory MovieDetailsState.loading() = Loading;
  const factory MovieDetailsState.details(MovieDetails movieDetails) = Details;
  const factory MovieDetailsState.error(MovieDetailsError error) = Error;
}