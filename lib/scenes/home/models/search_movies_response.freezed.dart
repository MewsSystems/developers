// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'search_movies_response.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more information: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

SearchMoviesResponse _$SearchMoviesResponseFromJson(Map<String, dynamic> json) {
  return _SearchMoviesResponse.fromJson(json);
}

/// @nodoc
mixin _$SearchMoviesResponse {
  @JsonKey(name: 'page')
  int get page => throw _privateConstructorUsedError;
  @JsonKey(name: 'total_pages')
  int get totalPages => throw _privateConstructorUsedError;
  @JsonKey(name: 'total_results')
  int get totalResults => throw _privateConstructorUsedError;
  @JsonKey(name: 'results')
  List<Movie> get movies => throw _privateConstructorUsedError;

  Map<String, dynamic> toJson() => throw _privateConstructorUsedError;
  @JsonKey(ignore: true)
  $SearchMoviesResponseCopyWith<SearchMoviesResponse> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $SearchMoviesResponseCopyWith<$Res> {
  factory $SearchMoviesResponseCopyWith(SearchMoviesResponse value,
          $Res Function(SearchMoviesResponse) then) =
      _$SearchMoviesResponseCopyWithImpl<$Res, SearchMoviesResponse>;
  @useResult
  $Res call(
      {@JsonKey(name: 'page') int page,
      @JsonKey(name: 'total_pages') int totalPages,
      @JsonKey(name: 'total_results') int totalResults,
      @JsonKey(name: 'results') List<Movie> movies});
}

/// @nodoc
class _$SearchMoviesResponseCopyWithImpl<$Res,
        $Val extends SearchMoviesResponse>
    implements $SearchMoviesResponseCopyWith<$Res> {
  _$SearchMoviesResponseCopyWithImpl(this._value, this._then);

  // ignore: unused_field
  final $Val _value;
  // ignore: unused_field
  final $Res Function($Val) _then;

  @pragma('vm:prefer-inline')
  @override
  $Res call({
    Object? page = null,
    Object? totalPages = null,
    Object? totalResults = null,
    Object? movies = null,
  }) {
    return _then(_value.copyWith(
      page: null == page
          ? _value.page
          : page // ignore: cast_nullable_to_non_nullable
              as int,
      totalPages: null == totalPages
          ? _value.totalPages
          : totalPages // ignore: cast_nullable_to_non_nullable
              as int,
      totalResults: null == totalResults
          ? _value.totalResults
          : totalResults // ignore: cast_nullable_to_non_nullable
              as int,
      movies: null == movies
          ? _value.movies
          : movies // ignore: cast_nullable_to_non_nullable
              as List<Movie>,
    ) as $Val);
  }
}

/// @nodoc
abstract class _$$_SearchMoviesResponseCopyWith<$Res>
    implements $SearchMoviesResponseCopyWith<$Res> {
  factory _$$_SearchMoviesResponseCopyWith(_$_SearchMoviesResponse value,
          $Res Function(_$_SearchMoviesResponse) then) =
      __$$_SearchMoviesResponseCopyWithImpl<$Res>;
  @override
  @useResult
  $Res call(
      {@JsonKey(name: 'page') int page,
      @JsonKey(name: 'total_pages') int totalPages,
      @JsonKey(name: 'total_results') int totalResults,
      @JsonKey(name: 'results') List<Movie> movies});
}

/// @nodoc
class __$$_SearchMoviesResponseCopyWithImpl<$Res>
    extends _$SearchMoviesResponseCopyWithImpl<$Res, _$_SearchMoviesResponse>
    implements _$$_SearchMoviesResponseCopyWith<$Res> {
  __$$_SearchMoviesResponseCopyWithImpl(_$_SearchMoviesResponse _value,
      $Res Function(_$_SearchMoviesResponse) _then)
      : super(_value, _then);

  @pragma('vm:prefer-inline')
  @override
  $Res call({
    Object? page = null,
    Object? totalPages = null,
    Object? totalResults = null,
    Object? movies = null,
  }) {
    return _then(_$_SearchMoviesResponse(
      page: null == page
          ? _value.page
          : page // ignore: cast_nullable_to_non_nullable
              as int,
      totalPages: null == totalPages
          ? _value.totalPages
          : totalPages // ignore: cast_nullable_to_non_nullable
              as int,
      totalResults: null == totalResults
          ? _value.totalResults
          : totalResults // ignore: cast_nullable_to_non_nullable
              as int,
      movies: null == movies
          ? _value._movies
          : movies // ignore: cast_nullable_to_non_nullable
              as List<Movie>,
    ));
  }
}

/// @nodoc
@JsonSerializable()
class _$_SearchMoviesResponse extends _SearchMoviesResponse {
  _$_SearchMoviesResponse(
      {@JsonKey(name: 'page') required this.page,
      @JsonKey(name: 'total_pages') required this.totalPages,
      @JsonKey(name: 'total_results') required this.totalResults,
      @JsonKey(name: 'results') required final List<Movie> movies})
      : _movies = movies,
        super._();

  factory _$_SearchMoviesResponse.fromJson(Map<String, dynamic> json) =>
      _$$_SearchMoviesResponseFromJson(json);

  @override
  @JsonKey(name: 'page')
  final int page;
  @override
  @JsonKey(name: 'total_pages')
  final int totalPages;
  @override
  @JsonKey(name: 'total_results')
  final int totalResults;
  final List<Movie> _movies;
  @override
  @JsonKey(name: 'results')
  List<Movie> get movies {
    if (_movies is EqualUnmodifiableListView) return _movies;
    // ignore: implicit_dynamic_type
    return EqualUnmodifiableListView(_movies);
  }

  @override
  String toString() {
    return 'SearchMoviesResponse(page: $page, totalPages: $totalPages, totalResults: $totalResults, movies: $movies)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _$_SearchMoviesResponse &&
            (identical(other.page, page) || other.page == page) &&
            (identical(other.totalPages, totalPages) ||
                other.totalPages == totalPages) &&
            (identical(other.totalResults, totalResults) ||
                other.totalResults == totalResults) &&
            const DeepCollectionEquality().equals(other._movies, _movies));
  }

  @JsonKey(ignore: true)
  @override
  int get hashCode => Object.hash(runtimeType, page, totalPages, totalResults,
      const DeepCollectionEquality().hash(_movies));

  @JsonKey(ignore: true)
  @override
  @pragma('vm:prefer-inline')
  _$$_SearchMoviesResponseCopyWith<_$_SearchMoviesResponse> get copyWith =>
      __$$_SearchMoviesResponseCopyWithImpl<_$_SearchMoviesResponse>(
          this, _$identity);

  @override
  Map<String, dynamic> toJson() {
    return _$$_SearchMoviesResponseToJson(
      this,
    );
  }
}

abstract class _SearchMoviesResponse extends SearchMoviesResponse {
  factory _SearchMoviesResponse(
          {@JsonKey(name: 'page') required final int page,
          @JsonKey(name: 'total_pages') required final int totalPages,
          @JsonKey(name: 'total_results') required final int totalResults,
          @JsonKey(name: 'results') required final List<Movie> movies}) =
      _$_SearchMoviesResponse;
  _SearchMoviesResponse._() : super._();

  factory _SearchMoviesResponse.fromJson(Map<String, dynamic> json) =
      _$_SearchMoviesResponse.fromJson;

  @override
  @JsonKey(name: 'page')
  int get page;
  @override
  @JsonKey(name: 'total_pages')
  int get totalPages;
  @override
  @JsonKey(name: 'total_results')
  int get totalResults;
  @override
  @JsonKey(name: 'results')
  List<Movie> get movies;
  @override
  @JsonKey(ignore: true)
  _$$_SearchMoviesResponseCopyWith<_$_SearchMoviesResponse> get copyWith =>
      throw _privateConstructorUsedError;
}
