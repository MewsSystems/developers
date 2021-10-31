// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'list_cubit.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more informations: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

/// @nodoc
class _$ListStateTearOff {
  const _$ListStateTearOff();

  Initial initial({List<MovieListItem> items = const []}) {
    return Initial(
      items: items,
    );
  }

  Loading loading({List<MovieListItem> items = const []}) {
    return Loading(
      items: items,
    );
  }

  LoadedData loadedData(
      {required List<MovieListItem> items, required int pageNumber}) {
    return LoadedData(
      items: items,
      pageNumber: pageNumber,
    );
  }

  Error error(
      {required List<MovieListItem> items, required String errorMessage}) {
    return Error(
      items: items,
      errorMessage: errorMessage,
    );
  }

  AllDataLoaded allDataLoaded({required List<MovieListItem> items}) {
    return AllDataLoaded(
      items: items,
    );
  }
}

/// @nodoc
const $ListState = _$ListStateTearOff();

/// @nodoc
mixin _$ListState {
  List<MovieListItem> get items => throw _privateConstructorUsedError;

  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function(List<MovieListItem> items) initial,
    required TResult Function(List<MovieListItem> items) loading,
    required TResult Function(List<MovieListItem> items, int pageNumber)
        loadedData,
    required TResult Function(List<MovieListItem> items, String errorMessage)
        error,
    required TResult Function(List<MovieListItem> items) allDataLoaded,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function(List<MovieListItem> items)? initial,
    TResult Function(List<MovieListItem> items)? loading,
    TResult Function(List<MovieListItem> items, int pageNumber)? loadedData,
    TResult Function(List<MovieListItem> items, String errorMessage)? error,
    TResult Function(List<MovieListItem> items)? allDataLoaded,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function(List<MovieListItem> items)? initial,
    TResult Function(List<MovieListItem> items)? loading,
    TResult Function(List<MovieListItem> items, int pageNumber)? loadedData,
    TResult Function(List<MovieListItem> items, String errorMessage)? error,
    TResult Function(List<MovieListItem> items)? allDataLoaded,
    required TResult orElse(),
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(Initial value) initial,
    required TResult Function(Loading value) loading,
    required TResult Function(LoadedData value) loadedData,
    required TResult Function(Error value) error,
    required TResult Function(AllDataLoaded value) allDataLoaded,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(Initial value)? initial,
    TResult Function(Loading value)? loading,
    TResult Function(LoadedData value)? loadedData,
    TResult Function(Error value)? error,
    TResult Function(AllDataLoaded value)? allDataLoaded,
  }) =>
      throw _privateConstructorUsedError;
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(Initial value)? initial,
    TResult Function(Loading value)? loading,
    TResult Function(LoadedData value)? loadedData,
    TResult Function(Error value)? error,
    TResult Function(AllDataLoaded value)? allDataLoaded,
    required TResult orElse(),
  }) =>
      throw _privateConstructorUsedError;

  @JsonKey(ignore: true)
  $ListStateCopyWith<ListState> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $ListStateCopyWith<$Res> {
  factory $ListStateCopyWith(ListState value, $Res Function(ListState) then) =
      _$ListStateCopyWithImpl<$Res>;
  $Res call({List<MovieListItem> items});
}

/// @nodoc
class _$ListStateCopyWithImpl<$Res> implements $ListStateCopyWith<$Res> {
  _$ListStateCopyWithImpl(this._value, this._then);

  final ListState _value;
  // ignore: unused_field
  final $Res Function(ListState) _then;

  @override
  $Res call({
    Object? items = freezed,
  }) {
    return _then(_value.copyWith(
      items: items == freezed
          ? _value.items
          : items // ignore: cast_nullable_to_non_nullable
              as List<MovieListItem>,
    ));
  }
}

