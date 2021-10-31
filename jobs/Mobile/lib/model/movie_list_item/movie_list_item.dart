import 'package:freezed_annotation/freezed_annotation.dart';

part 'movie_list_item.freezed.dart';
part 'movie_list_item.g.dart';

@freezed
class MovieListItem with _$MovieListItem {
  const MovieListItem._();

  const factory MovieListItem(
      {required bool adult,
      required int id,
      @JsonKey(name: 'original_title', defaultValue: '') required String originalTitle,
      @JsonKey(name: 'overview', defaultValue: '') required String description,
      @JsonKey(name: 'release_date', defaultValue: '') required String releaseDate}) = _MovieListItem;

  factory MovieListItem.fromJson(Map<String, dynamic> json) => _$MovieListItemFromJson(json);
}
