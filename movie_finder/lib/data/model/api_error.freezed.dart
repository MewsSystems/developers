// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'api_error.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more informations: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

ApiError<T> _$ApiErrorFromJson<T>(Map<String, dynamic> json) {
  return _ApiError<T>.fromJson(json);
}

/// @nodoc
class _$ApiErrorTearOff {
  const _$ApiErrorTearOff();

  _ApiError<T> call<T>(
      {@JsonKey(name: 'status_message') String? statusMessage,
      @JsonKey(name: 'status_code') int? statusCode}) {
    return _ApiError<T>(
      statusMessage: statusMessage,
      statusCode: statusCode,
    );
  }

  ApiError<T> fromJson<T>(Map<String, Object?> json) {
    return ApiError<T>.fromJson(json);
  }
}

/// @nodoc
const $ApiError = _$ApiErrorTearOff();

/// @nodoc
mixin _$ApiError<T> {
  @JsonKey(name: 'status_message')
  String? get statusMessage => throw _privateConstructorUsedError;
  @JsonKey(name: 'status_code')
  int? get statusCode => throw _privateConstructorUsedError;

  Map<String, dynamic> toJson() => throw _privateConstructorUsedError;
  @JsonKey(ignore: true)
  $ApiErrorCopyWith<T, ApiError<T>> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $ApiErrorCopyWith<T, $Res> {
  factory $ApiErrorCopyWith(
          ApiError<T> value, $Res Function(ApiError<T>) then) =
      _$ApiErrorCopyWithImpl<T, $Res>;
  $Res call(
      {@JsonKey(name: 'status_message') String? statusMessage,
      @JsonKey(name: 'status_code') int? statusCode});
}

/// @nodoc
class _$ApiErrorCopyWithImpl<T, $Res> implements $ApiErrorCopyWith<T, $Res> {
  _$ApiErrorCopyWithImpl(this._value, this._then);

  final ApiError<T> _value;
  // ignore: unused_field
  final $Res Function(ApiError<T>) _then;

  @override
  $Res call({
    Object? statusMessage = freezed,
    Object? statusCode = freezed,
  }) {
    return _then(_value.copyWith(
      statusMessage: statusMessage == freezed
          ? _value.statusMessage
          : statusMessage // ignore: cast_nullable_to_non_nullable
              as String?,
      statusCode: statusCode == freezed
          ? _value.statusCode
          : statusCode // ignore: cast_nullable_to_non_nullable
              as int?,
    ));
  }
}

/// @nodoc
abstract class _$ApiErrorCopyWith<T, $Res>
    implements $ApiErrorCopyWith<T, $Res> {
  factory _$ApiErrorCopyWith(
          _ApiError<T> value, $Res Function(_ApiError<T>) then) =
      __$ApiErrorCopyWithImpl<T, $Res>;
  @override
  $Res call(
      {@JsonKey(name: 'status_message') String? statusMessage,
      @JsonKey(name: 'status_code') int? statusCode});
}

/// @nodoc
class __$ApiErrorCopyWithImpl<T, $Res> extends _$ApiErrorCopyWithImpl<T, $Res>
    implements _$ApiErrorCopyWith<T, $Res> {
  __$ApiErrorCopyWithImpl(
      _ApiError<T> _value, $Res Function(_ApiError<T>) _then)
      : super(_value, (v) => _then(v as _ApiError<T>));

  @override
  _ApiError<T> get _value => super._value as _ApiError<T>;

  @override
  $Res call({
    Object? statusMessage = freezed,
    Object? statusCode = freezed,
  }) {
    return _then(_ApiError<T>(
      statusMessage: statusMessage == freezed
          ? _value.statusMessage
          : statusMessage // ignore: cast_nullable_to_non_nullable
              as String?,
      statusCode: statusCode == freezed
          ? _value.statusCode
          : statusCode // ignore: cast_nullable_to_non_nullable
              as int?,
    ));
  }
}

/// @nodoc
@JsonSerializable()
class _$_ApiError<T> implements _ApiError<T> {
  const _$_ApiError(
      {@JsonKey(name: 'status_message') this.statusMessage,
      @JsonKey(name: 'status_code') this.statusCode});

  factory _$_ApiError.fromJson(Map<String, dynamic> json) =>
      _$$_ApiErrorFromJson(json);

  @override
  @JsonKey(name: 'status_message')
  final String? statusMessage;
  @override
  @JsonKey(name: 'status_code')
  final int? statusCode;

  @override
  String toString() {
    return 'ApiError<$T>(statusMessage: $statusMessage, statusCode: $statusCode)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _ApiError<T> &&
            const DeepCollectionEquality()
                .equals(other.statusMessage, statusMessage) &&
            const DeepCollectionEquality()
                .equals(other.statusCode, statusCode));
  }

  @override
  int get hashCode => Object.hash(
      runtimeType,
      const DeepCollectionEquality().hash(statusMessage),
      const DeepCollectionEquality().hash(statusCode));

  @JsonKey(ignore: true)
  @override
  _$ApiErrorCopyWith<T, _ApiError<T>> get copyWith =>
      __$ApiErrorCopyWithImpl<T, _ApiError<T>>(this, _$identity);

  @override
  Map<String, dynamic> toJson() {
    return _$$_ApiErrorToJson(this);
  }
}

abstract class _ApiError<T> implements ApiError<T> {
  const factory _ApiError(
      {@JsonKey(name: 'status_message') String? statusMessage,
      @JsonKey(name: 'status_code') int? statusCode}) = _$_ApiError<T>;

  factory _ApiError.fromJson(Map<String, dynamic> json) =
      _$_ApiError<T>.fromJson;

  @override
  @JsonKey(name: 'status_message')
  String? get statusMessage;
  @override
  @JsonKey(name: 'status_code')
  int? get statusCode;
  @override
  @JsonKey(ignore: true)
  _$ApiErrorCopyWith<T, _ApiError<T>> get copyWith =>
      throw _privateConstructorUsedError;
}
