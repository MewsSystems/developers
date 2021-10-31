// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'movies_list_response.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more informations: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

MoviesListResponse _$MoviesListResponseFromJson(Map<String, dynamic> json) {
  return _MovieListResponse.fromJson(json);
}

/// @nodoc
class _$MoviesListResponseTearOff {
  const _$MoviesListResponseTearOff();

  _MovieListResponse call(
      {required int page,
      @JsonKey(name: 'total_pages') required int totalPages,
      @JsonKey(name: 'total_results') required int totalResults,
      @JsonKey(name: 'results') required List<MovieListItem> items}) {
    return _MovieListResponse(
      page: page,
      totalPages: totalPages,
      totalResults: totalResults,
      items: items,
    );
  }

  MoviesListResponse fromJson(Map<String, Object?> json) {
    return MoviesListResponse.fromJson(json);
  }
}

/// @nodoc
const $MoviesListResponse = _$MoviesListResponseTearOff();

/// @nodoc
mixin _$MoviesListResponse {
  int get page => throw _privateConstructorUsedError;
  @JsonKey(name: 'total_pages')
  int get totalPages => throw _privateConstructorUsedError;
  @JsonKey(name: 'total_results')
  int get totalResults => throw _privateConstructorUsedError;
  @JsonKey(name: 'results')
  List<MovieListItem> get items => throw _privateConstructorUsedError;

  Map<String, dynamic> toJson() => throw _privateConstructorUsedError;
  @JsonKey(ignore: true)
  $MoviesListResponseCopyWith<MoviesListResponse> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MoviesListResponseCopyWith<$Res> {
  factory $MoviesListResponseCopyWith(
          MoviesListResponse value, $Res Function(MoviesListResponse) then) =
      _$MoviesListResponseCopyWithImpl<$Res>;
  $Res call(
      {int page,
      @JsonKey(name: 'total_pages') int totalPages,
      @JsonKey(name: 'total_results') int totalResults,
      @JsonKey(name: 'results') List<MovieListItem> items});
}

/// @nodoc
class _$MoviesListResponseCopyWithImpl<$Res>
    implements $MoviesListResponseCopyWith<$Res> {
  _$MoviesListResponseCopyWithImpl(this._value, this._then);

  final MoviesListResponse _value;
  // ignore: unused_field
  final $Res Function(MoviesListResponse) _then;

  @override
  $Res call({
    Object? page = freezed,
    Object? totalPages = freezed,
    Object? totalResults = freezed,
    Object? items = freezed,
  }) {
    return _then(_value.copyWith(
      page: page == freezed
          ? _value.page
          : page // ignore: cast_nullable_to_non_nullable
              as int,
      totalPages: totalPages == freezed
          ? _value.totalPages
          : totalPages // ignore: cast_nullable_to_non_nullable
              as int,
      totalResults: totalResults == freezed
          ? _value.totalResults
          : totalResults // ignore: cast_nullable_to_non_nullable
              as int,
      items: items == freezed
          ? _value.items
          : items // ignore: cast_nullable_to_non_nullable
              as List<MovieListItem>,
    ));
  }
}

/// @nodoc
abstract class _$MovieListResponseCopyWith<$Res>
    implements $MoviesListResponseCopyWith<$Res> {
  factory _$MovieListResponseCopyWith(
          _MovieListResponse value, $Res Function(_MovieListResponse) then) =
      __$MovieListResponseCopyWithImpl<$Res>;
  @override
  $Res call(
      {int page,
      @JsonKey(name: 'total_pages') int totalPages,
      @JsonKey(name: 'total_results') int totalResults,
      @JsonKey(name: 'results') List<MovieListItem> items});
}

/// @nodoc
class __$MovieListResponseCopyWithImpl<$Res>
    extends _$MoviesListResponseCopyWithImpl<$Res>
    implements _$MovieListResponseCopyWith<$Res> {
  __$MovieListResponseCopyWithImpl(
      _MovieListResponse _value, $Res Function(_MovieListResponse) _then)
      : super(_value, (v) => _then(v as _MovieListResponse));

  @override
  _MovieListResponse get _value => super._value as _MovieListResponse;

  @override
  $Res call({
    Object? page = freezed,
    Object? totalPages = freezed,
    Object? totalResults = freezed,
    Object? items = freezed,
  }) {
    return _then(_MovieListResponse(
      page: page == freezed
          ? _value.page
          : page // ignore: cast_nullable_to_non_nullable
              as int,
      totalPages: totalPages == freezed
          ? _value.totalPages
          : totalPages // ignore: cast_nullable_to_non_nullable
              as int,
      totalResults: totalResults == freezed
          ? _value.totalResults
          : totalResults // ignore: cast_nullable_to_non_nullable
              as int,
      items: items == freezed
          ? _value.items
          : items // ignore: cast_nullable_to_non_nullable
              as List<MovieListItem>,
    ));
  }
}

/// @nodoc
@JsonSerializable()
class _$_MovieListResponse extends _MovieListResponse {
  const _$_MovieListResponse(
      {required this.page,
      @JsonKey(name: 'total_pages') required this.totalPages,
      @JsonKey(name: 'total_results') required this.totalResults,
      @JsonKey(name: 'results') required this.items})
      : super._();

  factory _$_MovieListResponse.fromJson(Map<String, dynamic> json) =>
      _$$_MovieListResponseFromJson(json);

  @override
  final int page;
  @override
  @JsonKey(name: 'total_pages')
  final int totalPages;
  @override
  @JsonKey(name: 'total_results')
  final int totalResults;
  @override
  @JsonKey(name: 'results')
  final List<MovieListItem> items;

  @override
  String toString() {
    return 'MoviesListResponse(page: $page, totalPages: $totalPages, totalResults: $totalResults, items: $items)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _MovieListResponse &&
            (identical(other.page, page) || other.page == page) &&
            (identical(other.totalPages, totalPages) ||
                other.totalPages == totalPages) &&
            (identical(other.totalResults, totalResults) ||
                other.totalResults == totalResults) &&
            const DeepCollectionEquality().equals(other.items, items));
  }

  @override
  int get hashCode => Object.hash(runtimeType, page, totalPages, totalResults,
      const DeepCollectionEquality().hash(items));

  @JsonKey(ignore: true)
  @override
  _$MovieListResponseCopyWith<_MovieListResponse> get copyWith =>
      __$MovieListResponseCopyWithImpl<_MovieListResponse>(this, _$identity);

  @override
  Map<String, dynamic> toJson() {
    return _$$_MovieListResponseToJson(this);
  }
}

abstract class _MovieListResponse extends MoviesListResponse {
  const factory _MovieListResponse(
          {required int page,
          @JsonKey(name: 'total_pages') required int totalPages,
          @JsonKey(name: 'total_results') required int totalResults,
          @JsonKey(name: 'results') required List<MovieListItem> items}) =
      _$_MovieListResponse;
  const _MovieListResponse._() : super._();

  factory _MovieListResponse.fromJson(Map<String, dynamic> json) =
      _$_MovieListResponse.fromJson;

  @override
  int get page;
  @override
  @JsonKey(name: 'total_pages')
  int get totalPages;
  @override
  @JsonKey(name: 'total_results')
  int get totalResults;
  @override
  @JsonKey(name: 'results')
  List<MovieListItem> get items;
  @override
  @JsonKey(ignore: true)
  _$MovieListResponseCopyWith<_MovieListResponse> get copyWith =>
      throw _privateConstructorUsedError;
}
