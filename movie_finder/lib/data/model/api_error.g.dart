// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'api_error.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

_$_ApiError<T> _$$_ApiErrorFromJson<T>(Map<String, dynamic> json) =>
    _$_ApiError<T>(
      statusMessage: json['status_message'] as String?,
      statusCode: json['status_code'] as int?,
    );

Map<String, dynamic> _$$_ApiErrorToJson<T>(_$_ApiError<T> instance) =>
    <String, dynamic>{
      'status_message': instance.statusMessage,
      'status_code': instance.statusCode,
    };
