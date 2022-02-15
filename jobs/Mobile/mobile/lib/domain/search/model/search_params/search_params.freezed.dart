// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'search_params.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more informations: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

/// @nodoc
class _$SearchParamsTearOff {
  const _$SearchParamsTearOff();

  _SearchParams call({required String query, required String page}) {
    return _SearchParams(
      query: query,
      page: page,
    );
  }
}

/// @nodoc
const $SearchParams = _$SearchParamsTearOff();

/// @nodoc
mixin _$SearchParams {
  String get query => throw _privateConstructorUsedError;
  String get page => throw _privateConstructorUsedError;

  @JsonKey(ignore: true)
  $SearchParamsCopyWith<SearchParams> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $SearchParamsCopyWith<$Res> {
  factory $SearchParamsCopyWith(
          SearchParams value, $Res Function(SearchParams) then) =
      _$SearchParamsCopyWithImpl<$Res>;
  $Res call({String query, String page});
}

/// @nodoc
class _$SearchParamsCopyWithImpl<$Res> implements $SearchParamsCopyWith<$Res> {
  _$SearchParamsCopyWithImpl(this._value, this._then);

  final SearchParams _value;
  // ignore: unused_field
  final $Res Function(SearchParams) _then;

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
              as String,
    ));
  }
}

/// @nodoc
abstract class _$SearchParamsCopyWith<$Res>
    implements $SearchParamsCopyWith<$Res> {
  factory _$SearchParamsCopyWith(
          _SearchParams value, $Res Function(_SearchParams) then) =
      __$SearchParamsCopyWithImpl<$Res>;
  @override
  $Res call({String query, String page});
}

/// @nodoc
class __$SearchParamsCopyWithImpl<$Res> extends _$SearchParamsCopyWithImpl<$Res>
    implements _$SearchParamsCopyWith<$Res> {
  __$SearchParamsCopyWithImpl(
      _SearchParams _value, $Res Function(_SearchParams) _then)
      : super(_value, (v) => _then(v as _SearchParams));

  @override
  _SearchParams get _value => super._value as _SearchParams;

  @override
  $Res call({
    Object? query = freezed,
    Object? page = freezed,
  }) {
    return _then(_SearchParams(
      query: query == freezed
          ? _value.query
          : query // ignore: cast_nullable_to_non_nullable
              as String,
      page: page == freezed
          ? _value.page
          : page // ignore: cast_nullable_to_non_nullable
              as String,
    ));
  }
}

/// @nodoc

class _$_SearchParams extends _SearchParams {
  const _$_SearchParams({required this.query, required this.page}) : super._();

  @override
  final String query;
  @override
  final String page;

  @override
  String toString() {
    return 'SearchParams(query: $query, page: $page)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _SearchParams &&
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
  _$SearchParamsCopyWith<_SearchParams> get copyWith =>
      __$SearchParamsCopyWithImpl<_SearchParams>(this, _$identity);
}

abstract class _SearchParams extends SearchParams {
  const factory _SearchParams({required String query, required String page}) =
      _$_SearchParams;
  const _SearchParams._() : super._();

  @override
  String get query;
  @override
  String get page;
  @override
  @JsonKey(ignore: true)
  _$SearchParamsCopyWith<_SearchParams> get copyWith =>
      throw _privateConstructorUsedError;
}
