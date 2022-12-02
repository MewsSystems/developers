// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'detailed_movie_model.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

DetailedMovie _$DetailedMovieFromJson(Map<String, dynamic> json) =>
    DetailedMovie(
      id: json['id'] as int,
      backdropPath: json['backdrop_path'] as String?,
      posterPath: json['poster_path'] as String?,
      originalTitle: json['original_title'] as String,
      tagline: json['tagline'] as String?,
      overview: json['overview'] as String?,
      releaseDate: json['release_date'] as String?,
      budget: json['budget'] as int,
      revenue: json['revenue'] as int,
      voteAverage: (json['vote_average'] as num).toDouble(),
      voteCount: json['vote_count'] as int,
    );
