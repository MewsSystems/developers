import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:movies/src/api/movie_search_api.dart';
import 'package:movies/src/blocs/genres_cubit.dart';
import 'package:movies/src/blocs/movie_search_bloc.dart';
import 'package:movies/src/model/movie_search/movie_search_state.dart';
import 'package:movies/src/pages/search/result_list.dart';
import 'package:movies/src/pages/search/search_bar.dart';
import 'package:movies/src/pages/search/top_button.dart';

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
  bool showTopButton = false;
  @override
  void initState() {
    super.initState();
    // Initialize the categories data
    context.read<GenresCubit>().fetch();

    // Listen to query changes
    searchQueryController.addListener(() {
      // Emit the event that the query changed
      context
          .read<MovieSearchBloc>()
          .add(MovieQueryChanged(searchQueryController.text));
      // Scroll the list back up
      if (resultsScrollController.hasClients) {
        //_scrollController.jumpTo(0);
      }
      // Reset the paging count
      resultsPagingController.nextPageKey = 1;
    });

    resultsScrollController.addListener(() {
      if (resultsScrollController.position.pixels > 0 && !showTopButton) {
        setState(() {
          showTopButton = true;
        });
      } else if (resultsScrollController.position.pixels == 0 &&
          showTopButton) {
        setState(() {
          showTopButton = false;
        });
      }
    });

    // Load the next results page using the search query
    resultsPagingController.addPageRequestListener((pageKey) async {
      context
          .read<MovieSearchBloc>()
          .add(NeedNextMoviePage(searchQueryController.text, pageKey + 1));
    });
  }

  @override
  Widget build(BuildContext context) => Scaffold(
        floatingActionButton: showTopButton ? const TopButton() : null,
        body: SafeArea(
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              const SearchBar(),
              Flexible(
                child: BlocConsumer<MovieSearchBloc, MovieSearchState>(
                  listener: (context, state) {
                    state.when(
                      result: (movies, isLastPage) {
                        resultsPagingController.itemList = movies;
                        if (isLastPage) {
                          // Tell the paging controller that last page is reached
                          resultsPagingController.nextPageKey = null;
                        } else if (movies.length > 20) {
                          // Set the next page value according to the items count
                          resultsPagingController.nextPageKey =
                              movies.length ~/ 20;
                        }
                      },
                      error: (error) {},
                    );
                  },
                  builder: (context, state) => state.when(
                    error: (exception) {
                      if (exception is NoMoviesFoundError) {
                        return Center(
                          child: Text(
                            AppLocalizations.of(context).noMoviesFound,
                            style: const TextStyle(fontSize: 16),
                          ),
                        );
                      }
                      if (exception is MovieRequestError) {
                        return Center(
                          child:
                              Text(AppLocalizations.of(context).errorHappened),
                        );
                      } else {
                        return const Offstage();
                      }
                    },
                    result: (movies, isLastPage) => ResultList(
                      movies: movies,
                      pagingController: resultsPagingController,
                      scrollController: resultsScrollController,
                    ),
                  ),
                ),
              ),
            ],
          ),
        ),
      );
}
