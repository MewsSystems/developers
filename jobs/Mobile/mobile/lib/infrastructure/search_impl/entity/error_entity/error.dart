import 'package:dio/dio.dart';
import 'package:freezed_annotation/freezed_annotation.dart';
import 'package:mobile/domain/core/error.dart';

part 'error.freezed.dart';

@freezed
class CinephileErrorEntity with _$CinephileErrorEntity {
  const CinephileErrorEntity._();
  const factory CinephileErrorEntity({required String error}) =
      _CinephileErrorEntity;

  /// Computes the appropratie error message from [DioErrorType].
  factory CinephileErrorEntity.fromCode(DioErrorType type) {
    String message = 'Unknown Error.';
    switch (type) {
      case DioErrorType.connectTimeout:
        message = 'No Internet Connection. Fix And Try Again.';
        break;
      case DioErrorType.sendTimeout:
        message = 'TimeOut. Check your Network settings And Try Again.';
        break;
      case DioErrorType.cancel:
        message = 'Request Cancelled.';
        break;
      case DioErrorType.response:
        message = 'Server Error.';
        break;
      case DioErrorType.other:
        message = 'Unknown Error.';
        break;
      default:
        message = 'Unknown Error';
    }
    return CinephileErrorEntity(error: message);
  }

  CinephileError toModel() {
    return CinephileError(error);
  }
}
