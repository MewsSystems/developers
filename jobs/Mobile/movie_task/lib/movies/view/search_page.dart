import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_task/movies/bloc/search_bloc/search_bloc.dart';
import 'package:movie_task/movies/movies.dart';
import 'package:optimus/optimus.dart';

class SearchPage extends StatelessWidget {
  const SearchPage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) => Scaffold(
        appBar: AppBar(title: const Text('Movie Search')),
        body: Builder(
          builder: (context) => Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              OptimusInputField(
                placeholder: 'Search for a movie',
                onChanged: (userInput) {
                  context
                      .read<SearchBloc>()
                      .add(SearchFetched(query: userInput));
                },
              ),
              const Expanded(child: MoviesList()),
            ],
          ),
        ),
      );
}
