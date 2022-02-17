// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'movies_search_bloc.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more informations: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

/// @nodoc
class _$MoviesSearchEventTearOff {
  const _$MoviesSearchEventTearOff();

  FetchRequested fetchRequested() {
    return const FetchRequested();
  }
}

/// @nodoc
const $MoviesSearchEvent = _$MoviesSearchEventTearOff();

/// @nodoc
mixin _$MoviesSearchEvent {
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() fetchRequested,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function()? fetchRequested,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? fetchRequested,
    required TResult orElse(),
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(FetchRequested value) fetchRequested,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(FetchRequested value)? fetchRequested,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(FetchRequested value)? fetchRequested,
    required TResult orElse(),
  }) =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MoviesSearchEventCopyWith<$Res> {
  factory $MoviesSearchEventCopyWith(
          MoviesSearchEvent value, $Res Function(MoviesSearchEvent) then) =
      _$MoviesSearchEventCopyWithImpl<$Res>;
}

/// @nodoc
class _$MoviesSearchEventCopyWithImpl<$Res>
    implements $MoviesSearchEventCopyWith<$Res> {
  _$MoviesSearchEventCopyWithImpl(this._value, this._then);

  final MoviesSearchEvent _value;
  // ignore: unused_field
  final $Res Function(MoviesSearchEvent) _then;
}

/// @nodoc
abstract class $FetchRequestedCopyWith<$Res> {
  factory $FetchRequestedCopyWith(
          FetchRequested value, $Res Function(FetchRequested) then) =
      _$FetchRequestedCopyWithImpl<$Res>;
}

/// @nodoc
class _$FetchRequestedCopyWithImpl<$Res>
    extends _$MoviesSearchEventCopyWithImpl<$Res>
    implements $FetchRequestedCopyWith<$Res> {
  _$FetchRequestedCopyWithImpl(
      FetchRequested _value, $Res Function(FetchRequested) _then)
      : super(_value, (v) => _then(v as FetchRequested));

  @override
  FetchRequested get _value => super._value as FetchRequested;
}

/// @nodoc

class _$FetchRequested implements FetchRequested {
  const _$FetchRequested();

  @override
  String toString() {
    return 'MoviesSearchEvent.fetchRequested()';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType && other is FetchRequested);
  }

  @override
  int get hashCode => runtimeType.hashCode;

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() fetchRequested,
  }) {
    return fetchRequested();
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function()? fetchRequested,
  }) {
    return fetchRequested?.call();
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? fetchRequested,
    required TResult orElse(),
  }) {
    if (fetchRequested != null) {
      return fetchRequested();
    }
    return orElse();
  }

  @override
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(FetchRequested value) fetchRequested,
  }) {
    return fetchRequested(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(FetchRequested value)? fetchRequested,
  }) {
    return fetchRequested?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(FetchRequested value)? fetchRequested,
    required TResult orElse(),
  }) {
    if (fetchRequested != null) {
      return fetchRequested(this);
    }
    return orElse();
  }
}

abstract class FetchRequested implements MoviesSearchEvent {
  const factory FetchRequested() = _$FetchRequested;
}

/// @nodoc
class _$MoviesSearchStateTearOff {
  const _$MoviesSearchStateTearOff();

  MoviesSearchStateInitial initial() {
    return const MoviesSearchStateInitial();
  }

  MoviesSearchStateLoading loading() {
    return const MoviesSearchStateLoading();
  }

  MoviesSearchStateSuccess success(List<Movie> movies) {
    return MoviesSearchStateSuccess(
      movies,
    );
  }

  MoviesSearchStateNoMorePages noMorePages(List<Movie> movies) {
    return MoviesSearchStateNoMorePages(
      movies,
    );
  }

  MoviesSearchStateError error(Object error) {
    return MoviesSearchStateError(
      error,
    );
  }
}

/// @nodoc
const $MoviesSearchState = _$MoviesSearchStateTearOff();

