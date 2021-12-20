import 'package:equatable/equatable.dart';
import 'package:move_app/data/repositories/models.dart';
import 'package:move_app/data/repositories/movie_repository.dart';

enum MovieDetailStatus{loading, success, failure}

class MovieDetailState extends Equatable {

  final MovieDetailStatus status;
  final MovieDetail movieDetail;

   MovieDetailState({
     this.status = MovieDetailStatus.loading,
     MovieDetail? movieDetail}) : 
     movieDetail = movieDetail ?? 
     MovieDetail.empty;

   MovieDetailState copyWith({
     MovieDetailStatus? status,
     MovieDetail? movieDetail
    }) {
    return MovieDetailState(
      status: status ?? this.status,
      movieDetail: movieDetail ?? this.movieDetail
    );
  }

  @override
  List<Object?> get props => [status, movieDetail];
}