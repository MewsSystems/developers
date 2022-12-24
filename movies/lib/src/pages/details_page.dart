import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/constants.dart';
import 'package:movies/src/blocs/selected_movie_bloc.dart';
import 'package:movies/src/components/genre_chips.dart';
import 'package:movies/src/components/movie_chips.dart';
import 'package:movies/src/components/poster.dart';
import 'package:movies/src/model/movie.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:sliver_tools/sliver_tools.dart';

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
          child: SafeArea(
            child: Scaffold(
              body: Stack(
                children: [
                  CustomScrollView(
                    slivers: [
                      SliverAppBar(
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
                        expandedHeight: 200,
                        flexibleSpace: FlexibleSpaceBar(
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
                        ),
                      ),
                      SliverPinnedHeader(
                        child: Container(
                          color: Theme.of(context).scaffoldBackgroundColor,
                          padding: const EdgeInsets.all(16),
                          child: Row(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Poster(
                                url: movie.smallPoster,
                                heroTag: movie.id,
                                height: 184,
                              ),
                              const SizedBox(width: 16),
                              Expanded(
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Text(
                                      movie.title,
                                      style: const TextStyle(fontSize: 28,),
                                      
                                    ),
                                    if (movie.title != movie.originalTitle)
                                      Text('(${movie.originalTitle})'),
                                    const SizedBox(height: 4),
                                    GenreChips(genreIds: movie.genreIds),
                                    MovieChips(movie: movie),
                                  ],
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                      SliverToBoxAdapter(
                        child: Container(
                          padding: const EdgeInsets.all(16),
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.start,
                            children: [
                              Text(
                                AppLocalizations.of(context).synopsis,
                                style: const TextStyle(fontSize: 24),
                              ),
                              const SizedBox(height: 8),
                              Text(
                                movie.overview,
                                style: const TextStyle(fontSize: 16),
                              ),
                              SizedBox(
                                height: 3000,
                              )
                            ],
                          ),
                        ),
                      )
                    ],
                  ),
                  /*Positioned(
                    top: 8,
                    left: 8,
                    child: SafeArea(
                      child: FloatingActionButton.small(
                        child: const Icon(Icons.arrow_back),
                        onPressed: () {
                          context
                              .read<SelectedMovieBloc>()
                              .add(DeselectMovie());
                          Navigator.pop(context);
                        },
                      ),
                    ),
                  )*/
                ],
              ),
            ),
          ),
        );
      });
}
