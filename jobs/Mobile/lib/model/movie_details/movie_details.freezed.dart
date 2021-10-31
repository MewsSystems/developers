// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'movie_details.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more informations: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

MovieDetails _$MovieDetailsFromJson(Map<String, dynamic> json) {
  return _MoviesDetails.fromJson(json);
}

/// @nodoc
class _$MovieDetailsTearOff {
  const _$MovieDetailsTearOff();

  _MoviesDetails call(
      {required bool adult,
      required int id,
      @JsonKey(name: 'original_title', defaultValue: '')
          required String originalTitle,
      @JsonKey(name: 'overview', defaultValue: '')
          required String description,
      @JsonKey(name: 'release_date', defaultValue: '')
          required String releaseDate}) {
    return _MoviesDetails(
      adult: adult,
      id: id,
      originalTitle: originalTitle,
      description: description,
      releaseDate: releaseDate,
    );
  }

  MovieDetails fromJson(Map<String, Object?> json) {
    return MovieDetails.fromJson(json);
  }
}

/// @nodoc
const $MovieDetails = _$MovieDetailsTearOff();

/// @nodoc
mixin _$MovieDetails {
  bool get adult => throw _privateConstructorUsedError;
  int get id => throw _privateConstructorUsedError;
  @JsonKey(name: 'original_title', defaultValue: '')
  String get originalTitle => throw _privateConstructorUsedError;
  @JsonKey(name: 'overview', defaultValue: '')
  String get description => throw _privateConstructorUsedError;
  @JsonKey(name: 'release_date', defaultValue: '')
  String get releaseDate => throw _privateConstructorUsedError;

  Map<String, dynamic> toJson() => throw _privateConstructorUsedError;
  @JsonKey(ignore: true)
  $MovieDetailsCopyWith<MovieDetails> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MovieDetailsCopyWith<$Res> {
  factory $MovieDetailsCopyWith(
          MovieDetails value, $Res Function(MovieDetails) then) =
      _$MovieDetailsCopyWithImpl<$Res>;
  $Res call(
      {bool adult,
      int id,
      @JsonKey(name: 'original_title', defaultValue: '') String originalTitle,
      @JsonKey(name: 'overview', defaultValue: '') String description,
      @JsonKey(name: 'release_date', defaultValue: '') String releaseDate});
}

/// @nodoc
class _$MovieDetailsCopyWithImpl<$Res> implements $MovieDetailsCopyWith<$Res> {
  _$MovieDetailsCopyWithImpl(this._value, this._then);

  final MovieDetails _value;
  // ignore: unused_field
  final $Res Function(MovieDetails) _then;

  @override
  $Res call({
    Object? adult = freezed,
    Object? id = freezed,
    Object? originalTitle = freezed,
    Object? description = freezed,
    Object? releaseDate = freezed,
  }) {
    return _then(_value.copyWith(
      adult: adult == freezed
          ? _value.adult
          : adult // ignore: cast_nullable_to_non_nullable
              as bool,
      id: id == freezed
          ? _value.id
          : id // ignore: cast_nullable_to_non_nullable
              as int,
      originalTitle: originalTitle == freezed
          ? _value.originalTitle
          : originalTitle // ignore: cast_nullable_to_non_nullable
              as String,
      description: description == freezed
          ? _value.description
          : description // ignore: cast_nullable_to_non_nullable
              as String,
      releaseDate: releaseDate == freezed
          ? _value.releaseDate
          : releaseDate // ignore: cast_nullable_to_non_nullable
              as String,
    ));
  }
}

/// @nodoc
abstract class _$MoviesDetailsCopyWith<$Res>
    implements $MovieDetailsCopyWith<$Res> {
  factory _$MoviesDetailsCopyWith(
          _MoviesDetails value, $Res Function(_MoviesDetails) then) =
      __$MoviesDetailsCopyWithImpl<$Res>;
  @override
  $Res call(
      {bool adult,
      int id,
      @JsonKey(name: 'original_title', defaultValue: '') String originalTitle,
      @JsonKey(name: 'overview', defaultValue: '') String description,
      @JsonKey(name: 'release_date', defaultValue: '') String releaseDate});
}

/// @nodoc
class __$MoviesDetailsCopyWithImpl<$Res>
    extends _$MovieDetailsCopyWithImpl<$Res>
    implements _$MoviesDetailsCopyWith<$Res> {
  __$MoviesDetailsCopyWithImpl(
      _MoviesDetails _value, $Res Function(_MoviesDetails) _then)
      : super(_value, (v) => _then(v as _MoviesDetails));

  @override
  _MoviesDetails get _value => super._value as _MoviesDetails;

  @override
  $Res call({
    Object? adult = freezed,
    Object? id = freezed,
    Object? originalTitle = freezed,
    Object? description = freezed,
    Object? releaseDate = freezed,
  }) {
    return _then(_MoviesDetails(
      adult: adult == freezed
          ? _value.adult
          : adult // ignore: cast_nullable_to_non_nullable
              as bool,
      id: id == freezed
          ? _value.id
          : id // ignore: cast_nullable_to_non_nullable
              as int,
      originalTitle: originalTitle == freezed
          ? _value.originalTitle
          : originalTitle // ignore: cast_nullable_to_non_nullable
              as String,
      description: description == freezed
          ? _value.description
          : description // ignore: cast_nullable_to_non_nullable
              as String,
      releaseDate: releaseDate == freezed
          ? _value.releaseDate
          : releaseDate // ignore: cast_nullable_to_non_nullable
              as String,
    ));
  }
}

/// @nodoc
@JsonSerializable()
class _$_MoviesDetails extends _MoviesDetails {
  const _$_MoviesDetails(
      {required this.adult,
      required this.id,
      @JsonKey(name: 'original_title', defaultValue: '')
          required this.originalTitle,
      @JsonKey(name: 'overview', defaultValue: '')
          required this.description,
      @JsonKey(name: 'release_date', defaultValue: '')
          required this.releaseDate})
      : super._();

  factory _$_MoviesDetails.fromJson(Map<String, dynamic> json) =>
      _$$_MoviesDetailsFromJson(json);

  @override
  final bool adult;
  @override
  final int id;
  @override
  @JsonKey(name: 'original_title', defaultValue: '')
  final String originalTitle;
  @override
  @JsonKey(name: 'overview', defaultValue: '')
  final String description;
  @override
  @JsonKey(name: 'release_date', defaultValue: '')
  final String releaseDate;

  @override
  String toString() {
    return 'MovieDetails(adult: $adult, id: $id, originalTitle: $originalTitle, description: $description, releaseDate: $releaseDate)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _MoviesDetails &&
            (identical(other.adult, adult) || other.adult == adult) &&
            (identical(other.id, id) || other.id == id) &&
            (identical(other.originalTitle, originalTitle) ||
                other.originalTitle == originalTitle) &&
            (identical(other.description, description) ||
                other.description == description) &&
            (identical(other.releaseDate, releaseDate) ||
                other.releaseDate == releaseDate));
  }

  @override
  int get hashCode => Object.hash(
      runtimeType, adult, id, originalTitle, description, releaseDate);

  @JsonKey(ignore: true)
  @override
  _$MoviesDetailsCopyWith<_MoviesDetails> get copyWith =>
      __$MoviesDetailsCopyWithImpl<_MoviesDetails>(this, _$identity);

  @override
  Map<String, dynamic> toJson() {
    return _$$_MoviesDetailsToJson(this);
  }
}

abstract class _MoviesDetails extends MovieDetails {
  const factory _MoviesDetails(
      {required bool adult,
      required int id,
      @JsonKey(name: 'original_title', defaultValue: '')
          required String originalTitle,
      @JsonKey(name: 'overview', defaultValue: '')
          required String description,
      @JsonKey(name: 'release_date', defaultValue: '')
          required String releaseDate}) = _$_MoviesDetails;
  const _MoviesDetails._() : super._();

  factory _MoviesDetails.fromJson(Map<String, dynamic> json) =
      _$_MoviesDetails.fromJson;

  @override
  bool get adult;
  @override
  int get id;
  @override
  @JsonKey(name: 'original_title', defaultValue: '')
  String get originalTitle;
  @override
  @JsonKey(name: 'overview', defaultValue: '')
  String get description;
  @override
  @JsonKey(name: 'release_date', defaultValue: '')
  String get releaseDate;
  @override
  @JsonKey(ignore: true)
  _$MoviesDetailsCopyWith<_MoviesDetails> get copyWith =>
      throw _privateConstructorUsedError;
}