/// @nodoc
mixin _$MoviesSearchState {
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() initial,
    required TResult Function() loading,
    required TResult Function(List<Movie> movies) success,
    required TResult Function(List<Movie> movies) noMorePages,
    required TResult Function(Object error) error,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(List<Movie> movies)? success,
    TResult Function(List<Movie> movies)? noMorePages,
    TResult Function(Object error)? error,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(List<Movie> movies)? success,
    TResult Function(List<Movie> movies)? noMorePages,
    TResult Function(Object error)? error,
    required TResult orElse(),
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(MoviesSearchStateInitial value) initial,
    required TResult Function(MoviesSearchStateLoading value) loading,
    required TResult Function(MoviesSearchStateSuccess value) success,
    required TResult Function(MoviesSearchStateNoMorePages value) noMorePages,
    required TResult Function(MoviesSearchStateError value) error,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(MoviesSearchStateInitial value)? initial,
    TResult Function(MoviesSearchStateLoading value)? loading,
    TResult Function(MoviesSearchStateSuccess value)? success,
    TResult Function(MoviesSearchStateNoMorePages value)? noMorePages,
    TResult Function(MoviesSearchStateError value)? error,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(MoviesSearchStateInitial value)? initial,
    TResult Function(MoviesSearchStateLoading value)? loading,
    TResult Function(MoviesSearchStateSuccess value)? success,
    TResult Function(MoviesSearchStateNoMorePages value)? noMorePages,
    TResult Function(MoviesSearchStateError value)? error,
    required TResult orElse(),
  }) =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MoviesSearchStateCopyWith<$Res> {
  factory $MoviesSearchStateCopyWith(
          MoviesSearchState value, $Res Function(MoviesSearchState) then) =
      _$MoviesSearchStateCopyWithImpl<$Res>;
}

/// @nodoc
class _$MoviesSearchStateCopyWithImpl<$Res>
    implements $MoviesSearchStateCopyWith<$Res> {
  _$MoviesSearchStateCopyWithImpl(this._value, this._then);

  final MoviesSearchState _value;
  // ignore: unused_field
  final $Res Function(MoviesSearchState) _then;
}

/// @nodoc
abstract class $MoviesSearchStateInitialCopyWith<$Res> {
  factory $MoviesSearchStateInitialCopyWith(MoviesSearchStateInitial value,
          $Res Function(MoviesSearchStateInitial) then) =
      _$MoviesSearchStateInitialCopyWithImpl<$Res>;
}

/// @nodoc
class _$MoviesSearchStateInitialCopyWithImpl<$Res>
    extends _$MoviesSearchStateCopyWithImpl<$Res>
    implements $MoviesSearchStateInitialCopyWith<$Res> {
  _$MoviesSearchStateInitialCopyWithImpl(MoviesSearchStateInitial _value,
      $Res Function(MoviesSearchStateInitial) _then)
      : super(_value, (v) => _then(v as MoviesSearchStateInitial));

  @override
  MoviesSearchStateInitial get _value =>
      super._value as MoviesSearchStateInitial;
}

/// @nodoc

class _$MoviesSearchStateInitial implements MoviesSearchStateInitial {
  const _$MoviesSearchStateInitial();

  @override
  String toString() {
    return 'MoviesSearchState.initial()';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType && other is MoviesSearchStateInitial);
  }

  @override
  int get hashCode => runtimeType.hashCode;

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() initial,
    required TResult Function() loading,
    required TResult Function(List<Movie> movies) success,
    required TResult Function(List<Movie> movies) noMorePages,
    required TResult Function(Object error) error,
  }) {
    return initial();
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(List<Movie> movies)? success,
    TResult Function(List<Movie> movies)? noMorePages,
    TResult Function(Object error)? error,
  }) {
    return initial?.call();
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(List<Movie> movies)? success,
    TResult Function(List<Movie> movies)? noMorePages,
    TResult Function(Object error)? error,
    required TResult orElse(),
  }) {
    if (initial != null) {
      return initial();
    }
    return orElse();
  }

  @override
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(MoviesSearchStateInitial value) initial,
    required TResult Function(MoviesSearchStateLoading value) loading,
    required TResult Function(MoviesSearchStateSuccess value) success,
    required TResult Function(MoviesSearchStateNoMorePages value) noMorePages,
    required TResult Function(MoviesSearchStateError value) error,
  }) {
    return initial(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(MoviesSearchStateInitial value)? initial,
    TResult Function(MoviesSearchStateLoading value)? loading,
    TResult Function(MoviesSearchStateSuccess value)? success,
    TResult Function(MoviesSearchStateNoMorePages value)? noMorePages,
    TResult Function(MoviesSearchStateError value)? error,
  }) {
    return initial?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(MoviesSearchStateInitial value)? initial,
    TResult Function(MoviesSearchStateLoading value)? loading,
    TResult Function(MoviesSearchStateSuccess value)? success,
    TResult Function(MoviesSearchStateNoMorePages value)? noMorePages,
    TResult Function(MoviesSearchStateError value)? error,
    required TResult orElse(),
  }) {
    if (initial != null) {
      return initial(this);
    }
    return orElse();
  }
}

