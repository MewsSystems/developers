// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'movie_details_request.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more informations: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

MovieDetailsRequest _$MovieDetailsRequestFromJson(Map<String, dynamic> json) {
  return _MovieDetailsRequest.fromJson(json);
}

/// @nodoc
class _$MovieDetailsRequestTearOff {
  const _$MovieDetailsRequestTearOff();

  _MovieDetailsRequest call(
      {@JsonKey(fromJson: MovieDetailsRequest._stringToInt, toJson: MovieDetailsRequest._stringFromInt)
          required int id}) {
    return _MovieDetailsRequest(
      id: id,
    );
  }

  MovieDetailsRequest fromJson(Map<String, Object?> json) {
    return MovieDetailsRequest.fromJson(json);
  }
}

/// @nodoc
const $MovieDetailsRequest = _$MovieDetailsRequestTearOff();

/// @nodoc
mixin _$MovieDetailsRequest {
  @JsonKey(
      fromJson: MovieDetailsRequest._stringToInt,
      toJson: MovieDetailsRequest._stringFromInt)
  int get id => throw _privateConstructorUsedError;

  Map<String, dynamic> toJson() => throw _privateConstructorUsedError;
  @JsonKey(ignore: true)
  $MovieDetailsRequestCopyWith<MovieDetailsRequest> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MovieDetailsRequestCopyWith<$Res> {
  factory $MovieDetailsRequestCopyWith(
          MovieDetailsRequest value, $Res Function(MovieDetailsRequest) then) =
      _$MovieDetailsRequestCopyWithImpl<$Res>;
  $Res call(
      {@JsonKey(fromJson: MovieDetailsRequest._stringToInt, toJson: MovieDetailsRequest._stringFromInt)
          int id});
}

/// @nodoc
class _$MovieDetailsRequestCopyWithImpl<$Res>
    implements $MovieDetailsRequestCopyWith<$Res> {
  _$MovieDetailsRequestCopyWithImpl(this._value, this._then);

  final MovieDetailsRequest _value;
  // ignore: unused_field
  final $Res Function(MovieDetailsRequest) _then;

  @override
  $Res call({
    Object? id = freezed,
  }) {
    return _then(_value.copyWith(
      id: id == freezed
          ? _value.id
          : id // ignore: cast_nullable_to_non_nullable
              as int,
    ));
  }
}

/// @nodoc
abstract class _$MovieDetailsRequestCopyWith<$Res>
    implements $MovieDetailsRequestCopyWith<$Res> {
  factory _$MovieDetailsRequestCopyWith(_MovieDetailsRequest value,
          $Res Function(_MovieDetailsRequest) then) =
      __$MovieDetailsRequestCopyWithImpl<$Res>;
  @override
  $Res call(
      {@JsonKey(fromJson: MovieDetailsRequest._stringToInt, toJson: MovieDetailsRequest._stringFromInt)
          int id});
}

/// @nodoc
class __$MovieDetailsRequestCopyWithImpl<$Res>
    extends _$MovieDetailsRequestCopyWithImpl<$Res>
    implements _$MovieDetailsRequestCopyWith<$Res> {
  __$MovieDetailsRequestCopyWithImpl(
      _MovieDetailsRequest _value, $Res Function(_MovieDetailsRequest) _then)
      : super(_value, (v) => _then(v as _MovieDetailsRequest));

  @override
  _MovieDetailsRequest get _value => super._value as _MovieDetailsRequest;

  @override
  $Res call({
    Object? id = freezed,
  }) {
    return _then(_MovieDetailsRequest(
      id: id == freezed
          ? _value.id
          : id // ignore: cast_nullable_to_non_nullable
              as int,
    ));
  }
}

/// @nodoc
@JsonSerializable()
class _$_MovieDetailsRequest implements _MovieDetailsRequest {
  const _$_MovieDetailsRequest(
      {@JsonKey(fromJson: MovieDetailsRequest._stringToInt, toJson: MovieDetailsRequest._stringFromInt)
          required this.id});

  factory _$_MovieDetailsRequest.fromJson(Map<String, dynamic> json) =>
      _$$_MovieDetailsRequestFromJson(json);

  @override
  @JsonKey(
      fromJson: MovieDetailsRequest._stringToInt,
      toJson: MovieDetailsRequest._stringFromInt)
  final int id;

  @override
  String toString() {
    return 'MovieDetailsRequest(id: $id)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _MovieDetailsRequest &&
            const DeepCollectionEquality().equals(other.id, id));
  }

  @override
  int get hashCode =>
      Object.hash(runtimeType, const DeepCollectionEquality().hash(id));

  @JsonKey(ignore: true)
  @override
  _$MovieDetailsRequestCopyWith<_MovieDetailsRequest> get copyWith =>
      __$MovieDetailsRequestCopyWithImpl<_MovieDetailsRequest>(
          this, _$identity);

  @override
  Map<String, dynamic> toJson() {
    return _$$_MovieDetailsRequestToJson(this);
  }
}

abstract class _MovieDetailsRequest implements MovieDetailsRequest {
  const factory _MovieDetailsRequest(
      {@JsonKey(fromJson: MovieDetailsRequest._stringToInt, toJson: MovieDetailsRequest._stringFromInt)
          required int id}) = _$_MovieDetailsRequest;

  factory _MovieDetailsRequest.fromJson(Map<String, dynamic> json) =
      _$_MovieDetailsRequest.fromJson;

  @override
  @JsonKey(
      fromJson: MovieDetailsRequest._stringToInt,
      toJson: MovieDetailsRequest._stringFromInt)
  int get id;
  @override
  @JsonKey(ignore: true)
  _$MovieDetailsRequestCopyWith<_MovieDetailsRequest> get copyWith =>
      throw _privateConstructorUsedError;
}
