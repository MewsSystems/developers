import 'package:cached_network_image/cached_network_image.dart';
import 'package:entry/entry.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:movies/constants.dart';
import 'package:movies/src/blocs/selected_movie_bloc.dart';
import 'package:movies/src/pages/details_page.dart';
import 'package:movies/src/model/movie.dart';

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
                  ),
                ),
                Expanded(
                  child: ListView.builder(
                    // Providing a restorationId allows the ListView to restore the
                    // scroll position when a user leaves and returns to the app after it
                    // has been killed while running in the background.
                    restorationId: 'sampleItemListView',
                    itemCount: items.length,
                    itemBuilder: (BuildContext context, int index) {
                      final item = items[index];

                      return Entry.all(
                        delay: const Duration(milliseconds: 250),
                        child: GestureDetector(
                          onTap: () {
                            BlocProvider.of<SelectedMovieBloc>(context)
                                .add(SelectMovie(item));
                          },
                          child: Container(
                            height: 168,
                            margin: const EdgeInsets.all(16),
                            child: Row(
                              children: [
                                if(item.posterPath != null)
                                ClipRRect(
                                  borderRadius: const BorderRadius.all(
                                      Radius.circular(16)),
                                  child: Hero(
                                    tag: item.id,
                                    child: CachedNetworkImage(
                                        imageUrl: item.posterPath ?? ''),
                                  ),
                                ),
                                const SizedBox(width: 16),
                                Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    Hero(
                                      tag: item.id.toString() + item.title,
                                      child: Text(
                                        item.originalTitle,
                                        style: const TextStyle(fontSize: 24),
                                      ),
                                    ),
                                    const SizedBox(height: 4),
                                    Text(item.releaseDate.year.toString()),
                                    Text(item.voteAverage.toString())
                                  ],
                                )
                              ],
                            ),
                          ),
                        ),
                      );
                    },
                  ),
                ),
              ],
            ),
          ),
        ),
      );
}