abstract class MoviesSearchStateInitial implements MoviesSearchState {
  const factory MoviesSearchStateInitial() = _$MoviesSearchStateInitial;
}

/// @nodoc
abstract class $MoviesSearchStateLoadingCopyWith<$Res> {
  factory $MoviesSearchStateLoadingCopyWith(MoviesSearchStateLoading value,
          $Res Function(MoviesSearchStateLoading) then) =
      _$MoviesSearchStateLoadingCopyWithImpl<$Res>;
}

/// @nodoc
class _$MoviesSearchStateLoadingCopyWithImpl<$Res>
    extends _$MoviesSearchStateCopyWithImpl<$Res>
    implements $MoviesSearchStateLoadingCopyWith<$Res> {
  _$MoviesSearchStateLoadingCopyWithImpl(MoviesSearchStateLoading _value,
      $Res Function(MoviesSearchStateLoading) _then)
      : super(_value, (v) => _then(v as MoviesSearchStateLoading));

  @override
  MoviesSearchStateLoading get _value =>
      super._value as MoviesSearchStateLoading;
}

/// @nodoc

class _$MoviesSearchStateLoading implements MoviesSearchStateLoading {
  const _$MoviesSearchStateLoading();

  @override
  String toString() {
    return 'MoviesSearchState.loading()';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType && other is MoviesSearchStateLoading);
  }

  @override
  int get hashCode => runtimeType.hashCode;

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() initial,
    required TResult Function() loading,
    required TResult Function(List<Movie> movies) success,
    required TResult Function(List<Movie> movies) noMorePages,
    required TResult Function(Object error) error,
  }) {
    return loading();
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(List<Movie> movies)? success,
    TResult Function(List<Movie> movies)? noMorePages,
    TResult Function(Object error)? error,
  }) {
    return loading?.call();
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(List<Movie> movies)? success,
    TResult Function(List<Movie> movies)? noMorePages,
    TResult Function(Object error)? error,
    required TResult orElse(),
  }) {
    if (loading != null) {
      return loading();
    }
    return orElse();
  }

  @override
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(MoviesSearchStateInitial value) initial,
    required TResult Function(MoviesSearchStateLoading value) loading,
    required TResult Function(MoviesSearchStateSuccess value) success,
    required TResult Function(MoviesSearchStateNoMorePages value) noMorePages,
    required TResult Function(MoviesSearchStateError value) error,
  }) {
    return loading(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(MoviesSearchStateInitial value)? initial,
    TResult Function(MoviesSearchStateLoading value)? loading,
    TResult Function(MoviesSearchStateSuccess value)? success,
    TResult Function(MoviesSearchStateNoMorePages value)? noMorePages,
    TResult Function(MoviesSearchStateError value)? error,
  }) {
    return loading?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(MoviesSearchStateInitial value)? initial,
    TResult Function(MoviesSearchStateLoading value)? loading,
    TResult Function(MoviesSearchStateSuccess value)? success,
    TResult Function(MoviesSearchStateNoMorePages value)? noMorePages,
    TResult Function(MoviesSearchStateError value)? error,
    required TResult orElse(),
  }) {
    if (loading != null) {
      return loading(this);
    }
    return orElse();
  }
}

abstract class MoviesSearchStateLoading implements MoviesSearchState {
  const factory MoviesSearchStateLoading() = _$MoviesSearchStateLoading;
}

