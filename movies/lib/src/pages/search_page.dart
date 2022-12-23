import 'package:entry/entry.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:movies/src/blocs/movie_search_bloc.dart';
import 'package:movies/src/blocs/selected_movie_bloc.dart';
import 'package:movies/src/components/poster.dart';
import 'package:movies/src/model/movie.dart';
import 'package:movies/src/pages/details_page.dart';

/// Displays a search bar and the list of results
class SearchPage extends StatelessWidget {
  const SearchPage({
    super.key,
  });

  static const routeName = '/';
  @override
  Widget build(BuildContext context) => BlocListener<SelectedMovieBloc, Movie?>(
        listener: (context, state) {
          if (state != null) {
            Navigator.of(context).restorablePushNamed(
              DetailsPage.routeName,
            );
          }
        },
        child: Scaffold(
          body: SafeArea(
            child: Column(
              children: [
                Padding(
                  padding: const EdgeInsets.all(16),
                  child: TextField(
                    decoration: InputDecoration(
                      suffixIcon: const Icon(Icons.search),
                      hintText: AppLocalizations.of(context).searchMovie,
                    ),
                    onChanged: (query) => context
                        .read<MovieSearchBloc>()
                        .add(MovieQueryChanged(query)),
                  ),
                ),
                Expanded(
                  child: BlocBuilder<MovieSearchBloc, List<Movie>>(
                      builder: (context, movies) => ListView.builder(
                            // allows the ListView to restore the scroll position
                            restorationId: 'sampleItemListView',
                            itemCount: movies.length,
                            itemBuilder: (BuildContext context, int index) {
                              final movie = movies[index];

                              return Entry.all(
                                child: GestureDetector(
                                  onTap: () {
                                    BlocProvider.of<SelectedMovieBloc>(context)
                                        .add(SelectMovie(movie));
                                  },
                                  child: Container(
                                    height: 128,
                                    margin: const EdgeInsets.symmetric(
                                      horizontal: 16,
                                      vertical: 8,
                                    ),
                                    child: Row(
                                      children: [
                                        Poster(
                                          url: movie.smallPoster,
                                          heroTag: movie.id,
                                        ),
                                        const SizedBox(width: 16),
                                        Expanded(
                                          child: Column(
                                            crossAxisAlignment:
                                                CrossAxisAlignment.start,
                                            children: [
                                              Flexible(
                                                child: Text(
                                                  movie.title,
                                                  overflow: TextOverflow.fade,
                                                  style: const TextStyle(
                                                      fontSize: 24),
                                                ),
                                              ),
                                              Wrap(
                                                spacing: 8,
                                                children: [
                                                  Chip(
                                                    label: Text(
                                                      movie.releaseDate.year
                                                          .toString(),
                                                    ),
                                                  ),
                                                  Chip(
                                                    avatar: const Icon(
                                                      Icons.star,
                                                      size: 16,
                                                    ),
                                                    label: Text(
                                                      movie.voteAverage
                                                          .toString(),
                                                    ),
                                                  ),
                                                ],
                                              )
                                            ],
                                          ),
                                        )
                                      ],
                                    ),
                                  ),
                                ),
                              );
                            },
                          )),
                ),
              ],
            ),
          ),
        ),
      );
}
