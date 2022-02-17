// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'movie_details_bloc.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more informations: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

/// @nodoc
class _$MovieDetailsEventTearOff {
  const _$MovieDetailsEventTearOff();

  FetchRequested fetchRequested(int id) {
    return FetchRequested(
      id,
    );
  }
}

/// @nodoc
const $MovieDetailsEvent = _$MovieDetailsEventTearOff();

/// @nodoc
mixin _$MovieDetailsEvent {
  int get id => throw _privateConstructorUsedError;

  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function(int id) fetchRequested,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function(int id)? fetchRequested,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function(int id)? fetchRequested,
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

  @JsonKey(ignore: true)
  $MovieDetailsEventCopyWith<MovieDetailsEvent> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MovieDetailsEventCopyWith<$Res> {
  factory $MovieDetailsEventCopyWith(
          MovieDetailsEvent value, $Res Function(MovieDetailsEvent) then) =
      _$MovieDetailsEventCopyWithImpl<$Res>;
  $Res call({int id});
}

/// @nodoc
class _$MovieDetailsEventCopyWithImpl<$Res>
    implements $MovieDetailsEventCopyWith<$Res> {
  _$MovieDetailsEventCopyWithImpl(this._value, this._then);

  final MovieDetailsEvent _value;
  // ignore: unused_field
  final $Res Function(MovieDetailsEvent) _then;

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
abstract class $FetchRequestedCopyWith<$Res>
    implements $MovieDetailsEventCopyWith<$Res> {
  factory $FetchRequestedCopyWith(
          FetchRequested value, $Res Function(FetchRequested) then) =
      _$FetchRequestedCopyWithImpl<$Res>;
  @override
  $Res call({int id});
}

/// @nodoc
class _$FetchRequestedCopyWithImpl<$Res>
    extends _$MovieDetailsEventCopyWithImpl<$Res>
    implements $FetchRequestedCopyWith<$Res> {
  _$FetchRequestedCopyWithImpl(
      FetchRequested _value, $Res Function(FetchRequested) _then)
      : super(_value, (v) => _then(v as FetchRequested));

  @override
  FetchRequested get _value => super._value as FetchRequested;

  @override
  $Res call({
    Object? id = freezed,
  }) {
    return _then(FetchRequested(
      id == freezed
          ? _value.id
          : id // ignore: cast_nullable_to_non_nullable
              as int,
    ));
  }
}

/// @nodoc

class _$FetchRequested implements FetchRequested {
  const _$FetchRequested(this.id);

  @override
  final int id;

  @override
  String toString() {
    return 'MovieDetailsEvent.fetchRequested(id: $id)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is FetchRequested &&
            const DeepCollectionEquality().equals(other.id, id));
  }

  @override
  int get hashCode =>
      Object.hash(runtimeType, const DeepCollectionEquality().hash(id));

  @JsonKey(ignore: true)
  @override
  $FetchRequestedCopyWith<FetchRequested> get copyWith =>
      _$FetchRequestedCopyWithImpl<FetchRequested>(this, _$identity);

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function(int id) fetchRequested,
  }) {
    return fetchRequested(id);
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function(int id)? fetchRequested,
  }) {
    return fetchRequested?.call(id);
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function(int id)? fetchRequested,
    required TResult orElse(),
  }) {
    if (fetchRequested != null) {
      return fetchRequested(id);
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

abstract class FetchRequested implements MovieDetailsEvent {
  const factory FetchRequested(int id) = _$FetchRequested;

  @override
  int get id;
  @override
  @JsonKey(ignore: true)
  $FetchRequestedCopyWith<FetchRequested> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
class _$MovieDetailsStateTearOff {
  const _$MovieDetailsStateTearOff();

  MovieDetailsStateInitial initial() {
    return const MovieDetailsStateInitial();
  }

  MovieDetailsStateLoading loading() {
    return const MovieDetailsStateLoading();
  }

  MovieDetailsStateSuccess success(MovieDetailsResult movieDetailsResult) {
    return MovieDetailsStateSuccess(
      movieDetailsResult,
    );
  }

  MovieDetailsStateError error(Object error) {
    return MovieDetailsStateError(
      error,
    );
  }
}

/// @nodoc
const $MovieDetailsState = _$MovieDetailsStateTearOff();

/// @nodoc
mixin _$MovieDetailsState {
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() initial,
    required TResult Function() loading,
    required TResult Function(MovieDetailsResult movieDetailsResult) success,
    required TResult Function(Object error) error,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(MovieDetailsResult movieDetailsResult)? success,
    TResult Function(Object error)? error,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(MovieDetailsResult movieDetailsResult)? success,
    TResult Function(Object error)? error,
    required TResult orElse(),
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(MovieDetailsStateInitial value) initial,
    required TResult Function(MovieDetailsStateLoading value) loading,
    required TResult Function(MovieDetailsStateSuccess value) success,
    required TResult Function(MovieDetailsStateError value) error,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(MovieDetailsStateInitial value)? initial,
    TResult Function(MovieDetailsStateLoading value)? loading,
    TResult Function(MovieDetailsStateSuccess value)? success,
    TResult Function(MovieDetailsStateError value)? error,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(MovieDetailsStateInitial value)? initial,
    TResult Function(MovieDetailsStateLoading value)? loading,
    TResult Function(MovieDetailsStateSuccess value)? success,
    TResult Function(MovieDetailsStateError value)? error,
    required TResult orElse(),
  }) =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MovieDetailsStateCopyWith<$Res> {
  factory $MovieDetailsStateCopyWith(
          MovieDetailsState value, $Res Function(MovieDetailsState) then) =
      _$MovieDetailsStateCopyWithImpl<$Res>;
}

/// @nodoc
class _$MovieDetailsStateCopyWithImpl<$Res>
    implements $MovieDetailsStateCopyWith<$Res> {
  _$MovieDetailsStateCopyWithImpl(this._value, this._then);

  final MovieDetailsState _value;
  // ignore: unused_field
  final $Res Function(MovieDetailsState) _then;
}

/// @nodoc
abstract class $MovieDetailsStateInitialCopyWith<$Res> {
  factory $MovieDetailsStateInitialCopyWith(MovieDetailsStateInitial value,
          $Res Function(MovieDetailsStateInitial) then) =
      _$MovieDetailsStateInitialCopyWithImpl<$Res>;
}

/// @nodoc
class _$MovieDetailsStateInitialCopyWithImpl<$Res>
    extends _$MovieDetailsStateCopyWithImpl<$Res>
    implements $MovieDetailsStateInitialCopyWith<$Res> {
  _$MovieDetailsStateInitialCopyWithImpl(MovieDetailsStateInitial _value,
      $Res Function(MovieDetailsStateInitial) _then)
      : super(_value, (v) => _then(v as MovieDetailsStateInitial));

  @override
  MovieDetailsStateInitial get _value =>
      super._value as MovieDetailsStateInitial;
}

/// @nodoc

class _$MovieDetailsStateInitial implements MovieDetailsStateInitial {
  const _$MovieDetailsStateInitial();

  @override
  String toString() {
    return 'MovieDetailsState.initial()';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType && other is MovieDetailsStateInitial);
  }

  @override
  int get hashCode => runtimeType.hashCode;

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() initial,
    required TResult Function() loading,
    required TResult Function(MovieDetailsResult movieDetailsResult) success,
    required TResult Function(Object error) error,
  }) {
    return initial();
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(MovieDetailsResult movieDetailsResult)? success,
    TResult Function(Object error)? error,
  }) {
    return initial?.call();
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(MovieDetailsResult movieDetailsResult)? success,
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
    required TResult Function(MovieDetailsStateInitial value) initial,
    required TResult Function(MovieDetailsStateLoading value) loading,
    required TResult Function(MovieDetailsStateSuccess value) success,
    required TResult Function(MovieDetailsStateError value) error,
  }) {
    return initial(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(MovieDetailsStateInitial value)? initial,
    TResult Function(MovieDetailsStateLoading value)? loading,
    TResult Function(MovieDetailsStateSuccess value)? success,
    TResult Function(MovieDetailsStateError value)? error,
  }) {
    return initial?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(MovieDetailsStateInitial value)? initial,
    TResult Function(MovieDetailsStateLoading value)? loading,
    TResult Function(MovieDetailsStateSuccess value)? success,
    TResult Function(MovieDetailsStateError value)? error,
    required TResult orElse(),
  }) {
    if (initial != null) {
      return initial(this);
    }
    return orElse();
  }
}

