import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:mews_imdb/search/bloc/search_bloc.dart';
import 'package:mews_imdb/search/views/search_list.dart';
import 'package:mews_imdb/search/widgets/search_bar.dart';
import 'package:movie_repository/movie_repository.dart';

class SearchPage extends StatefulWidget {
  const SearchPage({Key? key}) : super(key: key);

  @override
  State<SearchPage> createState() => _SearchPageState();
}

class _SearchPageState extends State<SearchPage> {
  @override
  Widget build(BuildContext context) => Scaffold(
        appBar: AppBar(
          title: const Center(child: Text('Movie Search')),
        ),
        body: BlocProvider(
          create: (context) =>
              SearchBloc(movieRepository: context.read<MovieRepository>()),
          child: Stack(
            children: const [
              SearchList(),
              SearchBarWidget(),
            ],
          ),
        ),
      );
}
