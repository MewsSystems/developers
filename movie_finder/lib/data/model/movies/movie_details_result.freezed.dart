// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'movie_details_result.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more informations: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

MovieDetailsResult _$MovieDetailsResultFromJson(Map<String, dynamic> json) {
  return _MovieDetailsResult.fromJson(json);
}

/// @nodoc
class _$MovieDetailsResultTearOff {
  const _$MovieDetailsResultTearOff();

  _MovieDetailsResult call(
      {String? title,
      String? overview,
      @JsonKey(name: 'original_title') String? originalTitle,
      @JsonKey(name: 'release_date') String? releaseDate,
      @JsonKey(name: 'poster_path') String? posterPath,
      int? revenue}) {
    return _MovieDetailsResult(
      title: title,
      overview: overview,
      originalTitle: originalTitle,
      releaseDate: releaseDate,
      posterPath: posterPath,
      revenue: revenue,
    );
  }

  MovieDetailsResult fromJson(Map<String, Object?> json) {
    return MovieDetailsResult.fromJson(json);
  }
}

/// @nodoc
const $MovieDetailsResult = _$MovieDetailsResultTearOff();

/// @nodoc
mixin _$MovieDetailsResult {
  String? get title => throw _privateConstructorUsedError;
  String? get overview => throw _privateConstructorUsedError;
  @JsonKey(name: 'original_title')
  String? get originalTitle => throw _privateConstructorUsedError;
  @JsonKey(name: 'release_date')
  String? get releaseDate => throw _privateConstructorUsedError;
  @JsonKey(name: 'poster_path')
  String? get posterPath => throw _privateConstructorUsedError;
  int? get revenue => throw _privateConstructorUsedError;

  Map<String, dynamic> toJson() => throw _privateConstructorUsedError;
  @JsonKey(ignore: true)
  $MovieDetailsResultCopyWith<MovieDetailsResult> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MovieDetailsResultCopyWith<$Res> {
  factory $MovieDetailsResultCopyWith(
          MovieDetailsResult value, $Res Function(MovieDetailsResult) then) =
      _$MovieDetailsResultCopyWithImpl<$Res>;
  $Res call(
      {String? title,
      String? overview,
      @JsonKey(name: 'original_title') String? originalTitle,
      @JsonKey(name: 'release_date') String? releaseDate,
      @JsonKey(name: 'poster_path') String? posterPath,
      int? revenue});
}

/// @nodoc
class _$MovieDetailsResultCopyWithImpl<$Res>
    implements $MovieDetailsResultCopyWith<$Res> {
  _$MovieDetailsResultCopyWithImpl(this._value, this._then);

  final MovieDetailsResult _value;
  // ignore: unused_field
  final $Res Function(MovieDetailsResult) _then;

  @override
  $Res call({
    Object? title = freezed,
    Object? overview = freezed,
    Object? originalTitle = freezed,
    Object? releaseDate = freezed,
    Object? posterPath = freezed,
    Object? revenue = freezed,
  }) {
    return _then(_value.copyWith(
      title: title == freezed
          ? _value.title
          : title // ignore: cast_nullable_to_non_nullable
              as String?,
      overview: overview == freezed
          ? _value.overview
          : overview // ignore: cast_nullable_to_non_nullable
              as String?,
      originalTitle: originalTitle == freezed
          ? _value.originalTitle
          : originalTitle // ignore: cast_nullable_to_non_nullable
              as String?,
      releaseDate: releaseDate == freezed
          ? _value.releaseDate
          : releaseDate // ignore: cast_nullable_to_non_nullable
              as String?,
      posterPath: posterPath == freezed
          ? _value.posterPath
          : posterPath // ignore: cast_nullable_to_non_nullable
              as String?,
      revenue: revenue == freezed
          ? _value.revenue
          : revenue // ignore: cast_nullable_to_non_nullable
              as int?,
    ));
  }
}

/// @nodoc
abstract class _$MovieDetailsResultCopyWith<$Res>
    implements $MovieDetailsResultCopyWith<$Res> {
  factory _$MovieDetailsResultCopyWith(
          _MovieDetailsResult value, $Res Function(_MovieDetailsResult) then) =
      __$MovieDetailsResultCopyWithImpl<$Res>;
  @override
  $Res call(
      {String? title,
      String? overview,
      @JsonKey(name: 'original_title') String? originalTitle,
      @JsonKey(name: 'release_date') String? releaseDate,
      @JsonKey(name: 'poster_path') String? posterPath,
      int? revenue});
}

/// @nodoc
class __$MovieDetailsResultCopyWithImpl<$Res>
    extends _$MovieDetailsResultCopyWithImpl<$Res>
    implements _$MovieDetailsResultCopyWith<$Res> {
  __$MovieDetailsResultCopyWithImpl(
      _MovieDetailsResult _value, $Res Function(_MovieDetailsResult) _then)
      : super(_value, (v) => _then(v as _MovieDetailsResult));

  @override
  _MovieDetailsResult get _value => super._value as _MovieDetailsResult;

  @override
  $Res call({
    Object? title = freezed,
    Object? overview = freezed,
    Object? originalTitle = freezed,
    Object? releaseDate = freezed,
    Object? posterPath = freezed,
    Object? revenue = freezed,
  }) {
    return _then(_MovieDetailsResult(
      title: title == freezed
          ? _value.title
          : title // ignore: cast_nullable_to_non_nullable
              as String?,
      overview: overview == freezed
          ? _value.overview
          : overview // ignore: cast_nullable_to_non_nullable
              as String?,
      originalTitle: originalTitle == freezed
          ? _value.originalTitle
          : originalTitle // ignore: cast_nullable_to_non_nullable
              as String?,
      releaseDate: releaseDate == freezed
          ? _value.releaseDate
          : releaseDate // ignore: cast_nullable_to_non_nullable
              as String?,
      posterPath: posterPath == freezed
          ? _value.posterPath
          : posterPath // ignore: cast_nullable_to_non_nullable
              as String?,
      revenue: revenue == freezed
          ? _value.revenue
          : revenue // ignore: cast_nullable_to_non_nullable
              as int?,
    ));
  }
}

/// @nodoc
@JsonSerializable()
class _$_MovieDetailsResult implements _MovieDetailsResult {
  const _$_MovieDetailsResult(
      {this.title,
      this.overview,
      @JsonKey(name: 'original_title') this.originalTitle,
      @JsonKey(name: 'release_date') this.releaseDate,
      @JsonKey(name: 'poster_path') this.posterPath,
      this.revenue});

  factory _$_MovieDetailsResult.fromJson(Map<String, dynamic> json) =>
      _$$_MovieDetailsResultFromJson(json);

  @override
  final String? title;
  @override
  final String? overview;
  @override
  @JsonKey(name: 'original_title')
  final String? originalTitle;
  @override
  @JsonKey(name: 'release_date')
  final String? releaseDate;
  @override
  @JsonKey(name: 'poster_path')
  final String? posterPath;
  @override
  final int? revenue;

  @override
  String toString() {
    return 'MovieDetailsResult(title: $title, overview: $overview, originalTitle: $originalTitle, releaseDate: $releaseDate, posterPath: $posterPath, revenue: $revenue)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _MovieDetailsResult &&
            const DeepCollectionEquality().equals(other.title, title) &&
            const DeepCollectionEquality().equals(other.overview, overview) &&
            const DeepCollectionEquality()
                .equals(other.originalTitle, originalTitle) &&
            const DeepCollectionEquality()
                .equals(other.releaseDate, releaseDate) &&
            const DeepCollectionEquality()
                .equals(other.posterPath, posterPath) &&
            const DeepCollectionEquality().equals(other.revenue, revenue));
  }

  @override
  int get hashCode => Object.hash(
      runtimeType,
      const DeepCollectionEquality().hash(title),
      const DeepCollectionEquality().hash(overview),
      const DeepCollectionEquality().hash(originalTitle),
      const DeepCollectionEquality().hash(releaseDate),
      const DeepCollectionEquality().hash(posterPath),
      const DeepCollectionEquality().hash(revenue));

  @JsonKey(ignore: true)
  @override
  _$MovieDetailsResultCopyWith<_MovieDetailsResult> get copyWith =>
      __$MovieDetailsResultCopyWithImpl<_MovieDetailsResult>(this, _$identity);

  @override
  Map<String, dynamic> toJson() {
    return _$$_MovieDetailsResultToJson(this);
  }
}

abstract class _MovieDetailsResult implements MovieDetailsResult {
  const factory _MovieDetailsResult(
      {String? title,
      String? overview,
      @JsonKey(name: 'original_title') String? originalTitle,
      @JsonKey(name: 'release_date') String? releaseDate,
      @JsonKey(name: 'poster_path') String? posterPath,
      int? revenue}) = _$_MovieDetailsResult;

  factory _MovieDetailsResult.fromJson(Map<String, dynamic> json) =
      _$_MovieDetailsResult.fromJson;

  @override
  String? get title;
  @override
  String? get overview;
  @override
  @JsonKey(name: 'original_title')
  String? get originalTitle;
  @override
  @JsonKey(name: 'release_date')
  String? get releaseDate;
  @override
  @JsonKey(name: 'poster_path')
  String? get posterPath;
  @override
  int? get revenue;
  @override
  @JsonKey(ignore: true)
  _$MovieDetailsResultCopyWith<_MovieDetailsResult> get copyWith =>
      throw _privateConstructorUsedError;
}
