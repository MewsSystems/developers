import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:move_app/presentation/search_screen/search_movie_result.dart';
import '../../logic/buisness_logic.dart';


class SearchMoviePage extends StatefulWidget {
  const SearchMoviePage({Key? key}) : super(key: key);
  @override
  State<SearchMoviePage> createState() => SearchMoviePageState();
}

class SearchMoviePageState extends State<SearchMoviePage> {
  final TextEditingController _textController = TextEditingController();

  @override
  void dispose() {
    _textController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Movie Search')),
      body: Column (children: [
        Row(
          children: [
            Expanded(
              child: Padding(
                padding: const EdgeInsets.all(8.0),
                child: TextField(
                  controller: _textController,
                  decoration: const InputDecoration(
                    labelText: 'Movie name',
                    hintText: 'Search for a move',
                  ),
                  onChanged: (txt) =>  context.read<PreviewCubit>().fetchMovies(txt)
                ),
              ),
            ),
          ],
        ),
        const Expanded(child: SearchMovieResult())
      ],),
    );
  }
}