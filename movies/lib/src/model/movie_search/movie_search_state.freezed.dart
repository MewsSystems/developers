// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'movie_search_state.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more information: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

/// @nodoc
mixin _$MovieSearchState {
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function(String query, List<Movie> movies, bool isLastPage)
        result,
    required TResult Function(MovieSearchError error) error,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult? Function(String query, List<Movie> movies, bool isLastPage)?
        result,
    TResult? Function(MovieSearchError error)? error,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function(String query, List<Movie> movies, bool isLastPage)? result,
    TResult Function(MovieSearchError error)? error,
    required TResult orElse(),
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(Result value) result,
    required TResult Function(Error value) error,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult? Function(Result value)? result,
    TResult? Function(Error value)? error,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(Result value)? result,
    TResult Function(Error value)? error,
    required TResult orElse(),
  }) =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MovieSearchStateCopyWith<$Res> {
  factory $MovieSearchStateCopyWith(
          MovieSearchState value, $Res Function(MovieSearchState) then) =
      _$MovieSearchStateCopyWithImpl<$Res, MovieSearchState>;
}

/// @nodoc
class _$MovieSearchStateCopyWithImpl<$Res, $Val extends MovieSearchState>
    implements $MovieSearchStateCopyWith<$Res> {
  _$MovieSearchStateCopyWithImpl(this._value, this._then);

  // ignore: unused_field
  final $Val _value;
  // ignore: unused_field
  final $Res Function($Val) _then;
}

/// @nodoc
abstract class _$$ResultCopyWith<$Res> {
  factory _$$ResultCopyWith(_$Result value, $Res Function(_$Result) then) =
      __$$ResultCopyWithImpl<$Res>;
  @useResult
  $Res call({String query, List<Movie> movies, bool isLastPage});
}

/// @nodoc
class __$$ResultCopyWithImpl<$Res>
    extends _$MovieSearchStateCopyWithImpl<$Res, _$Result>
    implements _$$ResultCopyWith<$Res> {
  __$$ResultCopyWithImpl(_$Result _value, $Res Function(_$Result) _then)
      : super(_value, _then);

  @pragma('vm:prefer-inline')
  @override
  $Res call({
    Object? query = null,
    Object? movies = null,
    Object? isLastPage = null,
  }) {
    return _then(_$Result(
      null == query
          ? _value.query
          : query // ignore: cast_nullable_to_non_nullable
              as String,
      null == movies
          ? _value._movies
          : movies // ignore: cast_nullable_to_non_nullable
              as List<Movie>,
      null == isLastPage
          ? _value.isLastPage
          : isLastPage // ignore: cast_nullable_to_non_nullable
              as bool,
    ));
  }
}

/// @nodoc

class _$Result implements Result {
  const _$Result(this.query, final List<Movie> movies, this.isLastPage)
      : _movies = movies;

  @override
  final String query;
  final List<Movie> _movies;
  @override
  List<Movie> get movies {
    // ignore: implicit_dynamic_type
    return EqualUnmodifiableListView(_movies);
  }

  @override
  final bool isLastPage;

