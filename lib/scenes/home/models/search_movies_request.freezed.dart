// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target, unnecessary_question_mark

part of 'search_movies_request.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more information: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

SearchMoviesRequest _$SearchMoviesRequestFromJson(Map<String, dynamic> json) {
  return _SearchMoviesRequest.fromJson(json);
}

/// @nodoc
mixin _$SearchMoviesRequest {
  @JsonKey(name: 'api_key')
  String get key => throw _privateConstructorUsedError;
  @JsonKey(name: 'query')
  String get searchText => throw _privateConstructorUsedError;
  @JsonKey(name: 'page')
  int get page => throw _privateConstructorUsedError;

  Map<String, dynamic> toJson() => throw _privateConstructorUsedError;
  @JsonKey(ignore: true)
  $SearchMoviesRequestCopyWith<SearchMoviesRequest> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $SearchMoviesRequestCopyWith<$Res> {
  factory $SearchMoviesRequestCopyWith(
          SearchMoviesRequest value, $Res Function(SearchMoviesRequest) then) =
      _$SearchMoviesRequestCopyWithImpl<$Res, SearchMoviesRequest>;
  @useResult
  $Res call(
      {@JsonKey(name: 'api_key') String key,
      @JsonKey(name: 'query') String searchText,
      @JsonKey(name: 'page') int page});
}

/// @nodoc
class _$SearchMoviesRequestCopyWithImpl<$Res, $Val extends SearchMoviesRequest>
    implements $SearchMoviesRequestCopyWith<$Res> {
  _$SearchMoviesRequestCopyWithImpl(this._value, this._then);

  // ignore: unused_field
  final $Val _value;
  // ignore: unused_field
  final $Res Function($Val) _then;

  @pragma('vm:prefer-inline')
  @override
  $Res call({
    Object? key = null,
    Object? searchText = null,
    Object? page = null,
  }) {
    return _then(_value.copyWith(
      key: null == key
          ? _value.key
          : key // ignore: cast_nullable_to_non_nullable
              as String,
      searchText: null == searchText
          ? _value.searchText
          : searchText // ignore: cast_nullable_to_non_nullable
              as String,
      page: null == page
          ? _value.page
          : page // ignore: cast_nullable_to_non_nullable
              as int,
    ) as $Val);
  }
}

/// @nodoc
abstract class _$$_SearchMoviesRequestCopyWith<$Res>
    implements $SearchMoviesRequestCopyWith<$Res> {
  factory _$$_SearchMoviesRequestCopyWith(_$_SearchMoviesRequest value,
          $Res Function(_$_SearchMoviesRequest) then) =
      __$$_SearchMoviesRequestCopyWithImpl<$Res>;
  @override
  @useResult
  $Res call(
      {@JsonKey(name: 'api_key') String key,
      @JsonKey(name: 'query') String searchText,
      @JsonKey(name: 'page') int page});
}

/// @nodoc
class __$$_SearchMoviesRequestCopyWithImpl<$Res>
    extends _$SearchMoviesRequestCopyWithImpl<$Res, _$_SearchMoviesRequest>
    implements _$$_SearchMoviesRequestCopyWith<$Res> {
  __$$_SearchMoviesRequestCopyWithImpl(_$_SearchMoviesRequest _value,
      $Res Function(_$_SearchMoviesRequest) _then)
      : super(_value, _then);

  @pragma('vm:prefer-inline')
  @override
  $Res call({
    Object? key = null,
    Object? searchText = null,
    Object? page = null,
  }) {
    return _then(_$_SearchMoviesRequest(
      key: null == key
          ? _value.key
          : key // ignore: cast_nullable_to_non_nullable
              as String,
      searchText: null == searchText
          ? _value.searchText
          : searchText // ignore: cast_nullable_to_non_nullable
              as String,
      page: null == page
          ? _value.page
          : page // ignore: cast_nullable_to_non_nullable
              as int,
    ));
  }
}

/// @nodoc
@JsonSerializable()
class _$_SearchMoviesRequest implements _SearchMoviesRequest {
  _$_SearchMoviesRequest(
      {@JsonKey(name: 'api_key') this.key = API_KEY,
      @JsonKey(name: 'query') required this.searchText,
      @JsonKey(name: 'page') required this.page});

  factory _$_SearchMoviesRequest.fromJson(Map<String, dynamic> json) =>
      _$$_SearchMoviesRequestFromJson(json);

  @override
  @JsonKey(name: 'api_key')
  final String key;
  @override
  @JsonKey(name: 'query')
  final String searchText;
  @override
  @JsonKey(name: 'page')
  final int page;

  @override
  String toString() {
    return 'SearchMoviesRequest(key: $key, searchText: $searchText, page: $page)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _$_SearchMoviesRequest &&
            (identical(other.key, key) || other.key == key) &&
            (identical(other.searchText, searchText) ||
                other.searchText == searchText) &&
            (identical(other.page, page) || other.page == page));
  }

  @JsonKey(ignore: true)
  @override
  int get hashCode => Object.hash(runtimeType, key, searchText, page);

  @JsonKey(ignore: true)
  @override
  @pragma('vm:prefer-inline')
  _$$_SearchMoviesRequestCopyWith<_$_SearchMoviesRequest> get copyWith =>
      __$$_SearchMoviesRequestCopyWithImpl<_$_SearchMoviesRequest>(
          this, _$identity);

  @override
  Map<String, dynamic> toJson() {
    return _$$_SearchMoviesRequestToJson(
      this,
    );
  }
}

abstract class _SearchMoviesRequest implements SearchMoviesRequest {
  factory _SearchMoviesRequest(
      {@JsonKey(name: 'api_key') final String key,
      @JsonKey(name: 'query') required final String searchText,
      @JsonKey(name: 'page') required final int page}) = _$_SearchMoviesRequest;

  factory _SearchMoviesRequest.fromJson(Map<String, dynamic> json) =
      _$_SearchMoviesRequest.fromJson;

  @override
  @JsonKey(name: 'api_key')
  String get key;
  @override
  @JsonKey(name: 'query')
  String get searchText;
  @override
  @JsonKey(name: 'page')
  int get page;
  @override
  @JsonKey(ignore: true)
  _$$_SearchMoviesRequestCopyWith<_$_SearchMoviesRequest> get copyWith =>
      throw _privateConstructorUsedError;
}
