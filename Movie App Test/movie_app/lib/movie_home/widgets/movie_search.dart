import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_app/l10n/l10n.dart';
import 'package:movie_app/movie_home/movie_home.dart';

class MovieSearch extends StatelessWidget {
  MovieSearch({super.key});

  final TextEditingController _textEditingController = TextEditingController();

  @override
  Widget build(BuildContext context) {
    final l10n = context.l10n;

    return Padding(
      padding: const EdgeInsets.all(8.0),
      child: BlocBuilder<MoviesBloc, MoviesState>(
        builder: (context, state) {
          return TextFormField(
            key: const Key('searchField'),
            controller: _textEditingController,
            decoration: InputDecoration(
              filled: true,
              labelText: l10n.searchButtonText,
              border: OutlineInputBorder(
                borderRadius: BorderRadius.circular(30),
              ),
              suffixIcon: state.isSearch
                  ? IconButton(
                      key: const Key('searchClear'),
                      icon: const Icon(Icons.close),
                      onPressed: () {
                        _textEditingController.clear();
                        FocusManager.instance.primaryFocus?.unfocus();
                        context.read<MoviesBloc>().add(CleanSearch());
                      },
                    )
                  : null,
            ),
            onEditingComplete: () {
              context
                  .read<MoviesBloc>()
                  .add(MoviesSearch(query: _textEditingController.text));
            },
          );
        },
      ),
    );
  }
}
