// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'movie_details_state.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more information: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

/// @nodoc
mixin _$MovieDetailsState {
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() noSelection,
    required TResult Function() loading,
    required TResult Function(MovieDetails movieDetails) details,
    required TResult Function(MovieDetailsError error) error,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult? Function()? noSelection,
    TResult? Function()? loading,
    TResult? Function(MovieDetails movieDetails)? details,
    TResult? Function(MovieDetailsError error)? error,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? noSelection,
    TResult Function()? loading,
    TResult Function(MovieDetails movieDetails)? details,
    TResult Function(MovieDetailsError error)? error,
    required TResult orElse(),
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(NoSelection value) noSelection,
    required TResult Function(Loading value) loading,
    required TResult Function(Details value) details,
    required TResult Function(Error value) error,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult? Function(NoSelection value)? noSelection,
    TResult? Function(Loading value)? loading,
    TResult? Function(Details value)? details,
    TResult? Function(Error value)? error,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(NoSelection value)? noSelection,
    TResult Function(Loading value)? loading,
    TResult Function(Details value)? details,
    TResult Function(Error value)? error,
    required TResult orElse(),
  }) =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MovieDetailsStateCopyWith<$Res> {
  factory $MovieDetailsStateCopyWith(
          MovieDetailsState value, $Res Function(MovieDetailsState) then) =
      _$MovieDetailsStateCopyWithImpl<$Res, MovieDetailsState>;
}

/// @nodoc
class _$MovieDetailsStateCopyWithImpl<$Res, $Val extends MovieDetailsState>
    implements $MovieDetailsStateCopyWith<$Res> {
  _$MovieDetailsStateCopyWithImpl(this._value, this._then);

  // ignore: unused_field
  final $Val _value;
  // ignore: unused_field
  final $Res Function($Val) _then;
}

/// @nodoc
abstract class _$$NoSelectionCopyWith<$Res> {
  factory _$$NoSelectionCopyWith(
          _$NoSelection value, $Res Function(_$NoSelection) then) =
      __$$NoSelectionCopyWithImpl<$Res>;
}

/// @nodoc
class __$$NoSelectionCopyWithImpl<$Res>
    extends _$MovieDetailsStateCopyWithImpl<$Res, _$NoSelection>
    implements _$$NoSelectionCopyWith<$Res> {
  __$$NoSelectionCopyWithImpl(
      _$NoSelection _value, $Res Function(_$NoSelection) _then)
      : super(_value, _then);
}

/// @nodoc

class _$NoSelection implements NoSelection {
  const _$NoSelection();

  @override
  String toString() {
    return 'MovieDetailsState.noSelection()';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType && other is _$NoSelection);
  }

  @override
  int get hashCode => runtimeType.hashCode;

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() noSelection,
    required TResult Function() loading,
    required TResult Function(MovieDetails movieDetails) details,
    required TResult Function(MovieDetailsError error) error,
  }) {
    return noSelection();
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult? Function()? noSelection,
    TResult? Function()? loading,
    TResult? Function(MovieDetails movieDetails)? details,
    TResult? Function(MovieDetailsError error)? error,
  }) {
    return noSelection?.call();
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? noSelection,
    TResult Function()? loading,
    TResult Function(MovieDetails movieDetails)? details,
    TResult Function(MovieDetailsError error)? error,
    required TResult orElse(),
  }) {
    if (noSelection != null) {
      return noSelection();
    }
    return orElse();
  }

  @override
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(NoSelection value) noSelection,
    required TResult Function(Loading value) loading,
    required TResult Function(Details value) details,
    required TResult Function(Error value) error,
  }) {
    return noSelection(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult? Function(NoSelection value)? noSelection,
    TResult? Function(Loading value)? loading,
    TResult? Function(Details value)? details,
    TResult? Function(Error value)? error,
  }) {
    return noSelection?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(NoSelection value)? noSelection,
    TResult Function(Loading value)? loading,
    TResult Function(Details value)? details,
    TResult Function(Error value)? error,
    required TResult orElse(),
  }) {
    if (noSelection != null) {
      return noSelection(this);
    }
    return orElse();
  }
}

