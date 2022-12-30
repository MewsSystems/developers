import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:movies/src/blocs/movie_details_cubit.dart';
import 'package:movies/src/components/movie_chip.dart';
import 'package:movies/src/model/movie/movie.dart';
import 'package:movies/src/model/movie_details/movie_details.dart';
import 'package:movies/src/model/movie_details/movie_details_state.dart';
import 'package:movies/theme.dart';
import 'package:movies/utils.dart';
import 'package:url_launcher/url_launcher.dart';

class DetailsList extends StatelessWidget {
  const DetailsList({
    Key? key,
    required this.movie,
    required this.headingStyle,
  }) : super(key: key);

  final Movie movie;
  final TextStyle headingStyle;

  @override
  Widget build(BuildContext context) =>
      BlocBuilder<MovieDetailsCubit, MovieDetailsState>(
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
              if (movieDetails.tagline?.isNotEmpty ?? false) ...[
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
                        label: 'Budget: \$${movieDetails.budget.toInt()}',
                      ),
                    if (movieDetails.revenue != 0)
                      MovieChip(
                        label: 'Revenue: \$${movieDetails.revenue}',
                      ),
                  ],
                ),
              ),
              const SizedBox(height: 8),

              Center(
                child: Wrap(
                  spacing: 8,
                  children: [
                    ElevatedButton.icon(
                      onPressed: () => launchUrl(
                        Uri.parse(movieDetails.homepage),
                      ),
                      icon: const Icon(Icons.open_in_new),
                      label: Text(
                        AppLocalizations.of(context).visitWebsite,
                      ),
                    ),
                    ElevatedButton.icon(
                      onPressed: () => launchUrl(
                        Uri.parse(movieDetails.imdbUrl),
                      ),
                      icon: const Icon(Icons.open_in_new),
                      label: Text(
                        AppLocalizations.of(context).imdb,
                      ),
                    ),
                  ],
                ),
              ),
              const SizedBox(height: 24),

              // Spoken languages
              if (movieDetails.spokenLanguages.isNotEmpty) ...[
                Text(
                  AppLocalizations.of(context).spokenLanguages,
                  style: headingStyle,
                ),
                Wrap(
                  spacing: 8,
                  children: [
                    for (var language in movieDetails.spokenLanguages)
                      MovieChip(
                        label: language['name'] as String,
                      ),
                  ],
                ),
                const SizedBox(height: 24),
              ],

              // Production countries
              if (movieDetails.productionCountries.isNotEmpty) ...[
                Text(
                  AppLocalizations.of(context).prodCountries,
                  style: headingStyle,
                ),
                Wrap(
                  spacing: 8,
                  children: [
                    for (var country in movieDetails.productionCountries)
                      MovieChip(
                        label: country['name'] as String,
                      ),
                  ],
                ),
                const SizedBox(height: 24),
              ],

              // Production companies
              if (movieDetails.productionCompanies.isNotEmpty) ...[
                Text(
                  AppLocalizations.of(context).prodCompanies,
                  style: headingStyle,
                ),
                Wrap(
                  spacing: 8,
                  children: [
                    for (var company in movieDetails.productionCompanies) ...[
                      Chip(
                        backgroundColor: chipColor,
                        label: Text(
                          company['name'] as String,
                        ),
                        avatar: company['logo_path'] == null
                            ? null
                            : CachedNetworkImage(
                                imageUrl: getApiImageUrl(
                                  company['logo_path'] as String,
                                  300,
                                ),
                              ),
                      ),
                      const SizedBox(height: 8),
                    ],
                  ],
                ),
              ],
              const SizedBox(height: 8),
            ],
          ),
        ),
      );
}
