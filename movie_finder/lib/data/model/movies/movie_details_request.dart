import 'package:freezed_annotation/freezed_annotation.dart';
part 'movie_details_request.freezed.dart';
part 'movie_details_request.g.dart';

@freezed
abstract class MovieDetailsRequest with _$MovieDetailsRequest {
  const factory MovieDetailsRequest({
    @JsonKey(
      fromJson: MovieDetailsRequest._stringToInt,
      toJson: MovieDetailsRequest._stringFromInt,
    )
        required int id,
  }) = _MovieDetailsRequest;

  factory MovieDetailsRequest.fromJson(Map<String, dynamic> json) => _$MovieDetailsRequestFromJson(json);
  static int _stringToInt(String number) => int.parse(number);
  static String? _stringFromInt(int number) => number.toString();
}
