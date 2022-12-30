import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/src/blocs/selected_movie_bloc.dart';
import 'package:movies/src/components/movie_tile.dart';
import 'package:movies/src/model/movie/movie.dart';
import 'package:movies/src/pages/details/movie_details_list.dart';
import 'package:sliver_tools/sliver_tools.dart';

/// Displays detailed information about the selected movie
class MovieDetailsPage extends StatelessWidget {
  const MovieDetailsPage({
    super.key,
  });

  static const routeName = '/sample_item';

  final headingStyle = const TextStyle(
    fontSize: 22,
    fontWeight: FontWeight.bold,
    letterSpacing: 1,
  );

  @override
  Widget build(BuildContext context) => BlocBuilder<SelectedMovieBloc, Movie?>(
        builder: (context, movie) {
          if (movie == null) {
            return const Center(child: CircularProgressIndicator());
          }
          final hasBackdrop = movie.backdropPath != null;

          return WillPopScope(
            onWillPop: () async {
              context.read<SelectedMovieBloc>().add(DeselectMovie());

              return true;
            },
            child: SafeArea(
              child: Scaffold(
                body: CustomScrollView(
                  slivers: [
                    SliverAppBar(
                      backgroundColor:
                          Theme.of(context).scaffoldBackgroundColor,
                      automaticallyImplyLeading: false,
                      leading: IconButton(
                        onPressed: () {
                          context
                              .read<SelectedMovieBloc>()
                              .add(DeselectMovie());
                          Navigator.pop(context);
                        },
                        icon: const Icon(Icons.arrow_back),
                      ),
                      expandedHeight: hasBackdrop ? 200 : 0,
                      flexibleSpace: hasBackdrop
                          ? FlexibleSpaceBar(
                              background: Hero(
                                tag: movie.id,
                                child: CachedNetworkImage(
                                  fit: BoxFit.fitWidth,
                                  imageUrl: movie.backdrop,
                                  placeholder: (context, _) => const Center(
                                    child: CircularProgressIndicator(),
                                  ),
                                ),
                              ),
                            )
                          : null,
                    ),
                    SliverPinnedHeader(
                      child: Container(
                        decoration: BoxDecoration(
                          color: Theme.of(context).scaffoldBackgroundColor,
                          boxShadow: [
                            BoxShadow(
                              offset: const Offset(4, 0),
                              blurRadius: 4,
                              color: Theme.of(context).scaffoldBackgroundColor,
                            ),
                          ],
                        ),
                        padding: const EdgeInsets.all(16),
                        child: MovieTile(
                          movie: movie,
                          showOriginalTitle: true,
                          posterHeight: MediaQuery.of(context).orientation ==
                                  Orientation.portrait
                              ? 176
                              : 96,
                          titleSize: 28,
                        ),
                      ),
                    ),
                    SliverToBoxAdapter(
                      child: Container(
                        padding: const EdgeInsets.all(16),
                        child: DetailsList(
                          movie: movie,
                          headingStyle: headingStyle,
                        ),
                      ),
                    )
                  ],
                ),
              ),
            ),
          );
        },
      );
}
