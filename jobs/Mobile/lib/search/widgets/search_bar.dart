import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:mews_imdb/search/bloc/search_bloc.dart';

class SearchBarWidget extends StatefulWidget {
  const SearchBarWidget({
    Key? key,
  }) : super(key: key);

  @override
  State<SearchBarWidget> createState() => _SearchBarWidgetState();
}

class _SearchBarWidgetState extends State<SearchBarWidget> {
  final TextEditingController _controller = TextEditingController();

  @override
  Widget build(BuildContext context) => SizedBox(
        height: 80,
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 10),
          child: TextField(
            controller: _controller,
            maxLines: 1,
            decoration: InputDecoration(
              label: const Text('Search'),
              suffixIcon: IconButton(
                onPressed: () {
                  _controller.clear();
                  updateResult(context, _controller.text);
                },
                icon: const Icon(Icons.clear),
              ),
            ),
            onChanged: (value) {
              updateResult(context, value);
            },
          ),
        ),
      );

  void updateResult(BuildContext context, String value) {
    if (value.isEmpty) {
      context.read<SearchBloc>().add(ResetSearchEvent(query: value));
    } else {
      context.read<SearchBloc>().add(NewSearchEvent(query: value));
    }
  }
}