/// @nodoc
abstract class $InitialCopyWith<$Res> implements $ListStateCopyWith<$Res> {
  factory $InitialCopyWith(Initial value, $Res Function(Initial) then) =
      _$InitialCopyWithImpl<$Res>;
  @override
  $Res call({List<MovieListItem> items});
}

/// @nodoc
class _$InitialCopyWithImpl<$Res> extends _$ListStateCopyWithImpl<$Res>
    implements $InitialCopyWith<$Res> {
  _$InitialCopyWithImpl(Initial _value, $Res Function(Initial) _then)
      : super(_value, (v) => _then(v as Initial));

  @override
  Initial get _value => super._value as Initial;

  @override
  $Res call({
    Object? items = freezed,
  }) {
    return _then(Initial(
      items: items == freezed
          ? _value.items
          : items // ignore: cast_nullable_to_non_nullable
              as List<MovieListItem>,
    ));
  }
}

/// @nodoc

class _$Initial with DiagnosticableTreeMixin implements Initial {
  const _$Initial({this.items = const []});

  @JsonKey(defaultValue: const [])
  @override
  final List<MovieListItem> items;

  @override
  String toString({DiagnosticLevel minLevel = DiagnosticLevel.info}) {
    return 'ListState.initial(items: $items)';
  }

  @override
  void debugFillProperties(DiagnosticPropertiesBuilder properties) {
    super.debugFillProperties(properties);
    properties
      ..add(DiagnosticsProperty('type', 'ListState.initial'))
      ..add(DiagnosticsProperty('items', items));
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is Initial &&
            const DeepCollectionEquality().equals(other.items, items));
  }

  @override
  int get hashCode =>
      Object.hash(runtimeType, const DeepCollectionEquality().hash(items));

  @JsonKey(ignore: true)
  @override
  $InitialCopyWith<Initial> get copyWith =>
      _$InitialCopyWithImpl<Initial>(this, _$identity);

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function(List<MovieListItem> items) initial,
    required TResult Function(List<MovieListItem> items) loading,
    required TResult Function(List<MovieListItem> items, int pageNumber)
        loadedData,
    required TResult Function(List<MovieListItem> items, String errorMessage)
        error,
    required TResult Function(List<MovieListItem> items) allDataLoaded,
  }) {
    return initial(items);
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function(List<MovieListItem> items)? initial,
    TResult Function(List<MovieListItem> items)? loading,
    TResult Function(List<MovieListItem> items, int pageNumber)? loadedData,
    TResult Function(List<MovieListItem> items, String errorMessage)? error,
    TResult Function(List<MovieListItem> items)? allDataLoaded,
  }) {
    return initial?.call(items);
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function(List<MovieListItem> items)? initial,
    TResult Function(List<MovieListItem> items)? loading,
    TResult Function(List<MovieListItem> items, int pageNumber)? loadedData,
    TResult Function(List<MovieListItem> items, String errorMessage)? error,
    TResult Function(List<MovieListItem> items)? allDataLoaded,
    required TResult orElse(),
  }) {
    if (initial != null) {
      return initial(items);
    }
    return orElse();
  }

  @override
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(Initial value) initial,
    required TResult Function(Loading value) loading,
    required TResult Function(LoadedData value) loadedData,
    required TResult Function(Error value) error,
    required TResult Function(AllDataLoaded value) allDataLoaded,
  }) {
    return initial(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(Initial value)? initial,
    TResult Function(Loading value)? loading,
    TResult Function(LoadedData value)? loadedData,
    TResult Function(Error value)? error,
    TResult Function(AllDataLoaded value)? allDataLoaded,
  }) {
    return initial?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(Initial value)? initial,
    TResult Function(Loading value)? loading,
    TResult Function(LoadedData value)? loadedData,
    TResult Function(Error value)? error,
    TResult Function(AllDataLoaded value)? allDataLoaded,
    required TResult orElse(),
  }) {
    if (initial != null) {
      return initial(this);
    }
    return orElse();
  }
}

