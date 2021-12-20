import 'package:flutter/material.dart';
import 'package:equatable/equatable.dart';

class Movie extends Equatable{
  final int id;
  final String title;
  final String releaseDate;
  final Image image;

 const Movie(
    {required this.id,
    required this.title,
    required this.releaseDate,
    required this.image});

  Movie copyWith({
    int? id,
    String? title,
    String? releaseDate,
    Image? image
  }) {
    return Movie(
      id: id ?? this.id,
      title: title ?? this.title,
      releaseDate: releaseDate ?? this.releaseDate,
      image: image ?? this.image,
    );
  }

  @override
  List<Object?> get props => [id, title, releaseDate];
}
