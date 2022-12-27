import 'package:flutter/material.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:movies/src/pages/search/clear_query_button.dart';

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
