// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'error.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more informations: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

/// @nodoc
class _$CinephileErrorTearOff {
  const _$CinephileErrorTearOff();

  _CinephileError call(String cinephileError) {
    return _CinephileError(
      cinephileError,
    );
  }
}

/// @nodoc
const $CinephileError = _$CinephileErrorTearOff();

/// @nodoc
mixin _$CinephileError {
  String get cinephileError => throw _privateConstructorUsedError;

  @JsonKey(ignore: true)
  $CinephileErrorCopyWith<CinephileError> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $CinephileErrorCopyWith<$Res> {
  factory $CinephileErrorCopyWith(
          CinephileError value, $Res Function(CinephileError) then) =
      _$CinephileErrorCopyWithImpl<$Res>;
  $Res call({String cinephileError});
}

/// @nodoc
class _$CinephileErrorCopyWithImpl<$Res>
    implements $CinephileErrorCopyWith<$Res> {
  _$CinephileErrorCopyWithImpl(this._value, this._then);

  final CinephileError _value;
  // ignore: unused_field
  final $Res Function(CinephileError) _then;

  @override
  $Res call({
    Object? cinephileError = freezed,
  }) {
    return _then(_value.copyWith(
      cinephileError: cinephileError == freezed
          ? _value.cinephileError
          : cinephileError // ignore: cast_nullable_to_non_nullable
              as String,
    ));
  }
}

/// @nodoc
abstract class _$CinephileErrorCopyWith<$Res>
    implements $CinephileErrorCopyWith<$Res> {
  factory _$CinephileErrorCopyWith(
          _CinephileError value, $Res Function(_CinephileError) then) =
      __$CinephileErrorCopyWithImpl<$Res>;
  @override
  $Res call({String cinephileError});
}

/// @nodoc
class __$CinephileErrorCopyWithImpl<$Res>
    extends _$CinephileErrorCopyWithImpl<$Res>
    implements _$CinephileErrorCopyWith<$Res> {
  __$CinephileErrorCopyWithImpl(
      _CinephileError _value, $Res Function(_CinephileError) _then)
      : super(_value, (v) => _then(v as _CinephileError));

  @override
  _CinephileError get _value => super._value as _CinephileError;

  @override
  $Res call({
    Object? cinephileError = freezed,
  }) {
    return _then(_CinephileError(
      cinephileError == freezed
          ? _value.cinephileError
          : cinephileError // ignore: cast_nullable_to_non_nullable
              as String,
    ));
  }
}

/// @nodoc

class _$_CinephileError extends _CinephileError {
  const _$_CinephileError(this.cinephileError) : super._();

  @override
  final String cinephileError;

  @override
  String toString() {
    return 'CinephileError(cinephileError: $cinephileError)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _CinephileError &&
            const DeepCollectionEquality()
                .equals(other.cinephileError, cinephileError));
  }

  @override
  int get hashCode => Object.hash(
      runtimeType, const DeepCollectionEquality().hash(cinephileError));

  @JsonKey(ignore: true)
  @override
  _$CinephileErrorCopyWith<_CinephileError> get copyWith =>
      __$CinephileErrorCopyWithImpl<_CinephileError>(this, _$identity);
}

abstract class _CinephileError extends CinephileError {
  const factory _CinephileError(String cinephileError) = _$_CinephileError;
  const _CinephileError._() : super._();

  @override
  String get cinephileError;
  @override
  @JsonKey(ignore: true)
  _$CinephileErrorCopyWith<_CinephileError> get copyWith =>
      throw _privateConstructorUsedError;
}