abstract class Initial implements ListState {
  const factory Initial({List<MovieListItem> items}) = _$Initial;

  @override
  List<MovieListItem> get items;
  @override
  @JsonKey(ignore: true)
  $InitialCopyWith<Initial> get copyWith => throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $LoadingCopyWith<$Res> implements $ListStateCopyWith<$Res> {
  factory $LoadingCopyWith(Loading value, $Res Function(Loading) then) =
      _$LoadingCopyWithImpl<$Res>;
  @override
  $Res call({List<MovieListItem> items});
}

/// @nodoc
class _$LoadingCopyWithImpl<$Res> extends _$ListStateCopyWithImpl<$Res>
    implements $LoadingCopyWith<$Res> {
  _$LoadingCopyWithImpl(Loading _value, $Res Function(Loading) _then)
      : super(_value, (v) => _then(v as Loading));

  @override
  Loading get _value => super._value as Loading;

  @override
  $Res call({
    Object? items = freezed,
  }) {
    return _then(Loading(
      items: items == freezed
          ? _value.items
          : items // ignore: cast_nullable_to_non_nullable
              as List<MovieListItem>,
    ));
  }
}

/// @nodoc

class _$Loading with DiagnosticableTreeMixin implements Loading {
  const _$Loading({this.items = const []});

  @JsonKey(defaultValue: const [])
  @override
  final List<MovieListItem> items;

  @override
  String toString({DiagnosticLevel minLevel = DiagnosticLevel.info}) {
    return 'ListState.loading(items: $items)';
  }

  @override
  void debugFillProperties(DiagnosticPropertiesBuilder properties) {
    super.debugFillProperties(properties);
    properties
      ..add(DiagnosticsProperty('type', 'ListState.loading'))
      ..add(DiagnosticsProperty('items', items));
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is Loading &&
            const DeepCollectionEquality().equals(other.items, items));
  }

  @override
  int get hashCode =>
      Object.hash(runtimeType, const DeepCollectionEquality().hash(items));

  @JsonKey(ignore: true)
  @override
  $LoadingCopyWith<Loading> get copyWith =>
      _$LoadingCopyWithImpl<Loading>(this, _$identity);

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function(List<MovieListItem> items) initial,
    required TResult Function(List<MovieListItem> items) loading,
    required TResult Function(List<MovieListItem> items, int pageNumber)
        loadedData,
    required TResult Function(List<MovieListItem> items, String errorMessage)
        error,
    required TResult Function(List<MovieListItem> items) allDataLoaded,
  }) {
    return loading(items);
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function(List<MovieListItem> items)? initial,
    TResult Function(List<MovieListItem> items)? loading,
    TResult Function(List<MovieListItem> items, int pageNumber)? loadedData,
    TResult Function(List<MovieListItem> items, String errorMessage)? error,
    TResult Function(List<MovieListItem> items)? allDataLoaded,
  }) {
    return loading?.call(items);
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function(List<MovieListItem> items)? initial,
    TResult Function(List<MovieListItem> items)? loading,
    TResult Function(List<MovieListItem> items, int pageNumber)? loadedData,
    TResult Function(List<MovieListItem> items, String errorMessage)? error,
    TResult Function(List<MovieListItem> items)? allDataLoaded,
    required TResult orElse(),
  }) {
    if (loading != null) {
      return loading(items);
    }
    return orElse();
  }

  @override
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(Initial value) initial,
    required TResult Function(Loading value) loading,
    required TResult Function(LoadedData value) loadedData,
    required TResult Function(Error value) error,
    required TResult Function(AllDataLoaded value) allDataLoaded,
  }) {
    return loading(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(Initial value)? initial,
    TResult Function(Loading value)? loading,
    TResult Function(LoadedData value)? loadedData,
    TResult Function(Error value)? error,
    TResult Function(AllDataLoaded value)? allDataLoaded,
  }) {
    return loading?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(Initial value)? initial,
    TResult Function(Loading value)? loading,
    TResult Function(LoadedData value)? loadedData,
    TResult Function(Error value)? error,
    TResult Function(AllDataLoaded value)? allDataLoaded,
    required TResult orElse(),
  }) {
    if (loading != null) {
      return loading(this);
    }
    return orElse();
  }
}

