import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:intl/intl.dart';
import 'package:mews_imdb/movie_details/cubit/credits/credits_cubit.dart';
import 'package:mews_imdb/movie_details/models/models.dart';
import 'package:mews_imdb/utils/utils.dart';

import '../cubit/movie/movie_cubit.dart';

class MovieDetailsView extends StatelessWidget {
  const MovieDetailsView({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) => Scaffold(
        body: SafeArea(
          child: BlocBuilder<MovieCubit, MovieState>(
            builder: (context, state) {
              switch (state.status) {
                case MovieStatus.initial:
                case MovieStatus.loading:
                  return const _LoadingView(message: 'Loading...');
                case MovieStatus.success:
                  return MovieDetailsWidget(movieDetails: state.movie);
                case MovieStatus.failure:
                default:
                  return const _LoadingView(message: 'Error...');
              }
            },
          ),
        ),
      );
}

class MovieDetailsWidget extends StatelessWidget {
  const MovieDetailsWidget({Key? key, required this.movieDetails})
      : super(key: key);

  final MovieDetails movieDetails;

  @override
  Widget build(BuildContext context) => SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            _PosterWidget(
              posterUrl: movieDetails.posterPath,
              backdropUrl: movieDetails.backdrop,
            ),
            _TitleWidget(title: movieDetails.title),
            _GenresWidget(genres: movieDetails.genres),
            _DetailsWidget(
              score: movieDetails.voteAverage,
              releaseDate: movieDetails.releaseDate,
              runtime: movieDetails.runtime,
            ),
            _OverviewWidget(overview: movieDetails.overview ?? ''),
            const Divider(color: Colors.black),
            BlocBuilder<CreditsCubit, CreditsState>(
              builder: (context, state) {
                switch (state.status) {
                  case CreditsStatus.loading:
                    return const Center(
                      child: Text('Loading...'),
                    );
                  case CreditsStatus.success:
                    return state.membersList.isNotEmpty
                        ? _CastListWidget(
                            members: state.membersList,
                          )
                        : const SizedBox.shrink();
                  case CreditsStatus.failure:
                    return const _LoadingView(
                      message: 'Error while loading...',
                    );
                  case CreditsStatus.init:
                  default:
                    return const SizedBox(
                      height: 220,
                      child: _LoadingView(
                        message: 'Loading...',
                      ),
                    );
                }
              },
            ),
          ],
        ),
      );
}

class _LoadingView extends StatelessWidget {
  const _LoadingView({
    Key? key,
    required this.message,
  }) : super(key: key);

  final String message;
  @override
  Widget build(BuildContext context) => Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            CircularProgressIndicator(color: Theme.of(context).primaryColor),
            const SizedBox(
              height: 20,
            ),
            Text(message),
          ],
        ),
      );
}

class _GenresWidget extends StatelessWidget {
  const _GenresWidget({
    Key? key,
    required this.genres,
  }) : super(key: key);

  final List<String> genres;

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.symmetric(horizontal: 5),
        child: Wrap(
          direction: Axis.horizontal,
          spacing: 10.0,
          children: genres.map((e) => Chip(label: Text(e))).toList(),
        ),
      );
}

class _PosterWidget extends StatelessWidget {
  const _PosterWidget({
    Key? key,
    required this.posterUrl,
    required this.backdropUrl,
  }) : super(key: key);

  final String? posterUrl;
  final String? backdropUrl;

  @override
  Widget build(BuildContext context) => SizedBox(
        child: AspectRatio(
          aspectRatio: 1.78,
          child: Stack(
            children: [
              SizedBox(
                width: double.infinity,
                child: Utils.getImage(backdropUrl),
              ),
              Positioned(
                left: 20,
                bottom: 5,
                child: SizedBox(
                  width: 120,
                  height: 200,
                  child: Utils.getImage(posterUrl),
                ),
              )
            ],
          ),
        ),
      );
}

class _TitleWidget extends StatelessWidget {
  const _TitleWidget({
    Key? key,
    required this.title,
  }) : super(key: key);

  final String title;

  static const TextStyle titleTextStyle = TextStyle(
    fontSize: 24,
    color: Color.fromARGB(255, 27, 27, 27),
    fontWeight: FontWeight.w600,
  );

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.all(8.0),
        child: Text(
          title,
          style: titleTextStyle,
          textAlign: TextAlign.left,
        ),
      );
}

class _DetailsWidget extends StatelessWidget {
  const _DetailsWidget({
    Key? key,
    required this.score,
    required this.releaseDate,
    required this.runtime,
  }) : super(key: key);

  static const TextStyle detailsTextStyle =
      TextStyle(fontSize: 16, color: Color.fromARGB(255, 46, 46, 46));

  final double score;
  final DateTime? releaseDate;
  final int? runtime;

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.all(8.0),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            Text('$score/10', style: detailsTextStyle),
            Text(
              _formatReleaseDate(releaseDate),
              style: detailsTextStyle,
            ),
            Text(
              _formatRuntime(runtime),
              style: detailsTextStyle,
            ),
          ],
        ),
      );

  String _formatReleaseDate(DateTime? date) {
    if (date == null) return '';

    return DateFormat.yMMM().format(date);
  }

  String _formatRuntime(int? duration) {
    if (duration == null) return '';
    final int hours = duration ~/ 60;
    final int minutes = duration % 60;

    return '${hours.toString().padLeft(2, "0")}:${minutes.toString().padLeft(2, "0")}';
  }
}

class _OverviewWidget extends StatelessWidget {
  const _OverviewWidget({Key? key, required this.overview}) : super(key: key);

  static const TextStyle overviewTextStyle =
      TextStyle(fontSize: 20, color: Color.fromARGB(255, 48, 48, 48));

  final String overview;

  @override
  Widget build(BuildContext context) => Padding(
        padding: const EdgeInsets.all(8.0),
        child: Text(
          overview,
          style: overviewTextStyle,
        ),
      );
}

class _CastListWidget extends StatelessWidget {
  const _CastListWidget({Key? key, required this.members}) : super(key: key);

  static const TextStyle nameStyle =
      TextStyle(color: Color.fromARGB(255, 116, 115, 115), fontSize: 16);

  final List<Member> members;

  @override
  Widget build(BuildContext context) => SizedBox(
        height: 240,
        child: ListView.builder(
          scrollDirection: Axis.horizontal,
          itemCount: members.length > 10 ? 10 : members.length,
          padding: const EdgeInsets.symmetric(horizontal: 5),
          itemBuilder: (context, index) => SizedBox(
            width: 160,
            height: 220,
            child: Card(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                children: [
                  ClipRRect(
                    borderRadius: const BorderRadius.all(Radius.circular(10.0)),
                    child: SizedBox(
                      width: 130,
                      height: 200,
                      child: Utils.getImage(members[index].posterPath),
                    ),
                  ),
                  Text(
                    members[index].name,
                    style: nameStyle,
                    maxLines: 1,
                    overflow: TextOverflow.ellipsis,
                  )
                ],
              ),
            ),
          ),
        ),
      );
}
