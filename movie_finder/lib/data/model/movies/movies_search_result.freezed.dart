// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'movies_search_result.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more informations: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

MoviesSearchResult _$MoviesSearchResultFromJson(Map<String, dynamic> json) {
  return _MoviesSearchResult.fromJson(json);
}

/// @nodoc
class _$MoviesSearchResultTearOff {
  const _$MoviesSearchResultTearOff();

  _MoviesSearchResult call({int? page, List<Movie>? results}) {
    return _MoviesSearchResult(
      page: page,
      results: results,
    );
  }

  MoviesSearchResult fromJson(Map<String, Object?> json) {
    return MoviesSearchResult.fromJson(json);
  }
}

/// @nodoc
const $MoviesSearchResult = _$MoviesSearchResultTearOff();

/// @nodoc
mixin _$MoviesSearchResult {
  int? get page => throw _privateConstructorUsedError;
  List<Movie>? get results => throw _privateConstructorUsedError;

  Map<String, dynamic> toJson() => throw _privateConstructorUsedError;
  @JsonKey(ignore: true)
  $MoviesSearchResultCopyWith<MoviesSearchResult> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MoviesSearchResultCopyWith<$Res> {
  factory $MoviesSearchResultCopyWith(
          MoviesSearchResult value, $Res Function(MoviesSearchResult) then) =
      _$MoviesSearchResultCopyWithImpl<$Res>;
  $Res call({int? page, List<Movie>? results});
}

/// @nodoc
class _$MoviesSearchResultCopyWithImpl<$Res>
    implements $MoviesSearchResultCopyWith<$Res> {
  _$MoviesSearchResultCopyWithImpl(this._value, this._then);

  final MoviesSearchResult _value;
  // ignore: unused_field
  final $Res Function(MoviesSearchResult) _then;

  @override
  $Res call({
    Object? page = freezed,
    Object? results = freezed,
  }) {
    return _then(_value.copyWith(
      page: page == freezed
          ? _value.page
          : page // ignore: cast_nullable_to_non_nullable
              as int?,
      results: results == freezed
          ? _value.results
          : results // ignore: cast_nullable_to_non_nullable
              as List<Movie>?,
    ));
  }
}

/// @nodoc
abstract class _$MoviesSearchResultCopyWith<$Res>
    implements $MoviesSearchResultCopyWith<$Res> {
  factory _$MoviesSearchResultCopyWith(
          _MoviesSearchResult value, $Res Function(_MoviesSearchResult) then) =
      __$MoviesSearchResultCopyWithImpl<$Res>;
  @override
  $Res call({int? page, List<Movie>? results});
}

/// @nodoc
class __$MoviesSearchResultCopyWithImpl<$Res>
    extends _$MoviesSearchResultCopyWithImpl<$Res>
    implements _$MoviesSearchResultCopyWith<$Res> {
  __$MoviesSearchResultCopyWithImpl(
      _MoviesSearchResult _value, $Res Function(_MoviesSearchResult) _then)
      : super(_value, (v) => _then(v as _MoviesSearchResult));

  @override
  _MoviesSearchResult get _value => super._value as _MoviesSearchResult;

  @override
  $Res call({
    Object? page = freezed,
    Object? results = freezed,
  }) {
    return _then(_MoviesSearchResult(
      page: page == freezed
          ? _value.page
          : page // ignore: cast_nullable_to_non_nullable
              as int?,
      results: results == freezed
          ? _value.results
          : results // ignore: cast_nullable_to_non_nullable
              as List<Movie>?,
    ));
  }
}

/// @nodoc
@JsonSerializable()
class _$_MoviesSearchResult implements _MoviesSearchResult {
  const _$_MoviesSearchResult({this.page, this.results});

  factory _$_MoviesSearchResult.fromJson(Map<String, dynamic> json) =>
      _$$_MoviesSearchResultFromJson(json);

  @override
  final int? page;
  @override
  final List<Movie>? results;

  @override
  String toString() {
    return 'MoviesSearchResult(page: $page, results: $results)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _MoviesSearchResult &&
            const DeepCollectionEquality().equals(other.page, page) &&
            const DeepCollectionEquality().equals(other.results, results));
  }

  @override
  int get hashCode => Object.hash(
      runtimeType,
      const DeepCollectionEquality().hash(page),
      const DeepCollectionEquality().hash(results));

  @JsonKey(ignore: true)
  @override
  _$MoviesSearchResultCopyWith<_MoviesSearchResult> get copyWith =>
      __$MoviesSearchResultCopyWithImpl<_MoviesSearchResult>(this, _$identity);

  @override
  Map<String, dynamic> toJson() {
    return _$$_MoviesSearchResultToJson(this);
  }
}

abstract class _MoviesSearchResult implements MoviesSearchResult {
  const factory _MoviesSearchResult({int? page, List<Movie>? results}) =
      _$_MoviesSearchResult;

  factory _MoviesSearchResult.fromJson(Map<String, dynamic> json) =
      _$_MoviesSearchResult.fromJson;

  @override
  int? get page;
  @override
  List<Movie>? get results;
  @override
  @JsonKey(ignore: true)
  _$MoviesSearchResultCopyWith<_MoviesSearchResult> get copyWith =>
      throw _privateConstructorUsedError;
}
