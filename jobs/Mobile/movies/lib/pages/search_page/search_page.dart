import 'package:debouncer_widget/debouncer_widget.dart';
import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movies/blocs/search_bloc/search_bloc.dart';
import 'package:movies/config/consts.dart';
import 'package:movies/config/custom_theme.dart';
import 'package:movies/data/repository/movie_repository.dart';
import 'package:movies/models/movie_model.dart';
import 'package:movies/pages/_widgets/loader.dart';
import 'package:movies/pages/search_page/_widgets/movie_card.dart';
import 'package:movies/pages/search_page/_widgets/no_movies.dart';
import 'package:movies/pages/search_page/_widgets/pagination/end_of_list.dart';
import 'package:movies/pages/search_page/_widgets/pagination/pagination_loader.dart';

class SearchPage extends StatefulWidget {
  const SearchPage({super.key});

  @override
  State<SearchPage> createState() => _SearchPageState();
}

class _SearchPageState extends State<SearchPage> {
  late SearchBloc _searchBloc;

  final _textEditingController = TextEditingController();
  final _scrollController = ScrollController();

  @override
  void initState() {
    super.initState();
    _searchBloc = SearchBloc(movieRepository: context.read<MovieRepository>());
  }

  @override
  void dispose() {
    _textEditingController.dispose();
    _scrollController.dispose();
    super.dispose();
  }

  List<Movie> _movies = [];

  @override
  Widget build(BuildContext context) => Scaffold(
        body: SafeArea(
          bottom: false,
          child: Stack(
            children: [
              Padding(
                padding: const EdgeInsets.only(top: 8.0),
                child: BlocConsumer<SearchBloc, SearchState>(
                  bloc: _searchBloc,
                  listener: (context, state) {
                    if (state is SuccessSearchState) {
                      (state.page == 1)
                          ? _movies = state.movies
                          : _movies.addAll(state.movies);
                    }
                  },
                  builder: (_, state) {
                    if (state is LoadingSearchState) {
                      return const Loader();
                    } else if (state is SuccessSearchState) {
                      if (_movies.isEmpty) {
                        return const NoMovies();
                      }

                      return RefreshIndicator(
                        color: CustomTheme.blue,
                        child: ListView.builder(
                          controller: _scrollController,
                          padding: const EdgeInsets.only(top: 60.0),
                          itemCount: _movies.length + 1,
                          itemBuilder: (_, index) {
                            if (index == _movies.length - 1) {
                              if (state.hasMorePages) {
                                _searchBloc.add(NextSearchEvent());
                              }
                            }

                            if (index == _movies.length) {
                              return state.hasMorePages
                                  ? const PaginationLoader()
                                  : const EndOfList();
                            } else {
                              return MovieCard(
                                index: index,
                                movie: _movies[index],
                                totalPages: state.total,
                              );
                            }
                          },
                        ),
                        onRefresh: () async => _searchBloc.add(
                          FirstSearchEvent(_textEditingController.text),
                        ),
                      );
                    } else if (state is ErrorSearchState) {
                      return Center(child: Text(state.message));
                    }

                    return const Center(
                      child: Text('Use serch to find a movie.'),
                    );
                  },
                ),
              ),
              Padding(
                padding: const EdgeInsets.symmetric(horizontal: 16.0),
                child: Debouncer(
                  timeout: Pagination.timeout,
                  action: () => _searchBloc.add(
                    FirstSearchEvent(_textEditingController.text),
                  ),
                  builder: (newContext, _) => Column(
                    children: [
                      TextField(
                        controller: _textEditingController,
                        decoration: const InputDecoration(
                          prefixIcon: Icon(
                            Icons.search_rounded,
                            color: CustomTheme.grey,
                          ),
                          label: Text('Search'),
                          floatingLabelBehavior: FloatingLabelBehavior.never,
                        ),
                        onChanged: (_) => Debouncer.execute(newContext),
                      ),
                    ],
                  ),
                ),
              ),
            ],
          ),
        ),
        floatingActionButton: BlocBuilder<SearchBloc, SearchState>(
          bloc: _searchBloc,
          builder: (context, state) => FloatingActionButton(
            backgroundColor: CustomTheme.grey.withAlpha(75),
            child: const Icon(Icons.arrow_upward_rounded),
            onPressed: () => (_searchBloc.state is SuccessSearchState)
                ? _scrollController.animateTo(
                    0,
                    duration: const Duration(milliseconds: 500),
                    curve: Curves.fastOutSlowIn,
                  )
                : null,
          ),
        ),
      );
}
