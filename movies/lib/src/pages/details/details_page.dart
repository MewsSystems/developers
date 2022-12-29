import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:movies/src/blocs/movie_details_cubit.dart';
import 'package:movies/src/blocs/selected_movie_bloc.dart';
import 'package:movies/src/components/genre_chips.dart';
import 'package:movies/src/components/movie_chip.dart';
import 'package:movies/src/components/movie_chips.dart';
import 'package:movies/src/components/movie_tile.dart';
import 'package:movies/src/components/poster.dart';
import 'package:movies/src/model/movie/movie.dart';
import 'package:movies/src/model/movie_details/movie_details_state.dart';
import 'package:movies/theme.dart';
import 'package:movies/utils.dart';
import 'package:sliver_tools/sliver_tools.dart';
import 'package:url_launcher/url_launcher.dart';

/// Displays detailed information about a movie
class DetailsPage extends StatelessWidget {
  const DetailsPage({
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
                body: Stack(
                  children: [
                    CustomScrollView(
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
                                  color:
                                      Theme.of(context).scaffoldBackgroundColor,
                                ),
                              ],
                            ),
                            padding: const EdgeInsets.all(16),
                            child: MovieTile(
                              movie: movie,
                              showOriginalTitle: true,
                              posterHeight:
                                  MediaQuery.of(context).orientation ==
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
                            child: BlocBuilder<MovieDetailsCubit,
                                MovieDetailsState>(
                              builder: (context, state) => state.when(
                                loading: () => const Center(
                                  child: CircularProgressIndicator(),
                                ),
                                noSelection: () => const Offstage(),
                                error: (_) => Text(
                                  AppLocalizations.of(context).errorHappened,
                                ),
                                details: (movieDetails) => Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    if (movieDetails.tagline?.isNotEmpty ??
                                        false) ...[
                                      Center(
                                        child: Text(
                                          movieDetails.tagline ?? '',
                                          textAlign: TextAlign.center,
                                          style: const TextStyle(
                                            fontSize: 22,
                                            fontStyle: FontStyle.italic,
                                          ),
                                        ),
                                      ),
                                      const SizedBox(height: 24),
                                    ],
                                    Text(
                                      AppLocalizations.of(context).synopsis,
                                      style: headingStyle,
                                    ),
                                    const SizedBox(height: 8),
                                    Text(
                                      movie.overview,
                                      style: const TextStyle(fontSize: 16),
                                    ),
                                    const SizedBox(height: 24),

                                    Center(
                                      child: Wrap(
                                        spacing: 8,
                                        children: [
                                          if (movieDetails.runtime != null)
                                            MovieChip(
                                              icon: Icons.schedule,
                                              label: formatMinutes(
                                                movieDetails.runtime ?? 0,
                                              ),
                                            ),
                                          MovieChip(label: movieDetails.status),
                                        ],
                                      ),
                                    ),
                                    Center(
                                      child: Wrap(
                                        spacing: 8,
                                        children: [
                                          if (movieDetails.budget != 0)
                                            MovieChip(
                                              label:
                                                  'Budget: \$${movieDetails.budget.toInt()}',
                                            ),
                                          if (movieDetails.revenue != 0)
                                            MovieChip(
                                              label:
                                                  'Revenue: \$${movieDetails.revenue}',
                                            ),
                                        ],
                                      ),
                                    ),
                                    const SizedBox(height: 8),
                                    Center(
                                      child: ElevatedButton.icon(
                                        onPressed: () => launchUrl(
                                          Uri.parse(movieDetails.homepage),
                                        ),
                                        icon: const Icon(Icons.open_in_new),
                                        label: Text(
                                          AppLocalizations.of(context)
                                              .visitWebsite,
                                        ),
                                      ),
                                    ),
                                    const SizedBox(height: 24),
                                    // Spoken languages
                                    Text(
                                      AppLocalizations.of(context)
                                          .spokenLanguages,
                                      style: headingStyle,
                                    ),
                                    Wrap(
                                      spacing: 8,
                                      children: [
                                        for (var language
                                            in movieDetails.spokenLanguages)
                                          MovieChip(
                                            label: language['name'] as String,
                                          ),
                                      ],
                                    ),
                                    const SizedBox(height: 24),

                                    // Production countries
                                    Text(
                                      AppLocalizations.of(context)
                                          .prodCountries,
                                      style: headingStyle,
                                    ),

                                    Wrap(
                                      spacing: 8,
                                      children: [
                                        for (var country
                                            in movieDetails.productionCountries)
                                          MovieChip(
                                            label: country['name'] as String,
                                          ),
                                      ],
                                    ),

                                    const SizedBox(height: 24),

                                    // Production companies
                                    Text(
                                      AppLocalizations.of(context)
                                          .prodCompanies,
                                      style: headingStyle,
                                    ),
                                    Wrap(
                                      spacing: 8,
                                      children: [
                                        for (var company in movieDetails
                                            .productionCompanies) ...[
                                          Chip(
                                            backgroundColor: chipColor,
                                            label: Text(
                                              company['name'] as String,
                                            ),
                                            avatar: company['logo_path'] == null
                                                ? null
                                                : CachedNetworkImage(
                                                    imageUrl: getApiImageUrl(
                                                      company['logo_path']
                                                          as String,
                                                      300,
                                                    ),
                                                  ),
                                          ),
                                          const SizedBox(height: 8),
                                        ],
                                      ],
                                    ),
                                    const SizedBox(height: 8),
                                  ],
                                ),
                              ),
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
        },
      );
}
