import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:move_app/presentation/common_widgets/common_wiedgets.dart';
import 'package:move_app/presentation/detail_screen/movie_datail_populated.dart';
import '../../logic/buisness_logic.dart';

class MovieDeatail extends StatelessWidget {
 
  final int movieID;
  const MovieDeatail({ Key? key, required this.movieID}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Movie Detail'),
      ),
      body: BlocBuilder<MovieDetailCubit, MovieDetailState>(
        builder: (context, state) {
          switch (state.status) {
            case MovieDetailStatus.loading:
              context.read<MovieDetailCubit>().fetchMovieDetail(movieID.toString());
              return const Center(child: MovieLoading());
            case MovieDetailStatus.failure:
              return const Center(child: MovieError());
              case MovieDetailStatus.success:
            final movieDetail = state.movieDetail;
              return MovieDetailPopulated(movieDetail: movieDetail);
            default:
            return const MovieError();
          }
        },
      ),
    );
  }
}