abstract class MovieDetailsStateInitial implements MovieDetailsState {
  const factory MovieDetailsStateInitial() = _$MovieDetailsStateInitial;
}

/// @nodoc
abstract class $MovieDetailsStateLoadingCopyWith<$Res> {
  factory $MovieDetailsStateLoadingCopyWith(MovieDetailsStateLoading value,
          $Res Function(MovieDetailsStateLoading) then) =
      _$MovieDetailsStateLoadingCopyWithImpl<$Res>;
}

/// @nodoc
class _$MovieDetailsStateLoadingCopyWithImpl<$Res>
    extends _$MovieDetailsStateCopyWithImpl<$Res>
    implements $MovieDetailsStateLoadingCopyWith<$Res> {
  _$MovieDetailsStateLoadingCopyWithImpl(MovieDetailsStateLoading _value,
      $Res Function(MovieDetailsStateLoading) _then)
      : super(_value, (v) => _then(v as MovieDetailsStateLoading));

  @override
  MovieDetailsStateLoading get _value =>
      super._value as MovieDetailsStateLoading;
}

/// @nodoc

class _$MovieDetailsStateLoading implements MovieDetailsStateLoading {
  const _$MovieDetailsStateLoading();

  @override
  String toString() {
    return 'MovieDetailsState.loading()';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType && other is MovieDetailsStateLoading);
  }

  @override
  int get hashCode => runtimeType.hashCode;

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() initial,
    required TResult Function() loading,
    required TResult Function(MovieDetailsResult movieDetailsResult) success,
    required TResult Function(Object error) error,
  }) {
    return loading();
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(MovieDetailsResult movieDetailsResult)? success,
    TResult Function(Object error)? error,
  }) {
    return loading?.call();
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(MovieDetailsResult movieDetailsResult)? success,
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
    required TResult Function(MovieDetailsStateInitial value) initial,
    required TResult Function(MovieDetailsStateLoading value) loading,
    required TResult Function(MovieDetailsStateSuccess value) success,
    required TResult Function(MovieDetailsStateError value) error,
  }) {
    return loading(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(MovieDetailsStateInitial value)? initial,
    TResult Function(MovieDetailsStateLoading value)? loading,
    TResult Function(MovieDetailsStateSuccess value)? success,
    TResult Function(MovieDetailsStateError value)? error,
  }) {
    return loading?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(MovieDetailsStateInitial value)? initial,
    TResult Function(MovieDetailsStateLoading value)? loading,
    TResult Function(MovieDetailsStateSuccess value)? success,
    TResult Function(MovieDetailsStateError value)? error,
    required TResult orElse(),
  }) {
    if (loading != null) {
      return loading(this);
    }
    return orElse();
  }
}