/// @nodoc
abstract class $MoviesSearchStateSuccessCopyWith<$Res> {
  factory $MoviesSearchStateSuccessCopyWith(MoviesSearchStateSuccess value,
          $Res Function(MoviesSearchStateSuccess) then) =
      _$MoviesSearchStateSuccessCopyWithImpl<$Res>;
  $Res call({List<Movie> movies});
}

/// @nodoc
class _$MoviesSearchStateSuccessCopyWithImpl<$Res>
    extends _$MoviesSearchStateCopyWithImpl<$Res>
    implements $MoviesSearchStateSuccessCopyWith<$Res> {
  _$MoviesSearchStateSuccessCopyWithImpl(MoviesSearchStateSuccess _value,
      $Res Function(MoviesSearchStateSuccess) _then)
      : super(_value, (v) => _then(v as MoviesSearchStateSuccess));

  @override
  MoviesSearchStateSuccess get _value =>
      super._value as MoviesSearchStateSuccess;

  @override
  $Res call({
    Object? movies = freezed,
  }) {
    return _then(MoviesSearchStateSuccess(
      movies == freezed
          ? _value.movies
          : movies // ignore: cast_nullable_to_non_nullable
              as List<Movie>,
    ));
  }
}

/// @nodoc

class _$MoviesSearchStateSuccess implements MoviesSearchStateSuccess {
  const _$MoviesSearchStateSuccess(this.movies);

  @override
  final List<Movie> movies;

  @override
  String toString() {
    return 'MoviesSearchState.success(movies: $movies)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is MoviesSearchStateSuccess &&
            const DeepCollectionEquality().equals(other.movies, movies));
  }

  @override
  int get hashCode =>
      Object.hash(runtimeType, const DeepCollectionEquality().hash(movies));

  @JsonKey(ignore: true)
  @override
  $MoviesSearchStateSuccessCopyWith<MoviesSearchStateSuccess> get copyWith =>
      _$MoviesSearchStateSuccessCopyWithImpl<MoviesSearchStateSuccess>(
          this, _$identity);

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() initial,
    required TResult Function() loading,
    required TResult Function(List<Movie> movies) success,
    required TResult Function(List<Movie> movies) noMorePages,
    required TResult Function(Object error) error,
  }) {
    return success(movies);
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(List<Movie> movies)? success,
    TResult Function(List<Movie> movies)? noMorePages,
    TResult Function(Object error)? error,
  }) {
    return success?.call(movies);
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(List<Movie> movies)? success,
    TResult Function(List<Movie> movies)? noMorePages,
    TResult Function(Object error)? error,
    required TResult orElse(),
  }) {
    if (success != null) {
      return success(movies);
    }
    return orElse();
  }

  @override
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(MoviesSearchStateInitial value) initial,
    required TResult Function(MoviesSearchStateLoading value) loading,
    required TResult Function(MoviesSearchStateSuccess value) success,
    required TResult Function(MoviesSearchStateNoMorePages value) noMorePages,
    required TResult Function(MoviesSearchStateError value) error,
  }) {
    return success(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(MoviesSearchStateInitial value)? initial,
    TResult Function(MoviesSearchStateLoading value)? loading,
    TResult Function(MoviesSearchStateSuccess value)? success,
    TResult Function(MoviesSearchStateNoMorePages value)? noMorePages,
    TResult Function(MoviesSearchStateError value)? error,
  }) {
    return success?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(MoviesSearchStateInitial value)? initial,
    TResult Function(MoviesSearchStateLoading value)? loading,
    TResult Function(MoviesSearchStateSuccess value)? success,
    TResult Function(MoviesSearchStateNoMorePages value)? noMorePages,
    TResult Function(MoviesSearchStateError value)? error,
    required TResult orElse(),
  }) {
    if (success != null) {
      return success(this);
    }
    return orElse();
  }
}

abstract class MoviesSearchStateSuccess implements MoviesSearchState {
  const factory MoviesSearchStateSuccess(List<Movie> movies) =
      _$MoviesSearchStateSuccess;