abstract class Loading implements ListState {
  const factory Loading({List<MovieListItem> items}) = _$Loading;

  @override
  List<MovieListItem> get items;
  @override
  @JsonKey(ignore: true)
  $LoadingCopyWith<Loading> get copyWith => throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $LoadedDataCopyWith<$Res> implements $ListStateCopyWith<$Res> {
  factory $LoadedDataCopyWith(
          LoadedData value, $Res Function(LoadedData) then) =
      _$LoadedDataCopyWithImpl<$Res>;
  @override
  $Res call({List<MovieListItem> items, int pageNumber});
}

/// @nodoc
class _$LoadedDataCopyWithImpl<$Res> extends _$ListStateCopyWithImpl<$Res>
    implements $LoadedDataCopyWith<$Res> {
  _$LoadedDataCopyWithImpl(LoadedData _value, $Res Function(LoadedData) _then)
      : super(_value, (v) => _then(v as LoadedData));

  @override
  LoadedData get _value => super._value as LoadedData;

  @override
  $Res call({
    Object? items = freezed,
    Object? pageNumber = freezed,
  }) {
    return _then(LoadedData(
      items: items == freezed
          ? _value.items
          : items // ignore: cast_nullable_to_non_nullable
              as List<MovieListItem>,
      pageNumber: pageNumber == freezed
          ? _value.pageNumber
          : pageNumber // ignore: cast_nullable_to_non_nullable
              as int,
    ));
  }
}

/// @nodoc

class _$LoadedData with DiagnosticableTreeMixin implements LoadedData {
  const _$LoadedData({required this.items, required this.pageNumber});

  @override
  final List<MovieListItem> items;
  @override
  final int pageNumber;

  @override
  String toString({DiagnosticLevel minLevel = DiagnosticLevel.info}) {
    return 'ListState.loadedData(items: $items, pageNumber: $pageNumber)';
  }

  @override
  void debugFillProperties(DiagnosticPropertiesBuilder properties) {
    super.debugFillProperties(properties);
    properties
      ..add(DiagnosticsProperty('type', 'ListState.loadedData'))
      ..add(DiagnosticsProperty('items', items))
      ..add(DiagnosticsProperty('pageNumber', pageNumber));
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is LoadedData &&
            const DeepCollectionEquality().equals(other.items, items) &&
            (identical(other.pageNumber, pageNumber) ||
                other.pageNumber == pageNumber));
  }

  @override
  int get hashCode => Object.hash(
      runtimeType, const DeepCollectionEquality().hash(items), pageNumber);

