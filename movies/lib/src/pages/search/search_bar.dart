import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:infinite_scroll_pagination/infinite_scroll_pagination.dart';
import 'package:movies/src/blocs/movie_search_bloc.dart';
import 'package:movies/src/model/movie/movie.dart';
import 'package:movies/src/pages/search/clear_query_button.dart';
import 'package:movies/src/pages/search/result_list.dart';

final searchQueryController = TextEditingController();

/// Used to type the query for the movie search
class SearchBar extends StatefulWidget {
  const SearchBar({
    Key? key,
  }) : super(key: key);

  @override
  State<SearchBar> createState() => _SearchBarState();
}

class _SearchBarState extends State<SearchBar> {
  @override
  void initState() {
    super.initState();
    
  }
  
  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.all(16),
        child: TextField(
          controller: searchQueryController,
          autofocus: true,
          decoration: InputDecoration(
            prefixIcon: const Icon(Icons.search),
            suffixIcon: const ClearQueryButton(),
            hintText: AppLocalizations.of(context).searchMovie,
          ),
        ),
      );
}