abstract class NoSelection implements MovieDetailsState {
  const factory NoSelection() = _$NoSelection;
}

/// @nodoc
abstract class _$$LoadingCopyWith<$Res> {
  factory _$$LoadingCopyWith(_$Loading value, $Res Function(_$Loading) then) =
      __$$LoadingCopyWithImpl<$Res>;
}

/// @nodoc
class __$$LoadingCopyWithImpl<$Res>
    extends _$MovieDetailsStateCopyWithImpl<$Res, _$Loading>
    implements _$$LoadingCopyWith<$Res> {
  __$$LoadingCopyWithImpl(_$Loading _value, $Res Function(_$Loading) _then)
      : super(_value, _then);
}

/// @nodoc

class _$Loading implements Loading {
  const _$Loading();

  @override
  String toString() {
    return 'MovieDetailsState.loading()';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType && other is _$Loading);
  }

  @override
  int get hashCode => runtimeType.hashCode;

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() noSelection,
    required TResult Function() loading,
    required TResult Function(MovieDetails movieDetails) details,
    required TResult Function(MovieDetailsError error) error,
  }) {
    return loading();
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult? Function()? noSelection,
    TResult? Function()? loading,
    TResult? Function(MovieDetails movieDetails)? details,
    TResult? Function(MovieDetailsError error)? error,
  }) {
    return loading?.call();
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? noSelection,
    TResult Function()? loading,
    TResult Function(MovieDetails movieDetails)? details,
    TResult Function(MovieDetailsError error)? error,
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
    required TResult Function(NoSelection value) noSelection,
    required TResult Function(Loading value) loading,
    required TResult Function(Details value) details,
    required TResult Function(Error value) error,
  }) {
    return loading(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult? Function(NoSelection value)? noSelection,
    TResult? Function(Loading value)? loading,
    TResult? Function(Details value)? details,
    TResult? Function(Error value)? error,
  }) {
    return loading?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(NoSelection value)? noSelection,
    TResult Function(Loading value)? loading,
    TResult Function(Details value)? details,
    TResult Function(Error value)? error,
    required TResult orElse(),
  }) {
    if (loading != null) {
      return loading(this);
    }
    return orElse();
  }
}

abstract class Loading implements MovieDetailsState {
  const factory Loading() = _$Loading;
}

/// @nodoc
abstract class _$$DetailsCopyWith<$Res> {
  factory _$$DetailsCopyWith(_$Details value, $Res Function(_$Details) then) =
      __$$DetailsCopyWithImpl<$Res>;
  @useResult
  $Res call({MovieDetails movieDetails});

  $MovieDetailsCopyWith<$Res> get movieDetails;
}

/// @nodoc
class __$$DetailsCopyWithImpl<$Res>
    extends _$MovieDetailsStateCopyWithImpl<$Res, _$Details>
    implements _$$DetailsCopyWith<$Res> {
  __$$DetailsCopyWithImpl(_$Details _value, $Res Function(_$Details) _then)
      : super(_value, _then);

  @pragma('vm:prefer-inline')
  @override
  $Res call({
    Object? movieDetails = null,
  }) {
    return _then(_$Details(
      null == movieDetails
          ? _value.movieDetails
          : movieDetails // ignore: cast_nullable_to_non_nullable
              as MovieDetails,
    ));
  }

  @override
  @pragma('vm:prefer-inline')
  $MovieDetailsCopyWith<$Res> get movieDetails {
    return $MovieDetailsCopyWith<$Res>(_value.movieDetails, (value) {
      return _then(_value.copyWith(movieDetails: value));
    });
  }
}

/// @nodoc

class _$Details implements Details {
  const _$Details(this.movieDetails);

  @override
  final MovieDetails movieDetails;

