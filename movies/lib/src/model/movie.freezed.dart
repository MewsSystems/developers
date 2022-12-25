// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: type=lint
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'movie.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more information: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

Movie _$MovieFromJson(Map<String, dynamic> json) {
  return _Movie.fromJson(json);
}

/// @nodoc
mixin _$Movie {
  int get id => throw _privateConstructorUsedError;
  String get title => throw _privateConstructorUsedError;
  String get originalTitle => throw _privateConstructorUsedError;
  String get originalLanguage => throw _privateConstructorUsedError;
  String? get posterPath => throw _privateConstructorUsedError;
  String? get backdropPath => throw _privateConstructorUsedError;
  @DateTimeConverter()
  DateTime? get releaseDate => throw _privateConstructorUsedError;
  double get voteAverage => throw _privateConstructorUsedError;
  double get popularity => throw _privateConstructorUsedError;
  bool get adult => throw _privateConstructorUsedError;
  String get overview => throw _privateConstructorUsedError;
  List<int> get genreIds => throw _privateConstructorUsedError;

  Map<String, dynamic> toJson() => throw _privateConstructorUsedError;
  @JsonKey(ignore: true)
  $MovieCopyWith<Movie> get copyWith => throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MovieCopyWith<$Res> {
  factory $MovieCopyWith(Movie value, $Res Function(Movie) then) =
      _$MovieCopyWithImpl<$Res, Movie>;
  @useResult
  $Res call(
      {int id,
      String title,
      String originalTitle,
      String originalLanguage,
      String? posterPath,
      String? backdropPath,
      @DateTimeConverter() DateTime? releaseDate,
      double voteAverage,
      double popularity,
      bool adult,
      String overview,
      List<int> genreIds});
}

/// @nodoc
class _$MovieCopyWithImpl<$Res, $Val extends Movie>
    implements $MovieCopyWith<$Res> {
  _$MovieCopyWithImpl(this._value, this._then);

  // ignore: unused_field
  final $Val _value;
  // ignore: unused_field
  final $Res Function($Val) _then;

  @pragma('vm:prefer-inline')
  @override
  $Res call({
    Object? id = null,
    Object? title = null,
    Object? originalTitle = null,
    Object? originalLanguage = null,
    Object? posterPath = freezed,
    Object? backdropPath = freezed,
    Object? releaseDate = freezed,
    Object? voteAverage = null,
    Object? popularity = null,
    Object? adult = null,
    Object? overview = null,
    Object? genreIds = null,
  }) {
    return _then(_value.copyWith(
      id: null == id
          ? _value.id
          : id // ignore: cast_nullable_to_non_nullable
              as int,
      title: null == title
          ? _value.title
          : title // ignore: cast_nullable_to_non_nullable
              as String,
      originalTitle: null == originalTitle
          ? _value.originalTitle
          : originalTitle // ignore: cast_nullable_to_non_nullable
              as String,
      originalLanguage: null == originalLanguage
          ? _value.originalLanguage
          : originalLanguage // ignore: cast_nullable_to_non_nullable
              as String,
      posterPath: freezed == posterPath
          ? _value.posterPath
          : posterPath // ignore: cast_nullable_to_non_nullable
              as String?,
      backdropPath: freezed == backdropPath
          ? _value.backdropPath
          : backdropPath // ignore: cast_nullable_to_non_nullable
              as String?,
      releaseDate: freezed == releaseDate
          ? _value.releaseDate
          : releaseDate // ignore: cast_nullable_to_non_nullable
              as DateTime?,
      voteAverage: null == voteAverage
          ? _value.voteAverage
          : voteAverage // ignore: cast_nullable_to_non_nullable
              as double,
      popularity: null == popularity
          ? _value.popularity
          : popularity // ignore: cast_nullable_to_non_nullable
              as double,
      adult: null == adult
          ? _value.adult
          : adult // ignore: cast_nullable_to_non_nullable
              as bool,
      overview: null == overview
          ? _value.overview
          : overview // ignore: cast_nullable_to_non_nullable
              as String,
      genreIds: null == genreIds
          ? _value.genreIds
          : genreIds // ignore: cast_nullable_to_non_nullable
              as List<int>,
    ) as $Val);
  }
}

/// @nodoc
abstract class _$$_MovieCopyWith<$Res> implements $MovieCopyWith<$Res> {
  factory _$$_MovieCopyWith(_$_Movie value, $Res Function(_$_Movie) then) =
      __$$_MovieCopyWithImpl<$Res>;
  @override
  @useResult
  $Res call(
      {int id,
      String title,
      String originalTitle,
      String originalLanguage,
      String? posterPath,
      String? backdropPath,
      @DateTimeConverter() DateTime? releaseDate,
      double voteAverage,
      double popularity,
      bool adult,
      String overview,
      List<int> genreIds});
}

/// @nodoc
class __$$_MovieCopyWithImpl<$Res> extends _$MovieCopyWithImpl<$Res, _$_Movie>
    implements _$$_MovieCopyWith<$Res> {
  __$$_MovieCopyWithImpl(_$_Movie _value, $Res Function(_$_Movie) _then)
      : super(_value, _then);

  @pragma('vm:prefer-inline')
  @override
  $Res call({
    Object? id = null,
    Object? title = null,
    Object? originalTitle = null,
    Object? originalLanguage = null,
    Object? posterPath = freezed,
    Object? backdropPath = freezed,
    Object? releaseDate = freezed,
    Object? voteAverage = null,
    Object? popularity = null,
    Object? adult = null,
    Object? overview = null,
    Object? genreIds = null,
  }) {
    return _then(_$_Movie(
      id: null == id
          ? _value.id
          : id // ignore: cast_nullable_to_non_nullable
              as int,
      title: null == title
          ? _value.title
          : title // ignore: cast_nullable_to_non_nullable
              as String,
      originalTitle: null == originalTitle
          ? _value.originalTitle
          : originalTitle // ignore: cast_nullable_to_non_nullable
              as String,
      originalLanguage: null == originalLanguage
          ? _value.originalLanguage
          : originalLanguage // ignore: cast_nullable_to_non_nullable
              as String,
      posterPath: freezed == posterPath
          ? _value.posterPath
          : posterPath // ignore: cast_nullable_to_non_nullable
              as String?,
      backdropPath: freezed == backdropPath
          ? _value.backdropPath
          : backdropPath // ignore: cast_nullable_to_non_nullable
              as String?,
      releaseDate: freezed == releaseDate
          ? _value.releaseDate
          : releaseDate // ignore: cast_nullable_to_non_nullable
              as DateTime?,
      voteAverage: null == voteAverage
          ? _value.voteAverage
          : voteAverage // ignore: cast_nullable_to_non_nullable
              as double,
      popularity: null == popularity
          ? _value.popularity
          : popularity // ignore: cast_nullable_to_non_nullable
              as double,
      adult: null == adult
          ? _value.adult
          : adult // ignore: cast_nullable_to_non_nullable
              as bool,
      overview: null == overview
          ? _value.overview
          : overview // ignore: cast_nullable_to_non_nullable
              as String,
      genreIds: null == genreIds
          ? _value._genreIds
          : genreIds // ignore: cast_nullable_to_non_nullable
              as List<int>,
    ));
  }
}

/// @nodoc

@JsonSerializable(fieldRename: FieldRename.snake)
class _$_Movie implements _Movie {
  const _$_Movie(
      {required this.id,
      required this.title,
      required this.originalTitle,
      required this.originalLanguage,
      this.posterPath,
      this.backdropPath,
      @DateTimeConverter() this.releaseDate,
      required this.voteAverage,
      required this.popularity,
      required this.adult,
      required this.overview,
      required final List<int> genreIds})
      : _genreIds = genreIds;

  factory _$_Movie.fromJson(Map<String, dynamic> json) =>
      _$$_MovieFromJson(json);

  @override
  final int id;
  @override
  final String title;
  @override
  final String originalTitle;
  @override
  final String originalLanguage;
  @override
  final String? posterPath;
  @override
  final String? backdropPath;
  @override
  @DateTimeConverter()
  final DateTime? releaseDate;
  @override
  final double voteAverage;
  @override
  final double popularity;
  @override
  final bool adult;
  @override
  final String overview;
  final List<int> _genreIds;
  @override
  List<int> get genreIds {
    // ignore: implicit_dynamic_type
    return EqualUnmodifiableListView(_genreIds);
  }

  @override
  String toString() {
    return 'Movie(id: $id, title: $title, originalTitle: $originalTitle, originalLanguage: $originalLanguage, posterPath: $posterPath, backdropPath: $backdropPath, releaseDate: $releaseDate, voteAverage: $voteAverage, popularity: $popularity, adult: $adult, overview: $overview, genreIds: $genreIds)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _$_Movie &&
            (identical(other.id, id) || other.id == id) &&
            (identical(other.title, title) || other.title == title) &&
            (identical(other.originalTitle, originalTitle) ||
                other.originalTitle == originalTitle) &&
            (identical(other.originalLanguage, originalLanguage) ||
                other.originalLanguage == originalLanguage) &&
            (identical(other.posterPath, posterPath) ||
                other.posterPath == posterPath) &&
            (identical(other.backdropPath, backdropPath) ||
                other.backdropPath == backdropPath) &&
            (identical(other.releaseDate, releaseDate) ||
                other.releaseDate == releaseDate) &&
            (identical(other.voteAverage, voteAverage) ||
                other.voteAverage == voteAverage) &&
            (identical(other.popularity, popularity) ||
                other.popularity == popularity) &&
            (identical(other.adult, adult) || other.adult == adult) &&
            (identical(other.overview, overview) ||
                other.overview == overview) &&
            const DeepCollectionEquality().equals(other._genreIds, _genreIds));
  }

  @JsonKey(ignore: true)
  @override
  int get hashCode => Object.hash(
      runtimeType,
      id,
      title,
      originalTitle,
      originalLanguage,
      posterPath,
      backdropPath,
      releaseDate,
      voteAverage,
      popularity,
      adult,
      overview,
      const DeepCollectionEquality().hash(_genreIds));

  @JsonKey(ignore: true)
  @override
  @pragma('vm:prefer-inline')
  _$$_MovieCopyWith<_$_Movie> get copyWith =>
      __$$_MovieCopyWithImpl<_$_Movie>(this, _$identity);

  @override
  Map<String, dynamic> toJson() {
    return _$$_MovieToJson(
      this,
    );
  }
}

abstract class _Movie implements Movie {
  const factory _Movie(
      {required final int id,
      required final String title,
      required final String originalTitle,
      required final String originalLanguage,
      final String? posterPath,
      final String? backdropPath,
      @DateTimeConverter() final DateTime? releaseDate,
      required final double voteAverage,
      required final double popularity,
      required final bool adult,
      required final String overview,
      required final List<int> genreIds}) = _$_Movie;

  factory _Movie.fromJson(Map<String, dynamic> json) = _$_Movie.fromJson;

  @override
  int get id;
  @override
  String get title;
  @override
  String get originalTitle;
  @override
  String get originalLanguage;
  @override
  String? get posterPath;
  @override
  String? get backdropPath;
  @override
  @DateTimeConverter()
  DateTime? get releaseDate;
  @override
  double get voteAverage;
  @override
  double get popularity;
  @override
  bool get adult;
  @override
  String get overview;
  @override
  List<int> get genreIds;
  @override
  @JsonKey(ignore: true)
  _$$_MovieCopyWith<_$_Movie> get copyWith =>
      throw _privateConstructorUsedError;
}
