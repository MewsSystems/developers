// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'movies_search_request.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more informations: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

MoviesSearchRequest _$MoviesSearchRequestFromJson(Map<String, dynamic> json) {
  return _MoviesSearchRequest.fromJson(json);
}

/// @nodoc
class _$MoviesSearchRequestTearOff {
  const _$MoviesSearchRequestTearOff();

  _MoviesSearchRequest call(
      {required String query,
      @JsonKey(fromJson: MoviesSearchRequest._stringToInt, toJson: MoviesSearchRequest._stringFromInt)
          int? page}) {
    return _MoviesSearchRequest(
      query: query,
      page: page,
    );
  }

  MoviesSearchRequest fromJson(Map<String, Object?> json) {
    return MoviesSearchRequest.fromJson(json);
  }
}

/// @nodoc
const $MoviesSearchRequest = _$MoviesSearchRequestTearOff();

/// @nodoc
mixin _$MoviesSearchRequest {
  String get query => throw _privateConstructorUsedError;
  @JsonKey(
      fromJson: MoviesSearchRequest._stringToInt,
      toJson: MoviesSearchRequest._stringFromInt)
  int? get page => throw _privateConstructorUsedError;

  Map<String, dynamic> toJson() => throw _privateConstructorUsedError;
  @JsonKey(ignore: true)
  $MoviesSearchRequestCopyWith<MoviesSearchRequest> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MoviesSearchRequestCopyWith<$Res> {
  factory $MoviesSearchRequestCopyWith(
          MoviesSearchRequest value, $Res Function(MoviesSearchRequest) then) =
      _$MoviesSearchRequestCopyWithImpl<$Res>;
  $Res call(
      {String query,
      @JsonKey(fromJson: MoviesSearchRequest._stringToInt, toJson: MoviesSearchRequest._stringFromInt)
          int? page});
}

/// @nodoc
class _$MoviesSearchRequestCopyWithImpl<$Res>
    implements $MoviesSearchRequestCopyWith<$Res> {
  _$MoviesSearchRequestCopyWithImpl(this._value, this._then);

  final MoviesSearchRequest _value;
  // ignore: unused_field
  final $Res Function(MoviesSearchRequest) _then;

  @override
  $Res call({
    Object? query = freezed,
    Object? page = freezed,
  }) {
    return _then(_value.copyWith(
      query: query == freezed
          ? _value.query
          : query // ignore: cast_nullable_to_non_nullable
              as String,
      page: page == freezed
          ? _value.page
          : page // ignore: cast_nullable_to_non_nullable
              as int?,
    ));
  }
}

/// @nodoc
abstract class _$MoviesSearchRequestCopyWith<$Res>
    implements $MoviesSearchRequestCopyWith<$Res> {
  factory _$MoviesSearchRequestCopyWith(_MoviesSearchRequest value,
          $Res Function(_MoviesSearchRequest) then) =
      __$MoviesSearchRequestCopyWithImpl<$Res>;
  @override
  $Res call(
      {String query,
      @JsonKey(fromJson: MoviesSearchRequest._stringToInt, toJson: MoviesSearchRequest._stringFromInt)
          int? page});
}

/// @nodoc
class __$MoviesSearchRequestCopyWithImpl<$Res>
    extends _$MoviesSearchRequestCopyWithImpl<$Res>
    implements _$MoviesSearchRequestCopyWith<$Res> {
  __$MoviesSearchRequestCopyWithImpl(
      _MoviesSearchRequest _value, $Res Function(_MoviesSearchRequest) _then)
      : super(_value, (v) => _then(v as _MoviesSearchRequest));

  @override
  _MoviesSearchRequest get _value => super._value as _MoviesSearchRequest;

  @override
  $Res call({
    Object? query = freezed,
    Object? page = freezed,
  }) {
    return _then(_MoviesSearchRequest(
      query: query == freezed
          ? _value.query
          : query // ignore: cast_nullable_to_non_nullable
              as String,
      page: page == freezed
          ? _value.page
          : page // ignore: cast_nullable_to_non_nullable
              as int?,
    ));
  }
}

/// @nodoc
@JsonSerializable()
class _$_MoviesSearchRequest implements _MoviesSearchRequest {
  const _$_MoviesSearchRequest(
      {required this.query,
      @JsonKey(fromJson: MoviesSearchRequest._stringToInt, toJson: MoviesSearchRequest._stringFromInt)
          this.page});

  factory _$_MoviesSearchRequest.fromJson(Map<String, dynamic> json) =>
      _$$_MoviesSearchRequestFromJson(json);

  @override
  final String query;
  @override
  @JsonKey(
      fromJson: MoviesSearchRequest._stringToInt,
      toJson: MoviesSearchRequest._stringFromInt)
  final int? page;

  @override
  String toString() {
    return 'MoviesSearchRequest(query: $query, page: $page)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _MoviesSearchRequest &&
            const DeepCollectionEquality().equals(other.query, query) &&
            const DeepCollectionEquality().equals(other.page, page));
  }

  @override
  int get hashCode => Object.hash(
      runtimeType,
      const DeepCollectionEquality().hash(query),
      const DeepCollectionEquality().hash(page));

  @JsonKey(ignore: true)
  @override
  _$MoviesSearchRequestCopyWith<_MoviesSearchRequest> get copyWith =>
      __$MoviesSearchRequestCopyWithImpl<_MoviesSearchRequest>(
          this, _$identity);

  @override
  Map<String, dynamic> toJson() {
    return _$$_MoviesSearchRequestToJson(this);
  }
}

abstract class _MoviesSearchRequest implements MoviesSearchRequest {
  const factory _MoviesSearchRequest(
      {required String query,
      @JsonKey(fromJson: MoviesSearchRequest._stringToInt, toJson: MoviesSearchRequest._stringFromInt)
          int? page}) = _$_MoviesSearchRequest;

  factory _MoviesSearchRequest.fromJson(Map<String, dynamic> json) =
      _$_MoviesSearchRequest.fromJson;

  @override
  String get query;
  @override
  @JsonKey(
      fromJson: MoviesSearchRequest._stringToInt,
      toJson: MoviesSearchRequest._stringFromInt)
  int? get page;
  @override
  @JsonKey(ignore: true)
  _$MoviesSearchRequestCopyWith<_MoviesSearchRequest> get copyWith =>
      throw _privateConstructorUsedError;
}