abstract class MovieDetailsStateLoading implements MovieDetailsState {
  const factory MovieDetailsStateLoading() = _$MovieDetailsStateLoading;
}

/// @nodoc
abstract class $MovieDetailsStateSuccessCopyWith<$Res> {
  factory $MovieDetailsStateSuccessCopyWith(MovieDetailsStateSuccess value,
          $Res Function(MovieDetailsStateSuccess) then) =
      _$MovieDetailsStateSuccessCopyWithImpl<$Res>;
  $Res call({MovieDetailsResult movieDetailsResult});

  $MovieDetailsResultCopyWith<$Res> get movieDetailsResult;
}

/// @nodoc
class _$MovieDetailsStateSuccessCopyWithImpl<$Res>
    extends _$MovieDetailsStateCopyWithImpl<$Res>
    implements $MovieDetailsStateSuccessCopyWith<$Res> {
  _$MovieDetailsStateSuccessCopyWithImpl(MovieDetailsStateSuccess _value,
      $Res Function(MovieDetailsStateSuccess) _then)
      : super(_value, (v) => _then(v as MovieDetailsStateSuccess));

  @override
  MovieDetailsStateSuccess get _value =>
      super._value as MovieDetailsStateSuccess;

  @override
  $Res call({
    Object? movieDetailsResult = freezed,
  }) {
    return _then(MovieDetailsStateSuccess(
      movieDetailsResult == freezed
          ? _value.movieDetailsResult
          : movieDetailsResult // ignore: cast_nullable_to_non_nullable
              as MovieDetailsResult,
    ));
  }

  @override
  $MovieDetailsResultCopyWith<$Res> get movieDetailsResult {
    return $MovieDetailsResultCopyWith<$Res>(_value.movieDetailsResult,
        (value) {
      return _then(_value.copyWith(movieDetailsResult: value));
    });
  }
}

