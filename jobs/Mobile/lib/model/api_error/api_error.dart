import 'package:freezed_annotation/freezed_annotation.dart';

part 'api_error.freezed.dart';

@freezed
class ApiError with _$ApiError {
  const ApiError._();

  const factory ApiError({required int code, String? message}) = _ApiError;
}
