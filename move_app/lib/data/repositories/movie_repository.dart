import 'package:flutter/material.dart';
import 'package:move_app/constants.dart';
import 'package:move_app/data/data_providers/move_api.dart';
import 'models.dart';
import '../models/models_raw.dart';

class MovieRepository {
  
  final MovieAPI _moveAPI;

  MovieRepository({MovieAPI? moveAPI}): 
  _moveAPI = moveAPI ?? MovieAPI();

  Future<List<Movie>> fetchMovieDetail(String query) async {
  
    final rawMoviews = await _moveAPI.fetchRawMovies(query);

    List<Movie> movies = [];

    for (MovieRaw rowMovie in rawMoviews) {

      var releasedDate = rowMovie.releaseDate == null ? 
        '' : rowMovie.releaseDate!;
      
      var movieImage = rowMovie.posterPath == null ?
        Image.asset('assets/images/no-image.png') : Image.network(API.imageURL+ rowMovie.posterPath!);

      movies.add(
        Movie(id: rowMovie.id, title: rowMovie.title, releaseDate: releasedDate, image: movieImage)
      );
    }

    return movies;
  }

  Future<MovieDetail>fetchMovie(String id) async {
    
    final rawMovieDetail = await _moveAPI.fetchRawMovieDetail(id);

    String genres = '';

    for (var genre in rawMovieDetail.genres) {
      genres = genres + genre.name + '  ';
    }
    var overview = rawMovieDetail.overview == null ? 
        '' : rawMovieDetail.overview!;

    var movieImage = rawMovieDetail.posterPath == null ?
        Image.asset('assets/images/no-image.png') : Image.network(API.imageURL+ rawMovieDetail.posterPath!);

    return MovieDetail(
      budget: rawMovieDetail.budget,
      genres: genres, 
      overview: overview, 
      image: movieImage,
      title: rawMovieDetail.title);
  }
}
