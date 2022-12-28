// ignore_for_file: invalid_annotation_target, depend_on_referenced_packages

import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:movie_search/utils/constants.dart';

part 'search_movies_request.freezed.dart';
part 'search_movies_request.g.dart';

@freezed
class SearchMoviesRequest with _$SearchMoviesRequest {
  factory SearchMoviesRequest({
    @JsonKey(name: 'api_key') @Default(API_KEY) String key,
    @JsonKey(name: 'query') required String searchText,
    @JsonKey(name: 'page') required int page,
  }) = _SearchMoviesRequest;

  factory SearchMoviesRequest.fromJson(Map<String, dynamic> json) =>
      _$SearchMoviesRequestFromJson(json);
}
