import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:http/http.dart' as http;
import 'package:movie_task/movies/bloc/post_bloc.dart';
import 'package:movie_task/movies/movies.dart';
import 'package:optimus/optimus.dart';

class SearchPage extends StatelessWidget {
  const SearchPage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) => Scaffold(
        appBar: AppBar(title: const Text('Movie Search')),
        body: BlocProvider(
          create: (_) =>
              PostBloc(httpClient: http.Client())..add(PostFetched()),
          child: Builder(
              builder: (context) => Column(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      OptimusInputField(
                        placeholder: 'Search for a movie',
                        onChanged: (userInput) {
                          context
                              .read<PostBloc>()
                              .add(PostFetched(query: userInput));
                        },
                      ),
                      const Expanded(child: MoviesList()),
                    ],
                  )),
        ),
      );
}
