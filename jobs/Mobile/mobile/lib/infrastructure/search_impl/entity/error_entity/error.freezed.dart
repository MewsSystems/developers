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
class _$CinephileErrorEntityTearOff {
  const _$CinephileErrorEntityTearOff();

  _CinephileErrorEntity call({required String error}) {
    return _CinephileErrorEntity(
      error: error,
    );
  }
}

/// @nodoc
const $CinephileErrorEntity = _$CinephileErrorEntityTearOff();

/// @nodoc
mixin _$CinephileErrorEntity {
  String get error => throw _privateConstructorUsedError;

  @JsonKey(ignore: true)
  $CinephileErrorEntityCopyWith<CinephileErrorEntity> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $CinephileErrorEntityCopyWith<$Res> {
  factory $CinephileErrorEntityCopyWith(CinephileErrorEntity value,
          $Res Function(CinephileErrorEntity) then) =
      _$CinephileErrorEntityCopyWithImpl<$Res>;
  $Res call({String error});
}

/// @nodoc
class _$CinephileErrorEntityCopyWithImpl<$Res>
    implements $CinephileErrorEntityCopyWith<$Res> {
  _$CinephileErrorEntityCopyWithImpl(this._value, this._then);

  final CinephileErrorEntity _value;
  // ignore: unused_field
  final $Res Function(CinephileErrorEntity) _then;

  @override
  $Res call({
    Object? error = freezed,
  }) {
    return _then(_value.copyWith(
      error: error == freezed
          ? _value.error
          : error // ignore: cast_nullable_to_non_nullable
              as String,
    ));
  }
}

/// @nodoc
abstract class _$CinephileErrorEntityCopyWith<$Res>
    implements $CinephileErrorEntityCopyWith<$Res> {
  factory _$CinephileErrorEntityCopyWith(_CinephileErrorEntity value,
          $Res Function(_CinephileErrorEntity) then) =
      __$CinephileErrorEntityCopyWithImpl<$Res>;
  @override
  $Res call({String error});
}

/// @nodoc
class __$CinephileErrorEntityCopyWithImpl<$Res>
    extends _$CinephileErrorEntityCopyWithImpl<$Res>
    implements _$CinephileErrorEntityCopyWith<$Res> {
  __$CinephileErrorEntityCopyWithImpl(
      _CinephileErrorEntity _value, $Res Function(_CinephileErrorEntity) _then)
      : super(_value, (v) => _then(v as _CinephileErrorEntity));

  @override
  _CinephileErrorEntity get _value => super._value as _CinephileErrorEntity;

  @override
  $Res call({
    Object? error = freezed,
  }) {
    return _then(_CinephileErrorEntity(
      error: error == freezed
          ? _value.error
          : error // ignore: cast_nullable_to_non_nullable
              as String,
    ));
  }
}

/// @nodoc

class _$_CinephileErrorEntity extends _CinephileErrorEntity {
  const _$_CinephileErrorEntity({required this.error}) : super._();

  @override
  final String error;

  @override
  String toString() {
    return 'CinephileErrorEntity(error: $error)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _CinephileErrorEntity &&
            const DeepCollectionEquality().equals(other.error, error));
  }

  @override
  int get hashCode =>
      Object.hash(runtimeType, const DeepCollectionEquality().hash(error));

  @JsonKey(ignore: true)
  @override
  _$CinephileErrorEntityCopyWith<_CinephileErrorEntity> get copyWith =>
      __$CinephileErrorEntityCopyWithImpl<_CinephileErrorEntity>(
          this, _$identity);
}

abstract class _CinephileErrorEntity extends CinephileErrorEntity {
  const factory _CinephileErrorEntity({required String error}) =
      _$_CinephileErrorEntity;
  const _CinephileErrorEntity._() : super._();

  @override
  String get error;
  @override
  @JsonKey(ignore: true)
  _$CinephileErrorEntityCopyWith<_CinephileErrorEntity> get copyWith =>
      throw _privateConstructorUsedError;
}
