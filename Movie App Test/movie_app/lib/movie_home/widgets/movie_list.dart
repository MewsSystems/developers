import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_app/data/api_services.dart';
import 'package:movie_app/models/movie.dart';
import 'package:movie_app/movie_home/movie_home.dart';
import 'package:transparent_image/transparent_image.dart';

class MovieList extends StatefulWidget {
  final List<Movie> moviesList;
  const MovieList({
    required this.moviesList,
    super.key,
  });

  @override
  State<MovieList> createState() => _MovieListState();
}

class _MovieListState extends State<MovieList> {
  ScrollController scrollController = ScrollController();

  @override
  void initState() {
    super.initState();
    scrollController.addListener(() {
      if (scrollController.position.maxScrollExtent ==
          scrollController.position.pixels) {
        if (context.read<MoviesBloc>().state.moviesLoadStatus !=
            MoviesLoadStatus.loading) {
          context.read<MoviesBloc>().add(
                NextPagePopularMovies(
                  page: context.read<MoviesBloc>().state.page! + 1,
                ),
              );
        }
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    return GridView.builder(
      itemCount: widget.moviesList.length,
      shrinkWrap: true,
      physics: const BouncingScrollPhysics(),
      controller: scrollController,
      itemBuilder: (context, index) {
        return Card(
          clipBehavior: Clip.antiAlias,
          child: FadeInImage.memoryNetwork(
            image: TheMovieDbService.imageUrl(
                widget.moviesList[index].posterPath!, PosterSize.w185),
            placeholder: kTransparentImage,
            fit: BoxFit.cover,
          ),
        );
      },
      gridDelegate: const SliverGridDelegateWithFixedCrossAxisCount(
        childAspectRatio: 0.7,
        crossAxisCount: 3,
        mainAxisSpacing: 5.0,
      ),
    );
  }
}