  List<Movie> get movies;
  @JsonKey(ignore: true)
  $MoviesSearchStateSuccessCopyWith<MoviesSearchStateSuccess> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MoviesSearchStateNoMorePagesCopyWith<$Res> {
  factory $MoviesSearchStateNoMorePagesCopyWith(
          MoviesSearchStateNoMorePages value,
          $Res Function(MoviesSearchStateNoMorePages) then) =
      _$MoviesSearchStateNoMorePagesCopyWithImpl<$Res>;
  $Res call({List<Movie> movies});
}

/// @nodoc
class _$MoviesSearchStateNoMorePagesCopyWithImpl<$Res>
    extends _$MoviesSearchStateCopyWithImpl<$Res>
    implements $MoviesSearchStateNoMorePagesCopyWith<$Res> {
  _$MoviesSearchStateNoMorePagesCopyWithImpl(
      MoviesSearchStateNoMorePages _value,
      $Res Function(MoviesSearchStateNoMorePages) _then)
      : super(_value, (v) => _then(v as MoviesSearchStateNoMorePages));

  @override
  MoviesSearchStateNoMorePages get _value =>
      super._value as MoviesSearchStateNoMorePages;

  @override
  $Res call({
    Object? movies = freezed,
  }) {
    return _then(MoviesSearchStateNoMorePages(
      movies == freezed
          ? _value.movies
          : movies // ignore: cast_nullable_to_non_nullable
              as List<Movie>,
    ));
  }
}

/// @nodoc

class _$MoviesSearchStateNoMorePages implements MoviesSearchStateNoMorePages {
  const _$MoviesSearchStateNoMorePages(this.movies);

  @override
  final List<Movie> movies;

  @override
  String toString() {
    return 'MoviesSearchState.noMorePages(movies: $movies)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is MoviesSearchStateNoMorePages &&
            const DeepCollectionEquality().equals(other.movies, movies));
  }

  @override
  int get hashCode =>
      Object.hash(runtimeType, const DeepCollectionEquality().hash(movies));

  @JsonKey(ignore: true)
  @override
  $MoviesSearchStateNoMorePagesCopyWith<MoviesSearchStateNoMorePages>
      get copyWith => _$MoviesSearchStateNoMorePagesCopyWithImpl<
          MoviesSearchStateNoMorePages>(this, _$identity);

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() initial,
    required TResult Function() loading,
    required TResult Function(List<Movie> movies) success,
    required TResult Function(List<Movie> movies) noMorePages,
    required TResult Function(Object error) error,
  }) {
    return noMorePages(movies);
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(List<Movie> movies)? success,
    TResult Function(List<Movie> movies)? noMorePages,
    TResult Function(Object error)? error,
  }) {
    return noMorePages?.call(movies);
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(List<Movie> movies)? success,
    TResult Function(List<Movie> movies)? noMorePages,
    TResult Function(Object error)? error,
    required TResult orElse(),
  }) {
    if (noMorePages != null) {
      return noMorePages(movies);
    }
    return orElse();
  }

  @override
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(MoviesSearchStateInitial value) initial,
    required TResult Function(MoviesSearchStateLoading value) loading,
    required TResult Function(MoviesSearchStateSuccess value) success,
    required TResult Function(MoviesSearchStateNoMorePages value) noMorePages,
    required TResult Function(MoviesSearchStateError value) error,
  }) {
    return noMorePages(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(MoviesSearchStateInitial value)? initial,
    TResult Function(MoviesSearchStateLoading value)? loading,
    TResult Function(MoviesSearchStateSuccess value)? success,
    TResult Function(MoviesSearchStateNoMorePages value)? noMorePages,
    TResult Function(MoviesSearchStateError value)? error,
  }) {
    return noMorePages?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(MoviesSearchStateInitial value)? initial,
    TResult Function(MoviesSearchStateLoading value)? loading,
    TResult Function(MoviesSearchStateSuccess value)? success,
    TResult Function(MoviesSearchStateNoMorePages value)? noMorePages,
    TResult Function(MoviesSearchStateError value)? error,
    required TResult orElse(),
  }) {
    if (noMorePages != null) {
      return noMorePages(this);
    }
    return orElse();
  }
}

abstract class MoviesSearchStateNoMorePages implements MoviesSearchState {
  const factory MoviesSearchStateNoMorePages(List<Movie> movies) =
      _$MoviesSearchStateNoMorePages;

