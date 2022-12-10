import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:intl/intl.dart';
import 'package:movies/blocs/movie_bloc/movie_bloc.dart';
import 'package:movies/config/consts.dart';
import 'package:movies/config/custom_theme.dart';
import 'package:movies/models/detailed_movie_model.dart';
import 'package:movies/networking/repository/movie_repository.dart';
import 'package:movies/pages/_widgets/loader.dart';
import 'package:movies/pages/_widgets/network_image_loader.dart';
import 'package:movies/pages/_widgets/poster.dart';
import 'package:movies/pages/_widgets/rating.dart';
import 'package:movies/pages/_widgets/votes.dart';
import 'package:movies/pages/movie_page.dart/_widgets/detail_info_cell.dart';
import 'package:movies/pages/search_page/_widgets/no_image.dart';

class MoviePage extends StatelessWidget {
  const MoviePage({super.key, required this.movieId});

  final int movieId;

  @override
  Widget build(BuildContext context) => Scaffold(
        body: BlocProvider(
          create: (context) => MovieBloc(
            movieRepository: context.read<MovieRepository>(),
          )..add(GetMovieEvent(movieId)),
          child: const _MoviePageView(),
        ),
      );
}

class _MoviePageView extends StatelessWidget {
  const _MoviePageView();

  @override
  Widget build(BuildContext context) => BlocBuilder<MovieBloc, MovieState>(
        builder: (context, state) {
          if (state is LoadingMovieState) {
            return const Loader();
          } else if (state is SuccessMovieState) {
            return CustomScrollView(
              slivers: <Widget>[
                _SliverAppBar(state.movie),
                SliverPadding(
                  padding: const EdgeInsets.fromLTRB(16.0, 0.0, 16.0, 32.0),
                  sliver: _SliverContent(state.movie),
                ),
              ],
            );
          } else if (state is ErrorMovieState) {
            return _ErrorMessage(state.message);
          } else {
            return Container();
          }
        },
      );
}

class _SliverAppBar extends StatelessWidget {
  const _SliverAppBar(this.movie);

  final DetailedMovie movie;

  @override
  Widget build(BuildContext context) => SliverAppBar(
        leading: IconButton(
          icon: const Icon(Icons.arrow_back_ios_rounded),
          onPressed: () => Navigator.of(context).pop(),
        ),
        expandedHeight: 200.0,
        floating: false,
        snap: false,
        flexibleSpace: FlexibleSpaceBar(
          titlePadding: EdgeInsets.zero,
          centerTitle: true,
          background: movie.backdropPath != null || movie.posterPath != null
              ? NetworkImageLoader(
                  path:
                      '${Images.w780}${movie.backdropPath ?? movie.posterPath}',
                )
              : Container(
                  color: CustomTheme.blue,
                  child: const Center(child: Text('no image')),
                ),
        ),
      );
}

class _SliverContent extends StatelessWidget {
  const _SliverContent(this.movie);

  final DetailedMovie movie;

  @override
  Widget build(BuildContext context) => SliverList(
        delegate: SliverChildListDelegate([
          const SizedBox(height: 16.0),
          Text(
            movie.originalTitle,
            style: const TextStyle(fontSize: 20.0),
          ),
          const SizedBox(height: 8.0),
          if (movie.tagline != null && movie.tagline!.isNotEmpty)
            Padding(
              padding: const EdgeInsets.only(bottom: 16.0),
              child: Text(
                '${movie.tagline}',
                style: const TextStyle(fontSize: 14.0),
              ),
            ),
          Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              if (movie.posterPath != null)
                Poster(
                  height: 225.0,
                  width: 150.0,
                  path: '${Images.w300}${movie.posterPath}',
                )
              else
                const NoImage(
                  height: 225.0,
                  width: 150.0,
                ),
              const SizedBox(width: 16.0),
              Column(
                mainAxisAlignment: MainAxisAlignment.start,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    children: [
                      Rating(rating: movie.voteAverage),
                      const SizedBox(width: 8.0),
                      Text(
                        '${movie.voteAverage.toStringAsFixed(1)}/10',
                      ),
                    ],
                  ),
                  const SizedBox(height: 4.0),
                  Votes(count: movie.voteCount),
                  const SizedBox(height: 16.0),
                  DetailInfoCell(
                    title: 'Relese',
                    text: movie.releaseDate != null &&
                            movie.releaseDate!.isNotEmpty
                        ? '${movie.releaseDate}'
                        : 'No release date',
                  ),
                  DetailInfoCell(
                    title: 'Budget',
                    text: NumberFormat.simpleCurrency().format(movie.budget),
                  ),
                  DetailInfoCell(
                    title: 'Revenue',
                    text: NumberFormat.simpleCurrency().format(movie.revenue),
                  ),
                ],
              )
            ],
          ),
          const SizedBox(height: 16.0),
          if (movie.overview != null)
            Text(
              '${movie.overview}',
              style: const TextStyle(fontSize: 16.0),
            ),
        ]),
      );
}

class _ErrorMessage extends StatelessWidget {
  const _ErrorMessage(this.message);

  final String message;

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Center(child: Text(message)),
            const SizedBox(height: 16.0),
            SizedBox(
              width: double.infinity,
              child: OutlinedButton(
                onPressed: () => Navigator.of(context).pop(),
                child: const Text('Go back'),
              ),
            ),
          ],
        ),
      );
}
