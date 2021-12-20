import 'package:json_annotation/json_annotation.dart';
import 'package:equatable/equatable.dart';
part 'movie_raw.g.dart';

@JsonSerializable(fieldRename: FieldRename.snake)
class MovieRaw extends Equatable{
  final int id;
  final String title;
  final String? releaseDate;
  final String? posterPath;

 const MovieRaw(
    {required this.id,
    required this.title,
    this.releaseDate,
    this.posterPath});

  factory MovieRaw.fromJson(Map<String, dynamic> json) => _$MovieRawFromJson(json);

  Map<String, dynamic> toJson() => _$MovieRawToJson(this);

  @override
  List<Object?> get props => [id, title, releaseDate, posterPath];
}
