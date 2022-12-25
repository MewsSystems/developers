import 'package:animations/animations.dart';
import 'package:entry/entry.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:infinite_scroll_pagination/infinite_scroll_pagination.dart';
import 'package:movies/src/blocs/genres_cubit.dart';
import 'package:movies/src/blocs/movie_search_bloc.dart';
import 'package:movies/src/blocs/selected_movie_bloc.dart';
import 'package:movies/src/components/genre_chips.dart';
import 'package:movies/src/components/movie_chips.dart';
import 'package:movies/src/components/poster.dart';
import 'package:movies/src/model/movie.dart';
import 'package:movies/src/pages/details_page.dart';

/// Displays a search bar and the list of results
class SearchPage extends StatefulWidget {
  const SearchPage({
    super.key,
  });

  static const routeName = '/';

  @override
  State<SearchPage> createState() => _SearchPageState();
}

class _SearchPageState extends State<SearchPage> {
  final queryController = TextEditingController();

  final _pagingController = PagingController<int, Movie>(firstPageKey: 1);

  final _scrollController = ScrollController();

  @override
  void initState() {
    super.initState();
    // Initialize the categories data
    context.read<GenresCubit>().fetch();

    _pagingController.addPageRequestListener((pageKey) async {
      context
          .read<MovieSearchBloc>()
          .add(NeedNextMoviePage(queryController.text, pageKey + 1));
    });
  }

  @override
  Widget build(BuildContext context) => Scaffold(
        body: SafeArea(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Padding(
                padding: const EdgeInsets.all(16),
                child: TextField(
                  controller: queryController,
                  autofocus: true,
                  decoration: InputDecoration(
                    prefixIcon: const Icon(Icons.search),
                    hintText: AppLocalizations.of(context).searchMovie,
                  ),
                  onChanged: (query) {
                    // Emit the event that the query changed
                    context
                        .read<MovieSearchBloc>()
                        .add(MovieQueryChanged(query));
                    // Scroll the list back up
                    _scrollController.jumpTo(0);
                    // Reset the paging count
                    _pagingController.nextPageKey = 1;
                  },
                ),
              ),
              Flexible(
                child: BlocConsumer<MovieSearchBloc, List<Movie>>(
                  listener: (context, state) {
                    _pagingController.itemList = state;
                    if (state.length > 20) {
                      _pagingController.nextPageKey = state.length ~/ 20;
                    }
                  },
                  builder: (context, movies) => AnimatedContainer(
                    duration: const Duration(milliseconds: 600),
                    curve: Curves.easeInOut,
                    height:
                        movies.isEmpty ? 0 : MediaQuery.of(context).size.height,
                    child: PagedListView<int, Movie>(
                      pagingController: _pagingController,
                      scrollController: _scrollController,
                      shrinkWrap: true,
                      // allows the ListView to restore the scroll position
                      restorationId: 'sampleItemListView',
                      builderDelegate: PagedChildBuilderDelegate<Movie>(
                        itemBuilder: (context, item, index) {
                          final movie = movies[index];

                          return Entry.all(
                            delay: const Duration(milliseconds: 80),
                            duration: const Duration(milliseconds: 200),
                            xOffset: -MediaQuery.of(context).size.width / 2,
                            child: OpenContainer(
                              transitionType:
                                  ContainerTransitionType.fadeThrough,
                              openBuilder: (context, action) =>
                                  const DetailsPage(),
                              closedColor:
                                  Theme.of(context).scaffoldBackgroundColor,
                              openColor:
                                  Theme.of(context).scaffoldBackgroundColor,
                              middleColor:
                                  Theme.of(context).scaffoldBackgroundColor,
                              closedElevation: 0,
                              closedBuilder: (context, action) =>
                                  GestureDetector(
                                onTap: () {
                                  context
                                      .read<SelectedMovieBloc>()
                                      .add(SelectMovie(movie));
                                  action();
                                },
                                child: Container(
                                  height: 168,
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
                                                  fontSize: 22,
                                                ),
                                              ),
                                            ),
                                            const SizedBox(
                                              height: 8,
                                            ),
                                            GenreChips(
                                              genreIds: movie.genreIds,
                                            ),
                                            MovieChips(
                                              movie: movie,
                                            ),
                                          ],
                                        ),
                                      )
                                    ],
                                  ),
                                ),
                              ),
                            ),
                          );
                        },
                      ),
                    ),
                  ),
                ),
              ),
            ],
          ),
        ),
      );
}
