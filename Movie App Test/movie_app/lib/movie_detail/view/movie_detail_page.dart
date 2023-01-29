import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_app/data/api_services.dart';
import 'package:movie_app/movie_detail/bloc/movie_info_cubit.dart';
import 'package:movie_app/movie_detail/bloc/movie_info_state.dart';
import 'package:movie_app/movie_home/bloc/movies_bloc.dart';
import 'package:movie_app/movie_home/bloc/movies_event.dart';
import 'package:movie_app/movie_home/bloc/movies_state.dart';
import 'package:movie_app/movie_home/widgets/movie_list.dart';
import 'package:movie_app/l10n/l10n.dart';
import 'package:movie_app/repositories/movies_repository.dart';
import 'package:transparent_image/transparent_image.dart';

class MovieDetailPage extends StatefulWidget {
  final int movieId;
  const MovieDetailPage({super.key, required this.movieId});
  @override
  State<MovieDetailPage> createState() => _MovieDetailPageState();
}

class _MovieDetailPageState extends State<MovieDetailPage> {
  @override
  Widget build(BuildContext context) {
    final l10n = context.l10n;

    return BlocProvider(
      create: (_) => MovieInfoCubit(
        theMovieDbRepository:
            RepositoryProvider.of<TheMovieDbRepository>(context),
      )..getMovieInfo(widget.movieId),
      child: BlocBuilder<MovieInfoCubit, MovieInfoState>(
        builder: (context, state) {
          switch (state.movieInfoLoadStatus) {
            case MovieInfoLoadStatus.loading:
              return const Center(
                child: CircularProgressIndicator(),
              );
            case MovieInfoLoadStatus.succeeded:
              return Scaffold(
                appBar: AppBar(
                  title: Text(
                    state.movieInfo!.title!,
                  ),
                ),
                body: SafeArea(
                  child: ListView(
                    shrinkWrap: true,
                    children: [
                      Hero(
                        tag: "${state.movieInfo!.id}",
                        child: FadeInImage.memoryNetwork(
                          image: TheMovieDbService.imageUrl(
                              state.movieInfo!.posterPath!, PosterSize.w780),
                          placeholder: kTransparentImage,
                          fit: BoxFit.cover,
                        ),
                      ),
                      Padding(
                        padding: const EdgeInsets.all(10),
                        child: Column(
                          crossAxisAlignment: CrossAxisAlignment.start,
                          children: <Widget>[
                            Text(
                              state.movieInfo!.title!,
                              style: const TextStyle(
                                fontSize: 25,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                            Container(
                                margin: const EdgeInsets.only(
                              top: 16,
                            )),
                            Row(
                              mainAxisAlignment: MainAxisAlignment.spaceBetween,
                              children: <Widget>[
                                Row(
                                  children: [
                                    const Icon(
                                      Icons.star,
                                      color: Colors.yellow,
                                    ),
                                    Container(
                                      margin: const EdgeInsets.only(
                                        left: 2,
                                      ),
                                    ),
                                    Text(
                                      state.movieInfo!.voteAverage.toString(),
                                      style: const TextStyle(
                                        fontSize: 18,
                                      ),
                                    ),
                                  ],
                                ),
                                Container(
                                  margin: const EdgeInsets.only(
                                    left: 20,
                                  ),
                                ),
                                Text(
                                  state.movieInfo!.releaseDate!,
                                  style: const TextStyle(
                                    fontSize: 18,
                                  ),
                                ),
                              ],
                            ),
                            Container(
                              margin: const EdgeInsets.only(
                                top: 16,
                              ),
                            ),
                            Text(
                              state.movieInfo!.overview!,
                            ),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
              );
            case MovieInfoLoadStatus.failed:
              return const ErrorView();
          }
        },
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
