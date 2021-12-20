import 'package:equatable/equatable.dart';
import 'package:flutter/material.dart';

class MovieDetail extends Equatable {

  static final empty = MovieDetail(
    budget: 0, 
    genres: '',
    overview: '',
    image: Image.asset('assets/images/no-image.png'),
    title:''
  );

  final int budget;
  final String genres;
  final String overview;
  final Image image;
  final String title;

  const MovieDetail(
    {required this.budget, 
    required this.genres,   
    required this.overview,
    required this.image,
    required this.title});

  @override
  List<Object?> get props => [budget, genres, overview, title];
}




