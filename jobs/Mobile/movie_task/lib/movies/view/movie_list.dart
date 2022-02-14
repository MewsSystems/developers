import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_task/movies/bloc/search_bloc/search_bloc.dart';
import 'package:movie_task/movies/movies.dart';

class MoviesList extends StatefulWidget {
  const MoviesList({Key? key}) : super(key: key);

  @override
  _MoviesListState createState() => _MoviesListState();
}

class _MoviesListState extends State<MoviesList> {
  final _scrollController = ScrollController();

  @override
  void initState() {
    super.initState();
    _scrollController.addListener(_onScroll);
  }

  @override
  Widget build(BuildContext context) => BlocBuilder<SearchBloc, SearchState>(
        builder: (context, state) {
          switch (state.status) {
            case SearchStatus.initial:
              return const Center(child: Text('Start typing to search'));
            case SearchStatus.failure:
              return const Center(child: Text('failed to fetch movies'));
            case SearchStatus.success:
              if (state.movies.isEmpty) {
                return const Center(child: Text('no movies found'));
              }
              return ListView.builder(
                itemBuilder: (BuildContext context, int index) =>
                    index >= state.movies.length
                        ? const Loading()
                        : MovieListItem(movie: state.movies[index]),
                itemCount: state.hasReachedMax
                    ? state.movies.length
                    : state.movies.length + 1,
                controller: _scrollController,
              );
            default:
              return const Center(
                child: CircularProgressIndicator(),
              );
          }
        },
      );

  @override
  void dispose() {
    _scrollController
      ..removeListener(_onScroll)
      ..dispose();
    super.dispose();
  }

  void _onScroll() {
    if (_isBottom) context.read<SearchBloc>().add(SearchFetched());
  }

  bool get _isBottom {
    if (!_scrollController.hasClients) return false;
    final maxScroll = _scrollController.position.maxScrollExtent;
    final currentScroll = _scrollController.offset;

    return currentScroll >= (maxScroll * 0.9);
  }
}
