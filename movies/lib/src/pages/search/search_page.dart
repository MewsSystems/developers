import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:infinite_scroll_pagination/infinite_scroll_pagination.dart';
import 'package:movies/src/blocs/genres_cubit.dart';
import 'package:movies/src/blocs/movie_search_bloc.dart';
import 'package:movies/src/model/movie/movie.dart';
import 'package:movies/src/pages/search/result_list.dart';
import 'package:animations/animations.dart';
import 'package:entry/entry.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:infinite_scroll_pagination/infinite_scroll_pagination.dart';
import 'package:movies/src/api/movie_search_api.dart';
import 'package:movies/src/blocs/movie_search_bloc.dart';
import 'package:movies/src/blocs/selected_movie_bloc.dart';
import 'package:movies/src/components/genre_chips.dart';
import 'package:movies/src/components/movie_chips.dart';
import 'package:movies/src/components/poster.dart';
import 'package:movies/src/model/movie/movie.dart';
import 'package:movies/src/model/movie_search/movie_search_state.dart';
import 'package:movies/src/pages/details/details_page.dart';

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
                    if (_scrollController.hasClients) {
                      //_scrollController.jumpTo(0);
                    }
                    // Reset the paging count
                    _pagingController.nextPageKey = 1;
                  },
                ),
              ),
              Flexible(
                child: BlocConsumer<MovieSearchBloc, MovieSearchState>(
                  listener: (context, state) {
                    state.when(
                      result: (movies, isLastPage) {
                        _pagingController.itemList = movies;
                        if (isLastPage) {
                          _pagingController.nextPageKey = null;
                        } else if (movies.length > 20) {
                          _pagingController.nextPageKey = movies.length ~/ 20;
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
                      pagingController: _pagingController,
                      scrollController: _scrollController,
                    ),
                  ),
                ),
              ),
            ],
          ),
        ),
      );
}
