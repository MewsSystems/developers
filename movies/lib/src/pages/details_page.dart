import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/constants.dart';
import 'package:movies/src/blocs/selected_movie_bloc.dart';
import 'package:movies/src/components/poster.dart';
import 'package:movies/src/model/movie.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

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
                CustomScrollView(
                  slivers: [
                    SliverAppBar(
                      expandedHeight: 200,
                      flexibleSpace: FlexibleSpaceBar(
                        background: Hero(
                          tag: movie.id,
                          child: CachedNetworkImage(
                            fit: BoxFit.fitWidth,
                            imageUrl: movie.backdrop,
                            placeholder: (context, _) =>
                                const CircularProgressIndicator(),
                          ),
                        ),
                      ),
                    ),
                    SliverToBoxAdapter(
                      child: Container(
                        height: 216,
                        margin: const EdgeInsets.all(16),
                        child: Row(
                          children: [
                            Poster(
                              url: movie.smallPoster,
                              heroTag: movie.id,
                            ),
                            const SizedBox(width: 16),
                            Expanded(
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(
                                    movie.title,
                                    style: const TextStyle(fontSize: 28),
                                  ),
                                  Text(movie.voteAverage.toString())
                                ],
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                    SliverToBoxAdapter(
                      child: Container(
                        margin: const EdgeInsets.all(16),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: [
                            Text(AppLocalizations.of(context).synopsis, style: TextStyle(fontSize: 24),),
                            Text(movie.overview),
                            SizedBox(
                              height: 3000,
                            )
                          ],
                        ),
                      ),
                    )
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