  @override
  String toString() {
    return 'MovieSearchState.result(query: $query, movies: $movies, isLastPage: $isLastPage)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _$Result &&
            (identical(other.query, query) || other.query == query) &&
            const DeepCollectionEquality().equals(other._movies, _movies) &&
            (identical(other.isLastPage, isLastPage) ||
                other.isLastPage == isLastPage));
  }

  @override
  int get hashCode => Object.hash(runtimeType, query,
      const DeepCollectionEquality().hash(_movies), isLastPage);

  @JsonKey(ignore: true)
  @override
  @pragma('vm:prefer-inline')
  _$$ResultCopyWith<_$Result> get copyWith =>
      __$$ResultCopyWithImpl<_$Result>(this, _$identity);

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function(String query, List<Movie> movies, bool isLastPage)
        result,
    required TResult Function(MovieSearchError error) error,
  }) {
    return result(query, movies, isLastPage);
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult? Function(String query, List<Movie> movies, bool isLastPage)?
        result,
    TResult? Function(MovieSearchError error)? error,
  }) {
    return result?.call(query, movies, isLastPage);
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function(String query, List<Movie> movies, bool isLastPage)? result,
    TResult Function(MovieSearchError error)? error,
    required TResult orElse(),
  }) {
    if (result != null) {
      return result(query, movies, isLastPage);
    }
    return orElse();
  }

  @override
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(Result value) result,
    required TResult Function(Error value) error,
  }) {
    return result(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult? Function(Result value)? result,
    TResult? Function(Error value)? error,
  }) {
    return result?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(Result value)? result,
    TResult Function(Error value)? error,
    required TResult orElse(),
  }) {
    if (result != null) {
      return result(this);
    }
    return orElse();
  }
}

abstract class Result implements MovieSearchState {
  const factory Result(
          final String query, final List<Movie> movies, final bool isLastPage) =
      _$Result;

  String get query;
  List<Movie> get movies;
  bool get isLastPage;
  @JsonKey(ignore: true)
  _$$ResultCopyWith<_$Result> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class _$$ErrorCopyWith<$Res> {
  factory _$$ErrorCopyWith(_$Error value, $Res Function(_$Error) then) =
      __$$ErrorCopyWithImpl<$Res>;
  @useResult
  $Res call({MovieSearchError error});
}

/// @nodoc
class __$$ErrorCopyWithImpl<$Res>
    extends _$MovieSearchStateCopyWithImpl<$Res, _$Error>
    implements _$$ErrorCopyWith<$Res> {
  __$$ErrorCopyWithImpl(_$Error _value, $Res Function(_$Error) _then)
      : super(_value, _then);

  @pragma('vm:prefer-inline')
  @override
  $Res call({
    Object? error = null,
  }) {
    return _then(_$Error(
      null == error
          ? _value.error
          : error // ignore: cast_nullable_to_non_nullable
              as MovieSearchError,
    ));
  }
}

/// @nodoc

class _$Error implements Error {
  const _$Error(this.error);

  @override
  final MovieSearchError error;

  @override
  String toString() {
    return 'MovieSearchState.error(error: $error)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _$Error &&
            (identical(other.error, error) || other.error == error));
  }

  @override
  int get hashCode => Object.hash(runtimeType, error);

  @JsonKey(ignore: true)
  @override
  @pragma('vm:prefer-inline')
  _$$ErrorCopyWith<_$Error> get copyWith =>
      __$$ErrorCopyWithImpl<_$Error>(this, _$identity);

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function(String query, List<Movie> movies, bool isLastPage)
        result,
    required TResult Function(MovieSearchError error) error,
  }) {
    return error(this.error);
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult? Function(String query, List<Movie> movies, bool isLastPage)?
        result,
    TResult? Function(MovieSearchError error)? error,
  }) {
    return error?.call(this.error);
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function(String query, List<Movie> movies, bool isLastPage)? result,
    TResult Function(MovieSearchError error)? error,
    required TResult orElse(),
  }) {
    if (error != null) {
      return error(this.error);
    }
    return orElse();
  }

  @override
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(Result value) result,
    required TResult Function(Error value) error,
  }) {
    return error(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult? Function(Result value)? result,
    TResult? Function(Error value)? error,
  }) {
    return error?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(Result value)? result,
    TResult Function(Error value)? error,
    required TResult orElse(),
  }) {
    if (error != null) {
      return error(this);
    }
    return orElse();
  }
}

abstract class Error implements MovieSearchState {
  const factory Error(final MovieSearchError error) = _$Error;

  MovieSearchError get error;
  @JsonKey(ignore: true)
  _$$ErrorCopyWith<_$Error> get copyWith => throw _privateConstructorUsedError;
}
