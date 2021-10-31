// coverage:ignore-file
// GENERATED CODE - DO NOT MODIFY BY HAND
// ignore_for_file: unused_element, deprecated_member_use, deprecated_member_use_from_same_package, use_function_type_syntax_for_parameters, unnecessary_const, avoid_init_to_null, invalid_override_different_default_values_named, prefer_expression_function_bodies, annotate_overrides, invalid_annotation_target

part of 'movie_list_item.dart';

// **************************************************************************
// FreezedGenerator
// **************************************************************************

T _$identity<T>(T value) => value;

final _privateConstructorUsedError = UnsupportedError(
    'It seems like you constructed your class using `MyClass._()`. This constructor is only meant to be used by freezed and you are not supposed to need it nor use it.\nPlease check the documentation here for more informations: https://github.com/rrousselGit/freezed#custom-getters-and-methods');

MovieListItem _$MovieListItemFromJson(Map<String, dynamic> json) {
  return _MovieListItem.fromJson(json);
}

/// @nodoc
class _$MovieListItemTearOff {
  const _$MovieListItemTearOff();

  _MovieListItem call(
      {required bool adult,
      required int id,
      @JsonKey(name: 'original_title') required String originalTitle,
      @JsonKey(name: 'overview') required String description,
      @JsonKey(name: 'release_date') required String releaseDate}) {
    return _MovieListItem(
      adult: adult,
      id: id,
      originalTitle: originalTitle,
      description: description,
      releaseDate: releaseDate,
    );
  }

  MovieListItem fromJson(Map<String, Object?> json) {
    return MovieListItem.fromJson(json);
  }
}

/// @nodoc
const $MovieListItem = _$MovieListItemTearOff();

/// @nodoc
mixin _$MovieListItem {
  bool get adult => throw _privateConstructorUsedError;
  int get id => throw _privateConstructorUsedError;
  @JsonKey(name: 'original_title')
  String get originalTitle => throw _privateConstructorUsedError;
  @JsonKey(name: 'overview')
  String get description => throw _privateConstructorUsedError;
  @JsonKey(name: 'release_date')
  String get releaseDate => throw _privateConstructorUsedError;

  Map<String, dynamic> toJson() => throw _privateConstructorUsedError;
  @JsonKey(ignore: true)
  $MovieListItemCopyWith<MovieListItem> get copyWith =>
      throw _privateConstructorUsedError;
}

/// @nodoc
abstract class $MovieListItemCopyWith<$Res> {
  factory $MovieListItemCopyWith(
          MovieListItem value, $Res Function(MovieListItem) then) =
      _$MovieListItemCopyWithImpl<$Res>;
  $Res call(
      {bool adult,
      int id,
      @JsonKey(name: 'original_title') String originalTitle,
      @JsonKey(name: 'overview') String description,
      @JsonKey(name: 'release_date') String releaseDate});
}

/// @nodoc
class _$MovieListItemCopyWithImpl<$Res>
    implements $MovieListItemCopyWith<$Res> {
  _$MovieListItemCopyWithImpl(this._value, this._then);

  final MovieListItem _value;
  // ignore: unused_field
  final $Res Function(MovieListItem) _then;

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
abstract class _$MovieListItemCopyWith<$Res>
    implements $MovieListItemCopyWith<$Res> {
  factory _$MovieListItemCopyWith(
          _MovieListItem value, $Res Function(_MovieListItem) then) =
      __$MovieListItemCopyWithImpl<$Res>;
  @override
  $Res call(
      {bool adult,
      int id,
      @JsonKey(name: 'original_title') String originalTitle,
      @JsonKey(name: 'overview') String description,
      @JsonKey(name: 'release_date') String releaseDate});
}

/// @nodoc
class __$MovieListItemCopyWithImpl<$Res>
    extends _$MovieListItemCopyWithImpl<$Res>
    implements _$MovieListItemCopyWith<$Res> {
  __$MovieListItemCopyWithImpl(
      _MovieListItem _value, $Res Function(_MovieListItem) _then)
      : super(_value, (v) => _then(v as _MovieListItem));

  @override
  _MovieListItem get _value => super._value as _MovieListItem;

  @override
  $Res call({
    Object? adult = freezed,
    Object? id = freezed,
    Object? originalTitle = freezed,
    Object? description = freezed,
    Object? releaseDate = freezed,
  }) {
    return _then(_MovieListItem(
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
class _$_MovieListItem extends _MovieListItem {
  const _$_MovieListItem(
      {required this.adult,
      required this.id,
      @JsonKey(name: 'original_title') required this.originalTitle,
      @JsonKey(name: 'overview') required this.description,
      @JsonKey(name: 'release_date') required this.releaseDate})
      : super._();

  factory _$_MovieListItem.fromJson(Map<String, dynamic> json) =>
      _$$_MovieListItemFromJson(json);

  @override
  final bool adult;
  @override
  final int id;
  @override
  @JsonKey(name: 'original_title')
  final String originalTitle;
  @override
  @JsonKey(name: 'overview')
  final String description;
  @override
  @JsonKey(name: 'release_date')
  final String releaseDate;

  @override
  String toString() {
    return 'MovieListItem(adult: $adult, id: $id, originalTitle: $originalTitle, description: $description, releaseDate: $releaseDate)';
  }

  @override
  bool operator ==(dynamic other) {
    return identical(this, other) ||
        (other.runtimeType == runtimeType &&
            other is _MovieListItem &&
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
  _$MovieListItemCopyWith<_MovieListItem> get copyWith =>
      __$MovieListItemCopyWithImpl<_MovieListItem>(this, _$identity);

  @override
  Map<String, dynamic> toJson() {
    return _$$_MovieListItemToJson(this);
  }
}

abstract class _MovieListItem extends MovieListItem {
  const factory _MovieListItem(
          {required bool adult,
          required int id,
          @JsonKey(name: 'original_title') required String originalTitle,
          @JsonKey(name: 'overview') required String description,
          @JsonKey(name: 'release_date') required String releaseDate}) =
      _$_MovieListItem;
  const _MovieListItem._() : super._();

  factory _MovieListItem.fromJson(Map<String, dynamic> json) =
      _$_MovieListItem.fromJson;

  @override
  bool get adult;
  @override
  int get id;
  @override
  @JsonKey(name: 'original_title')
  String get originalTitle;
  @override
  @JsonKey(name: 'overview')
  String get description;
  @override
  @JsonKey(name: 'release_date')
  String get releaseDate;
  @override
  @JsonKey(ignore: true)
  _$MovieListItemCopyWith<_MovieListItem> get copyWith =>
      throw _privateConstructorUsedError;
}
