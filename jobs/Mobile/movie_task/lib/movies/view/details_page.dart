import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_task/movies/bloc/movie_bloc/movie_bloc.dart';
import 'package:movie_task/movies/widgets/details_card.dart';

class DetailsPage extends StatelessWidget {
  const DetailsPage({
    Key? key,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) => BlocBuilder<MovieBloc, MovieState>(
        builder: (context, state) {
          switch (state.status) {
            case MovieStatus.initial:
              return const Center(
                child: CircularProgressIndicator(),
              );
            case MovieStatus.failure:
              return const Center(child: Text('failed to fetch movie details'));
            case MovieStatus.success:
              return Scaffold(
                appBar: AppBar(
                  title: Text(state.movie.title),
                ),
                body: Builder(
                  builder: (context) => Center(
                    child: Padding(
                      padding: const EdgeInsets.all(8),
                      child: ListView(
                        children: [
                          if (state.movie.backdropImage.isNotEmpty)
                            Image.network(
                              'https://image.tmdb.org/t/p/w500${state.movie.backdropImage}',
                              width: double.infinity,
                              fit: BoxFit.fitWidth,
                            ),
                          DetailsCard(
                            title: 'Overview',
                            content: state.movie.body,
                          ),
                          DetailsCard(
                            title: 'Tagline',
                            content: state.movie.tagline,
                          ),
                          DetailsCard(
                            title: 'Rating (out of 10)',
                            content: state.movie.vote.toString(),
                          ),
                          if (state.movie.budget > 0)
                            DetailsCard(
                              title: 'Budget',
                              content: '\$${state.movie.budget.toString()}',
                            ),
                        ],
                      ),
                    ),
                  ),
                ),
              );
            default:
              return const Center(
                child: CircularProgressIndicator(),
              );
          }
        },
      );
}
