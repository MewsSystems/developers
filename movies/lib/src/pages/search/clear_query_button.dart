import 'package:flutter/material.dart';
import 'package:movies/src/pages/search/search_bar.dart';

/// Used to clear the search bar's query
class ClearQueryButton extends StatefulWidget {
  const ClearQueryButton({Key? key}) : super(key: key);

  @override
  State<ClearQueryButton> createState() => _ClearQueryButtonState();
}
class _ClearQueryButtonState extends State<ClearQueryButton> {
  bool isQueryEmpty = true;

  @override
  void initState() {
    super.initState();
    searchQueryController.addListener(() {
      setState(() {
        isQueryEmpty = searchQueryController.text.isEmpty;
      });
    });
  }

  @override
  Widget build(BuildContext context) => isQueryEmpty
      ? const Offstage()
      : IconButton(
          onPressed: searchQueryController.clear,
          icon: Icon(
            Icons.close,
            color: Theme.of(context).colorScheme.primary,
          ),
        );
}
