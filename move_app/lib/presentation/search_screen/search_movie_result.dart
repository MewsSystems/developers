import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:move_app/presentation/common_widgets/common_wiedgets.dart';
import 'package:move_app/presentation/search_screen/movie_populated.dart';
import '../../logic/buisness_logic.dart';

class SearchMovieResult extends StatefulWidget {
  const SearchMovieResult({Key? key}) : super(key: key);

  @override
  SearchMovieResultState createState() => SearchMovieResultState();
}

class SearchMovieResultState extends State<SearchMovieResult> {
  @override
  Widget build(BuildContext context) {
    return BlocBuilder<PreviewCubit, MoviesState>(
      builder: (context, state) {
        switch (state.status) {
          case MoviesStatus.initial:
            return const MovieEmpty();
          case MoviesStatus.loading:
            return const MovieLoading();
          case MoviesStatus.failure:
            return const MovieError();
          case MoviesStatus.success:
            return MoviePopulated(movies: state.movies);
          default:
          return const MovieError();
        }
      },
    );
  }
}