  List<Movie> get movies;
  @JsonKey(ignore: true)
  $MoviesSearchStateNoMorePagesCopyWith<MoviesSearchStateNoMorePages>
      get copyWith => throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MoviesSearchStateErrorCopyWith<$Res> {
  factory $MoviesSearchStateErrorCopyWith(MoviesSearchStateError value,
          $Res Function(MoviesSearchStateError) then) =
      _$MoviesSearchStateErrorCopyWithImpl<$Res>;
  $Res call({Object error});
}

/// @nodoc
class _$MoviesSearchStateErrorCopyWithImpl<$Res>
    extends _$MoviesSearchStateCopyWithImpl<$Res>
    implements $MoviesSearchStateErrorCopyWith<$Res> {
  _$MoviesSearchStateErrorCopyWithImpl(MoviesSearchStateError _value,
      $Res Function(MoviesSearchStateError) _then)
      : super(_value, (v) => _then(v as MoviesSearchStateError));

  @override
  MoviesSearchStateError get _value => super._value as MoviesSearchStateError;

  @override
  $Res call({
    Object? error = freezed,
  }) {
    return _then(MoviesSearchStateError(
      error == freezed
          ? _value.error
          : error // ignore: cast_nullable_to_non_nullable
              as Object,
    ));
  }
}

/// @nodoc

class _$MoviesSearchStateError implements MoviesSearchStateError {
  const _$MoviesSearchStateError(this.error);

  @override
  final Object error;

  @override
  String toString() {
    return 'MoviesSearchState.error(error: $error)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is MoviesSearchStateError &&
            const DeepCollectionEquality().equals(other.error, error));
  }

  @override
  int get hashCode =>
      Object.hash(runtimeType, const DeepCollectionEquality().hash(error));

  @JsonKey(ignore: true)
  @override
  $MoviesSearchStateErrorCopyWith<MoviesSearchStateError> get copyWith =>
      _$MoviesSearchStateErrorCopyWithImpl<MoviesSearchStateError>(
          this, _$identity);

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() initial,
    required TResult Function() loading,
    required TResult Function(List<Movie> movies) success,
    required TResult Function(List<Movie> movies) noMorePages,
    required TResult Function(Object error) error,
  }) {
    return error(this.error);
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(List<Movie> movies)? success,
    TResult Function(List<Movie> movies)? noMorePages,
    TResult Function(Object error)? error,
  }) {
    return error?.call(this.error);
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(List<Movie> movies)? success,
    TResult Function(List<Movie> movies)? noMorePages,
    TResult Function(Object error)? error,
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
    required TResult Function(MoviesSearchStateInitial value) initial,
    required TResult Function(MoviesSearchStateLoading value) loading,
    required TResult Function(MoviesSearchStateSuccess value) success,
    required TResult Function(MoviesSearchStateNoMorePages value) noMorePages,
    required TResult Function(MoviesSearchStateError value) error,
  }) {
    return error(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(MoviesSearchStateInitial value)? initial,
    TResult Function(MoviesSearchStateLoading value)? loading,
    TResult Function(MoviesSearchStateSuccess value)? success,
    TResult Function(MoviesSearchStateNoMorePages value)? noMorePages,
    TResult Function(MoviesSearchStateError value)? error,
  }) {
    return error?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(MoviesSearchStateInitial value)? initial,
    TResult Function(MoviesSearchStateLoading value)? loading,
    TResult Function(MoviesSearchStateSuccess value)? success,
    TResult Function(MoviesSearchStateNoMorePages value)? noMorePages,
    TResult Function(MoviesSearchStateError value)? error,
    required TResult orElse(),
  }) {
    if (error != null) {
      return error(this);
    }
    return orElse();
  }
}

abstract class MoviesSearchStateError implements MoviesSearchState {
  const factory MoviesSearchStateError(Object error) = _$MoviesSearchStateError;

  Object get error;
  @JsonKey(ignore: true)
  $MoviesSearchStateErrorCopyWith<MoviesSearchStateError> get copyWith =>
      throw _privateConstructorUsedError;
}