/// @nodoc

class _$MovieDetailsStateSuccess implements MovieDetailsStateSuccess {
  const _$MovieDetailsStateSuccess(this.movieDetailsResult);

  @override
  final MovieDetailsResult movieDetailsResult;

  @override
  String toString() {
    return 'MovieDetailsState.success(movieDetailsResult: $movieDetailsResult)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is MovieDetailsStateSuccess &&
            const DeepCollectionEquality()
                .equals(other.movieDetailsResult, movieDetailsResult));
  }

  @override
  int get hashCode => Object.hash(
      runtimeType, const DeepCollectionEquality().hash(movieDetailsResult));

  @JsonKey(ignore: true)
  @override
  $MovieDetailsStateSuccessCopyWith<MovieDetailsStateSuccess> get copyWith =>
      _$MovieDetailsStateSuccessCopyWithImpl<MovieDetailsStateSuccess>(
          this, _$identity);

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() initial,
    required TResult Function() loading,
    required TResult Function(MovieDetailsResult movieDetailsResult) success,
    required TResult Function(Object error) error,
  }) {
    return success(movieDetailsResult);
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(MovieDetailsResult movieDetailsResult)? success,
    TResult Function(Object error)? error,
  }) {
    return success?.call(movieDetailsResult);
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(MovieDetailsResult movieDetailsResult)? success,
    TResult Function(Object error)? error,
    required TResult orElse(),
  }) {
    if (success != null) {
      return success(movieDetailsResult);
    }
    return orElse();
  }

  @override
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(MovieDetailsStateInitial value) initial,
    required TResult Function(MovieDetailsStateLoading value) loading,
    required TResult Function(MovieDetailsStateSuccess value) success,
    required TResult Function(MovieDetailsStateError value) error,
  }) {
    return success(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(MovieDetailsStateInitial value)? initial,
    TResult Function(MovieDetailsStateLoading value)? loading,
    TResult Function(MovieDetailsStateSuccess value)? success,
    TResult Function(MovieDetailsStateError value)? error,
  }) {
    return success?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(MovieDetailsStateInitial value)? initial,
    TResult Function(MovieDetailsStateLoading value)? loading,
    TResult Function(MovieDetailsStateSuccess value)? success,
    TResult Function(MovieDetailsStateError value)? error,
    required TResult orElse(),
  }) {
    if (success != null) {
      return success(this);
    }
    return orElse();
  }
}

