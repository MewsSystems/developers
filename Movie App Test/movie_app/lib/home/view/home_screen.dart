import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_app/home/bloc/movies_bloc.dart';
import 'package:movie_app/home/bloc/movies_event.dart';
import 'package:movie_app/home/bloc/movies_state.dart';
import 'package:movie_app/home/widgets/movie_list.dart';
import 'package:movie_app/l10n/l10n.dart';
import 'package:movie_app/repositories/movies_repository.dart';

class HomePage extends StatefulWidget {
  const HomePage({super.key});
  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  TextEditingController _textEditingController = TextEditingController();
  @override
  Widget build(BuildContext context) {
    final l10n = context.l10n;

    return BlocProvider(
      create: (_) => MoviesBloc(
        theMovieDbRepository:
            RepositoryProvider.of<TheMovieDbRepository>(context),
      )..add(
          GetPopularMovies(),
        ),
      child: Scaffold(
        appBar: AppBar(
          title: Text(
            l10n.movieListAppBarTitle,
          ),
        ),
        body: SafeArea(
          child: Column(
            children: [
              BlocBuilder<MoviesBloc, MoviesState>(
                builder: (context, state) {
                  switch (state.moviesLoadStatus) {
                    case MoviesLoadStatus.failed:
                      return const ErrorView();
                    case MoviesLoadStatus.succeed:
                      return Expanded(
                        child: Column(
                          children: [
                            Padding(
                              padding: const EdgeInsets.symmetric(
                                  horizontal: 12.0, vertical: 24),
                              child: TextFormField(
                                controller: _textEditingController,
                                decoration: InputDecoration(
                                  labelText: l10n.searchButtonText,
                                ),
                                onEditingComplete: () {
                                  //TODO: Add search event an dlogic related to it
                                },
                              ),
                            ),
                            state.movieList.isNotEmpty
                                ? Flexible(
                                    child: MovieList(
                                      moviesList: state.movieList,
                                    ),
                                  )
                                : const Center(
                                    child: Text('No movies found'),
                                  ),
                          ],
                        ),
                      );
                    case MoviesLoadStatus.loading:
                      return const Center(child: CircularProgressIndicator());
                  }
                },
              ),
            ],
          ),
        ),
      ),
    );
  }
}

class ErrorView extends StatelessWidget {
  const ErrorView({super.key});

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        const Center(
          child: Text('Failed to fetch movies'),
        ),
        MaterialButton(
            child: const Icon(
              Icons.replay_circle_filled_rounded,
            ),
            onPressed: () {
              context.read<MoviesBloc>().add(
                    GetPopularMovies(),
                  );
            }),
      ],
    );
  }
}