  @JsonKey(ignore: true)
  @override
  $LoadedDataCopyWith<LoadedData> get copyWith =>
      _$LoadedDataCopyWithImpl<LoadedData>(this, _$identity);

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function(List<MovieListItem> items) initial,
    required TResult Function(List<MovieListItem> items) loading,
    required TResult Function(List<MovieListItem> items, int pageNumber)
        loadedData,
    required TResult Function(List<MovieListItem> items, String errorMessage)
        error,
    required TResult Function(List<MovieListItem> items) allDataLoaded,
  }) {
    return loadedData(items, pageNumber);
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function(List<MovieListItem> items)? initial,
    TResult Function(List<MovieListItem> items)? loading,
    TResult Function(List<MovieListItem> items, int pageNumber)? loadedData,
    TResult Function(List<MovieListItem> items, String errorMessage)? error,
    TResult Function(List<MovieListItem> items)? allDataLoaded,
  }) {
    return loadedData?.call(items, pageNumber);
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function(List<MovieListItem> items)? initial,
    TResult Function(List<MovieListItem> items)? loading,
    TResult Function(List<MovieListItem> items, int pageNumber)? loadedData,
    TResult Function(List<MovieListItem> items, String errorMessage)? error,
    TResult Function(List<MovieListItem> items)? allDataLoaded,
    required TResult orElse(),
  }) {
    if (loadedData != null) {
      return loadedData(items, pageNumber);
    }
    return orElse();
  }

  @override
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(Initial value) initial,
    required TResult Function(Loading value) loading,
    required TResult Function(LoadedData value) loadedData,
    required TResult Function(Error value) error,
    required TResult Function(AllDataLoaded value) allDataLoaded,
  }) {
    return loadedData(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(Initial value)? initial,
    TResult Function(Loading value)? loading,
    TResult Function(LoadedData value)? loadedData,
    TResult Function(Error value)? error,
    TResult Function(AllDataLoaded value)? allDataLoaded,
  }) {
    return loadedData?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(Initial value)? initial,
    TResult Function(Loading value)? loading,
    TResult Function(LoadedData value)? loadedData,
    TResult Function(Error value)? error,
    TResult Function(AllDataLoaded value)? allDataLoaded,
    required TResult orElse(),
  }) {
    if (loadedData != null) {
      return loadedData(this);
    }
    return orElse();
  }
}

abstract class LoadedData implements ListState {
  const factory LoadedData(
      {required List<MovieListItem> items,
      required int pageNumber}) = _$LoadedData;

  @override
  List<MovieListItem> get items;
  int get pageNumber;
  @override
  @JsonKey(ignore: true)
  $LoadedDataCopyWith<LoadedData> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $ErrorCopyWith<$Res> implements $ListStateCopyWith<$Res> {
  factory $ErrorCopyWith(Error value, $Res Function(Error) then) =
      _$ErrorCopyWithImpl<$Res>;
  @override
  $Res call({List<MovieListItem> items, String errorMessage});
}

/// @nodoc
class _$ErrorCopyWithImpl<$Res> extends _$ListStateCopyWithImpl<$Res>
    implements $ErrorCopyWith<$Res> {
  _$ErrorCopyWithImpl(Error _value, $Res Function(Error) _then)
      : super(_value, (v) => _then(v as Error));

  @override
  Error get _value => super._value as Error;

  @override
  $Res call({
    Object? items = freezed,
    Object? errorMessage = freezed,
  }) {
    return _then(Error(
      items: items == freezed
          ? _value.items
          : items // ignore: cast_nullable_to_non_nullable
              as List<MovieListItem>,
      errorMessage: errorMessage == freezed
          ? _value.errorMessage
          : errorMessage // ignore: cast_nullable_to_non_nullable
              as String,
    ));
  }
}

/// @nodoc

class _$Error with DiagnosticableTreeMixin implements Error {
  const _$Error({required this.items, required this.errorMessage});

  @override
  final List<MovieListItem> items;
  @override
  final String errorMessage;

  @override
  String toString({DiagnosticLevel minLevel = DiagnosticLevel.info}) {
    return 'ListState.error(items: $items, errorMessage: $errorMessage)';
  }

  @override
  void debugFillProperties(DiagnosticPropertiesBuilder properties) {
    super.debugFillProperties(properties);
    properties
      ..add(DiagnosticsProperty('type', 'ListState.error'))
      ..add(DiagnosticsProperty('items', items))
      ..add(DiagnosticsProperty('errorMessage', errorMessage));
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is Error &&
            const DeepCollectionEquality().equals(other.items, items) &&
            (identical(other.errorMessage, errorMessage) ||
                other.errorMessage == errorMessage));
  }

  @override
  int get hashCode => Object.hash(
      runtimeType, const DeepCollectionEquality().hash(items), errorMessage);

