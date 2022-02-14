import 'package:equatable/equatable.dart';

class DetailedMovie extends Equatable {
  const DetailedMovie({
    required this.id,
    required this.title,
    required this.body,
    this.image = '',
    this.backdropImage = '',
    this.vote = 0.0,
    this.budget = 0,
    this.tagline = '',
  });

  factory DetailedMovie.fromJson(Map<String, dynamic> json) => DetailedMovie(
        id: json['id'] as int,
        title: json['title'] as String,
        body: json['overview'] as String,
        vote: json['vote_average'] as num,
        image: json['poster_path'] != null ? json['poster_path'] as String : '',
        backdropImage: json['backdrop_path'] != null
            ? json['backdrop_path'] as String
            : '',
        budget: json['budget'] as int,
        tagline: json['tagline'] as String,
      );

  final int id;
  final String title;
  final String body;
  final String image;
  final num vote;
  final String backdropImage;
  final int budget;
  final String tagline;

  @override
  List<Object> get props => [
        id,
        title,
        body,
      ];
}