abstract class MovieDetailsStateSuccess implements MovieDetailsState {
  const factory MovieDetailsStateSuccess(
      MovieDetailsResult movieDetailsResult) = _$MovieDetailsStateSuccess;

  MovieDetailsResult get movieDetailsResult;
  @JsonKey(ignore: true)
  $MovieDetailsStateSuccessCopyWith<MovieDetailsStateSuccess> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MovieDetailsStateErrorCopyWith<$Res> {
  factory $MovieDetailsStateErrorCopyWith(MovieDetailsStateError value,
          $Res Function(MovieDetailsStateError) then) =
      _$MovieDetailsStateErrorCopyWithImpl<$Res>;
  $Res call({Object error});
}

/// @nodoc
class _$MovieDetailsStateErrorCopyWithImpl<$Res>
    extends _$MovieDetailsStateCopyWithImpl<$Res>
    implements $MovieDetailsStateErrorCopyWith<$Res> {
  _$MovieDetailsStateErrorCopyWithImpl(MovieDetailsStateError _value,
      $Res Function(MovieDetailsStateError) _then)
      : super(_value, (v) => _then(v as MovieDetailsStateError));

  @override
  MovieDetailsStateError get _value => super._value as MovieDetailsStateError;

  @override
  $Res call({
    Object? error = freezed,
  }) {
    return _then(MovieDetailsStateError(
      error == freezed
          ? _value.error
          : error // ignore: cast_nullable_to_non_nullable
              as Object,
    ));
  }
}

/// @nodoc

class _$MovieDetailsStateError implements MovieDetailsStateError {
  const _$MovieDetailsStateError(this.error);

  @override
  final Object error;

  @override
  String toString() {
    return 'MovieDetailsState.error(error: $error)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is MovieDetailsStateError &&
            const DeepCollectionEquality().equals(other.error, error));
  }

  @override
  int get hashCode =>
      Object.hash(runtimeType, const DeepCollectionEquality().hash(error));

  @JsonKey(ignore: true)
  @override
  $MovieDetailsStateErrorCopyWith<MovieDetailsStateError> get copyWith =>
      _$MovieDetailsStateErrorCopyWithImpl<MovieDetailsStateError>(
          this, _$identity);

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() initial,
    required TResult Function() loading,
    required TResult Function(MovieDetailsResult movieDetailsResult) success,
    required TResult Function(Object error) error,
  }) {
    return error(this.error);
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(MovieDetailsResult movieDetailsResult)? success,
    TResult Function(Object error)? error,
  }) {
    return error?.call(this.error);
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? initial,
    TResult Function()? loading,
    TResult Function(MovieDetailsResult movieDetailsResult)? success,
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
    required TResult Function(MovieDetailsStateInitial value) initial,
    required TResult Function(MovieDetailsStateLoading value) loading,
    required TResult Function(MovieDetailsStateSuccess value) success,
    required TResult Function(MovieDetailsStateError value) error,
  }) {
    return error(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(MovieDetailsStateInitial value)? initial,
    TResult Function(MovieDetailsStateLoading value)? loading,
    TResult Function(MovieDetailsStateSuccess value)? success,
    TResult Function(MovieDetailsStateError value)? error,
  }) {
    return error?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(MovieDetailsStateInitial value)? initial,
    TResult Function(MovieDetailsStateLoading value)? loading,
    TResult Function(MovieDetailsStateSuccess value)? success,
    TResult Function(MovieDetailsStateError value)? error,
    required TResult orElse(),
  }) {
    if (error != null) {
      return error(this);
    }
    return orElse();
  }
}

abstract class MovieDetailsStateError implements MovieDetailsState {
  const factory MovieDetailsStateError(Object error) = _$MovieDetailsStateError;

  Object get error;
  @JsonKey(ignore: true)
  $MovieDetailsStateErrorCopyWith<MovieDetailsStateError> get copyWith =>
      throw _privateConstructorUsedError;
}
