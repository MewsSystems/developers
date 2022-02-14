import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_task/movies/models/models.dart';

class NavCubit extends Cubit<Movie> {
  NavCubit() : super(const Movie(id: -1, title: '', body: ''));

  Movie movie = const Movie(id: -1, title: '', body: '');

  void showDetails(Movie movie) => emit(movie);

  void popToSearch() => emit(movie);
}
