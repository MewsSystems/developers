import 'package:freezed_annotation/freezed_annotation.dart';
part 'api_error.freezed.dart';
part 'api_error.g.dart';

@freezed
class ApiError<T> with _$ApiError<T> {
  const factory ApiError({
    @JsonKey(name: 'status_message') String? statusMessage,
    @JsonKey(name: 'status_code') int? statusCode,
  }) = _ApiError;

  factory ApiError.fromJson(Map<String, dynamic> json) => _$ApiErrorFromJson(json);
}
