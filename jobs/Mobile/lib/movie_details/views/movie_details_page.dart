import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:mews_imdb/movie_details/cubit/credits/credits_cubit.dart';
import 'package:mews_imdb/movie_details/cubit/movie/movie_cubit.dart';
import 'package:mews_imdb/movie_details/views/movie_details_view.dart';
import 'package:movie_repository/movie_repository.dart' hide MovieDetails;

class MovieDetailsPage extends StatelessWidget {
  const MovieDetailsPage({
    Key? key,
  }) : super(key: key);

  static Route route(int movieId, MovieRepository movieRepository) =>
      MaterialPageRoute<void>(
        builder: (context) => MultiBlocProvider(
          providers: [
            BlocProvider(
              create: (context) => MovieCubit(
                movieId: movieId,
                movieRepository: movieRepository,
              )..startLoading(),
            ),
            BlocProvider(
              create: (context) => CreditsCubit(
                movieId: movieId,
                movieRepository: movieRepository,
              )..startLoading(),
            ),
          ],
          child: const MovieDetailsPage(),
        ),
      );

  @override
  Widget build(BuildContext context) => const MovieDetailsView();
}
