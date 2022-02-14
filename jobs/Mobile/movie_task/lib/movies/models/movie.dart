import 'package:equatable/equatable.dart';

class Movie extends Equatable {
  const Movie({
    required this.id,
    required this.title,
    required this.body,
    this.image,
  });

  factory Movie.fromJson(Map<String, dynamic> json) => Movie(
        id: json['id'] as int,
        title: json['title'] as String,
        body: json['overview'] as String,
        image: json['poster_path'] as String?,
      );

  final int id;
  final String title;
  final String body;
  final String? image;

  @override
  List<Object> get props => [
        id,
        title,
        body,
      ];
}
