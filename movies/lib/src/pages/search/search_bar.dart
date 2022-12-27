import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:infinite_scroll_pagination/infinite_scroll_pagination.dart';
import 'package:movies/src/blocs/movie_search_bloc.dart';
import 'package:movies/src/model/movie/movie.dart';

final searchQueryController = TextEditingController();

class SearchBar extends StatelessWidget {
  const SearchBar({
    Key? key,
    required ScrollController scrollController,
    required PagingController<int, Movie> pagingController,
  })  : _scrollController = scrollController,
        _pagingController = pagingController,
        super(key: key);

  final ScrollController _scrollController;
  final PagingController<int, Movie> _pagingController;

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.all(16),
        child: TextField(
          controller: searchQueryController,
          autofocus: true,
          decoration: InputDecoration(
            prefixIcon: const Icon(Icons.search),
            hintText: AppLocalizations.of(context).searchMovie,
          ),
          onChanged: (query) {
            // Emit the event that the query changed
            context.read<MovieSearchBloc>().add(MovieQueryChanged(query));
            // Scroll the list back up
            if (_scrollController.hasClients) {
              //_scrollController.jumpTo(0);
            }
            // Reset the paging count
            _pagingController.nextPageKey = 1;
          },
        ),
      );
}
