import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/src/blocs/selected_movie_bloc.dart';
import 'package:movies/src/pages/movie.dart';

/// Displays detailed information about a movie
class DetailsPage extends StatelessWidget {
  const DetailsPage({
    super.key,
  });

  static const routeName = '/sample_item';

  @override
  Widget build(BuildContext context) =>
      BlocBuilder<SelectedMovieBloc, Movie?>(builder: (context, movie) {
        if (movie == null) {
          return const CircularProgressIndicator();
        }

        return WillPopScope(
          onWillPop: () async {
            context.read<SelectedMovieBloc>().add(DeselectMovie());

            return true;
          },
          child: Scaffold(
            body: Stack(
              children: [
                Column(
                  children: [
                    Hero(
                      tag: movie.id,
                      child: CachedNetworkImage(imageUrl: movie.posterUrl),
                    ),
                    Hero(
                      tag: movie.id + movie.title,
                      child: Text(
                        movie.title,
                        style: const TextStyle(fontSize: 32),
                      ),
                    ),
                  ],
                ),
                Positioned(
                  top: 8,
                  left: 8,
                  child: SafeArea(
                    child: FloatingActionButton.small(
                      child: const Icon(Icons.arrow_back),
                      onPressed: () {
                        context.read<SelectedMovieBloc>().add(DeselectMovie());
                        Navigator.pop(context);
                      },
                    ),
                  ),
                )
              ],
            ),
          ),
        );
      });
}