  @override
  String toString() {
    return 'MovieDetailsState.details(movieDetails: $movieDetails)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _$Details &&
            (identical(other.movieDetails, movieDetails) ||
                other.movieDetails == movieDetails));
  }

  @override
  int get hashCode => Object.hash(runtimeType, movieDetails);

  @JsonKey(ignore: true)
  @override
  @pragma('vm:prefer-inline')
  _$$DetailsCopyWith<_$Details> get copyWith =>
      __$$DetailsCopyWithImpl<_$Details>(this, _$identity);

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function() noSelection,
    required TResult Function() loading,
    required TResult Function(MovieDetails movieDetails) details,
    required TResult Function(MovieDetailsError error) error,
  }) {
    return details(movieDetails);
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult? Function()? noSelection,
    TResult? Function()? loading,
    TResult? Function(MovieDetails movieDetails)? details,
    TResult? Function(MovieDetailsError error)? error,
  }) {
    return details?.call(movieDetails);
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? noSelection,
    TResult Function()? loading,
    TResult Function(MovieDetails movieDetails)? details,
    TResult Function(MovieDetailsError error)? error,
    required TResult orElse(),
  }) {
    if (details != null) {
      return details(movieDetails);
    }
    return orElse();
  }

  @override
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(NoSelection value) noSelection,
    required TResult Function(Loading value) loading,
    required TResult Function(Details value) details,
    required TResult Function(Error value) error,
  }) {
    return details(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult? Function(NoSelection value)? noSelection,
    TResult? Function(Loading value)? loading,
    TResult? Function(Details value)? details,
    TResult? Function(Error value)? error,
  }) {
    return details?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(NoSelection value)? noSelection,
    TResult Function(Loading value)? loading,
    TResult Function(Details value)? details,
    TResult Function(Error value)? error,
    required TResult orElse(),
  }) {
    if (details != null) {
      return details(this);
    }
    return orElse();
  }
}

abstract class Details implements MovieDetailsState {
  const factory Details(final MovieDetails movieDetails) = _$Details;

  MovieDetails get movieDetails;
  @JsonKey(ignore: true)
  _$$DetailsCopyWith<_$Details> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class _$$ErrorCopyWith<$Res> {
  factory _$$ErrorCopyWith(_$Error value, $Res Function(_$Error) then) =
      __$$ErrorCopyWithImpl<$Res>;
  @useResult
  $Res call({MovieDetailsError error});
}

/// @nodoc
class __$$ErrorCopyWithImpl<$Res>
    extends _$MovieDetailsStateCopyWithImpl<$Res, _$Error>
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
              as MovieDetailsError,
    ));
  }
}

/// @nodoc

class _$Error implements Error {
  const _$Error(this.error);

  @override
  final MovieDetailsError error;

  @override
  String toString() {
    return 'MovieDetailsState.error(error: $error)';
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
    required TResult Function() noSelection,
    required TResult Function() loading,
    required TResult Function(MovieDetails movieDetails) details,
    required TResult Function(MovieDetailsError error) error,
  }) {
    return error(this.error);
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult? Function()? noSelection,
    TResult? Function()? loading,
    TResult? Function(MovieDetails movieDetails)? details,
    TResult? Function(MovieDetailsError error)? error,
  }) {
    return error?.call(this.error);
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function()? noSelection,
    TResult Function()? loading,
    TResult Function(MovieDetails movieDetails)? details,
    TResult Function(MovieDetailsError error)? error,
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
    required TResult Function(NoSelection value) noSelection,
    required TResult Function(Loading value) loading,
    required TResult Function(Details value) details,
    required TResult Function(Error value) error,
  }) {
    return error(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult? Function(NoSelection value)? noSelection,
    TResult? Function(Loading value)? loading,
    TResult? Function(Details value)? details,
    TResult? Function(Error value)? error,
  }) {
    return error?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(NoSelection value)? noSelection,
    TResult Function(Loading value)? loading,
    TResult Function(Details value)? details,
    TResult Function(Error value)? error,
    required TResult orElse(),
  }) {
    if (error != null) {
      return error(this);
    }
    return orElse();
  }
}

abstract class Error implements MovieDetailsState {
  const factory Error(final MovieDetailsError error) = _$Error;

  MovieDetailsError get error;
  @JsonKey(ignore: true)
  _$$ErrorCopyWith<_$Error> get copyWith => throw _privateConstructorUsedError;
}