  @JsonKey(ignore: true)
  @override
  $ErrorCopyWith<Error> get copyWith =>
      _$ErrorCopyWithImpl<Error>(this, _$identity);

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function(List<MovieListItem> items) initial,
    required TResult Function(List<MovieListItem> items) loading,
    required TResult Function(List<MovieListItem> items, int pageNumber)
        loadedData,
    required TResult Function(List<MovieListItem> items, String errorMessage)
        error,
    required TResult Function(List<MovieListItem> items) allDataLoaded,
  }) {
    return error(items, errorMessage);
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function(List<MovieListItem> items)? initial,
    TResult Function(List<MovieListItem> items)? loading,
    TResult Function(List<MovieListItem> items, int pageNumber)? loadedData,
    TResult Function(List<MovieListItem> items, String errorMessage)? error,
    TResult Function(List<MovieListItem> items)? allDataLoaded,
  }) {
    return error?.call(items, errorMessage);
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function(List<MovieListItem> items)? initial,
    TResult Function(List<MovieListItem> items)? loading,
    TResult Function(List<MovieListItem> items, int pageNumber)? loadedData,
    TResult Function(List<MovieListItem> items, String errorMessage)? error,
    TResult Function(List<MovieListItem> items)? allDataLoaded,
    required TResult orElse(),
  }) {
    if (error != null) {
      return error(items, errorMessage);
    }
    return orElse();
  }

  @override
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(Initial value) initial,
    required TResult Function(Loading value) loading,
    required TResult Function(LoadedData value) loadedData,
    required TResult Function(Error value) error,
    required TResult Function(AllDataLoaded value) allDataLoaded,
  }) {
    return error(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(Initial value)? initial,
    TResult Function(Loading value)? loading,
    TResult Function(LoadedData value)? loadedData,
    TResult Function(Error value)? error,
    TResult Function(AllDataLoaded value)? allDataLoaded,
  }) {
    return error?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(Initial value)? initial,
    TResult Function(Loading value)? loading,
    TResult Function(LoadedData value)? loadedData,
    TResult Function(Error value)? error,
    TResult Function(AllDataLoaded value)? allDataLoaded,
    required TResult orElse(),
  }) {
    if (error != null) {
      return error(this);
    }
    return orElse();
  }
}

abstract class Error implements ListState {
  const factory Error(
      {required List<MovieListItem> items,
      required String errorMessage}) = _$Error;

  @override
  List<MovieListItem> get items;
  String get errorMessage;
  @override
  @JsonKey(ignore: true)
  $ErrorCopyWith<Error> get copyWith => throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $AllDataLoadedCopyWith<$Res>
    implements $ListStateCopyWith<$Res> {
  factory $AllDataLoadedCopyWith(
          AllDataLoaded value, $Res Function(AllDataLoaded) then) =
      _$AllDataLoadedCopyWithImpl<$Res>;
  @override
  $Res call({List<MovieListItem> items});
}

/// @nodoc
class _$AllDataLoadedCopyWithImpl<$Res> extends _$ListStateCopyWithImpl<$Res>
    implements $AllDataLoadedCopyWith<$Res> {
  _$AllDataLoadedCopyWithImpl(
      AllDataLoaded _value, $Res Function(AllDataLoaded) _then)
      : super(_value, (v) => _then(v as AllDataLoaded));

  @override
  AllDataLoaded get _value => super._value as AllDataLoaded;

  @override
  $Res call({
    Object? items = freezed,
  }) {
    return _then(AllDataLoaded(
      items: items == freezed
          ? _value.items
          : items // ignore: cast_nullable_to_non_nullable
              as List<MovieListItem>,
    ));
  }
}

/// @nodoc

class _$AllDataLoaded with DiagnosticableTreeMixin implements AllDataLoaded {
  const _$AllDataLoaded({required this.items});

  @override
  final List<MovieListItem> items;

  @override
  String toString({DiagnosticLevel minLevel = DiagnosticLevel.info}) {
    return 'ListState.allDataLoaded(items: $items)';
  }

