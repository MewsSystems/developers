import 'package:freezed_annotation/freezed_annotation.dart';
part 'movies_search_request.g.dart';
part 'movies_search_request.freezed.dart';

@freezed
class MoviesSearchRequest with _$MoviesSearchRequest {
  const factory MoviesSearchRequest({
    required String query,
    @JsonKey(
      fromJson: MoviesSearchRequest._stringToInt,
      toJson: MoviesSearchRequest._stringFromInt,
    )
        int? page,
  }) = _MoviesSearchRequest;

  factory MoviesSearchRequest.fromJson(Map<String, dynamic> json) => _$MoviesSearchRequestFromJson(json);

  static int? _stringToInt(String number) => number == null ? null : int.parse(number);
  static String? _stringFromInt(int? number) => number?.toString();
}