  @override
  void debugFillProperties(DiagnosticPropertiesBuilder properties) {
    super.debugFillProperties(properties);
    properties
      ..add(DiagnosticsProperty('type', 'ListState.allDataLoaded'))
      ..add(DiagnosticsProperty('items', items));
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is AllDataLoaded &&
            const DeepCollectionEquality().equals(other.items, items));
  }

  @override
  int get hashCode =>
      Object.hash(runtimeType, const DeepCollectionEquality().hash(items));

  @JsonKey(ignore: true)
  @override
  $AllDataLoadedCopyWith<AllDataLoaded> get copyWith =>
      _$AllDataLoadedCopyWithImpl<AllDataLoaded>(this, _$identity);

  @override
  @optionalTypeArgs
  TResult when<TResult extends Object?>({
    required TResult Function(List<MovieListItem> items) initial,
    required TResult Function(List<MovieListItem> items) loading,
    required TResult Function(List<MovieListItem> items, int pageNumber)
        loadedData,
    required TResult Function(List<MovieListItem> items, String errorMessage)
        error,
    required TResult Function(List<MovieListItem> items) allDataLoaded,
  }) {
    return allDataLoaded(items);
  }

  @override
  @optionalTypeArgs
  TResult? whenOrNull<TResult extends Object?>({
    TResult Function(List<MovieListItem> items)? initial,
    TResult Function(List<MovieListItem> items)? loading,
    TResult Function(List<MovieListItem> items, int pageNumber)? loadedData,
    TResult Function(List<MovieListItem> items, String errorMessage)? error,
    TResult Function(List<MovieListItem> items)? allDataLoaded,
  }) {
    return allDataLoaded?.call(items);
  }

  @override
  @optionalTypeArgs
  TResult maybeWhen<TResult extends Object?>({
    TResult Function(List<MovieListItem> items)? initial,
    TResult Function(List<MovieListItem> items)? loading,
    TResult Function(List<MovieListItem> items, int pageNumber)? loadedData,
    TResult Function(List<MovieListItem> items, String errorMessage)? error,
    TResult Function(List<MovieListItem> items)? allDataLoaded,
    required TResult orElse(),
  }) {
    if (allDataLoaded != null) {
      return allDataLoaded(items);
    }
    return orElse();
  }

  @override
  @optionalTypeArgs
  TResult map<TResult extends Object?>({
    required TResult Function(Initial value) initial,
    required TResult Function(Loading value) loading,
    required TResult Function(LoadedData value) loadedData,
    required TResult Function(Error value) error,
    required TResult Function(AllDataLoaded value) allDataLoaded,
  }) {
    return allDataLoaded(this);
  }

  @override
  @optionalTypeArgs
  TResult? mapOrNull<TResult extends Object?>({
    TResult Function(Initial value)? initial,
    TResult Function(Loading value)? loading,
    TResult Function(LoadedData value)? loadedData,
    TResult Function(Error value)? error,
    TResult Function(AllDataLoaded value)? allDataLoaded,
  }) {
    return allDataLoaded?.call(this);
  }

  @override
  @optionalTypeArgs
  TResult maybeMap<TResult extends Object?>({
    TResult Function(Initial value)? initial,
    TResult Function(Loading value)? loading,
    TResult Function(LoadedData value)? loadedData,
    TResult Function(Error value)? error,
    TResult Function(AllDataLoaded value)? allDataLoaded,
    required TResult orElse(),
  }) {
    if (allDataLoaded != null) {
      return allDataLoaded(this);
    }
    return orElse();
  }
}

abstract class AllDataLoaded implements ListState {
  const factory AllDataLoaded({required List<MovieListItem> items}) =
      _$AllDataLoaded;

  @override
  List<MovieListItem> get items;
  @override
  @JsonKey(ignore: true)
  $AllDataLoadedCopyWith<AllDataLoaded> get copyWith =>
      throw _privateConstructorUsedError;
}
