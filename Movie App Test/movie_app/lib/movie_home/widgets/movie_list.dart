import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_app/data/api_services.dart';
import 'package:movie_app/models/movie.dart';
import 'package:movie_app/movie_detail/view/movie_detail_page.dart';
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
          context.read<MoviesBloc>().state.isSearch
              ? context.read<MoviesBloc>().add(
                    NextPageSearch(),
                  )
              : context.read<MoviesBloc>().add(
                    NextPagePopularMovies(),
                  );
        }
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    return GridView.builder(
      key: const Key('movieGridScrollView'),
      itemCount: widget.moviesList.length + 1,
      shrinkWrap: true,
      physics: const BouncingScrollPhysics(),
      controller: scrollController,
      itemBuilder: (context, index) {
        return index >= widget.moviesList.length
            ? const BottomLoader()
            : GestureDetector(
                key: const Key('openMovieDetail'),
                onTap: () {
                  Navigator.push(
                    context,
                    MaterialPageRoute(
                      builder: (context) => MovieDetailPage(
                        movieId: widget.moviesList[index].id!,
                      ),
                    ),
                  );
                },
                child: Card(
                  clipBehavior: Clip.antiAlias,
                  child: Hero(
                    tag: "${widget.moviesList[index].id}",
                    child: widget.moviesList[index].posterPath != null
                        ? FadeInImage.memoryNetwork(
                            image: TheMovieDbService.imageUrl(
                                widget.moviesList[index].posterPath!,
                                PosterSize.w185),
                            placeholder: kTransparentImage,
                            fit: BoxFit.cover,
                          )
                        : const SizedBox.shrink(),
                  ),
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

class BottomLoader extends StatelessWidget {
  const BottomLoader({super.key});

  @override
  Widget build(BuildContext context) {
    return Container(
      alignment: Alignment.center,
      child: const Center(
        child: SizedBox(
          width: 33,
          height: 33,
          child: CircularProgressIndicator(
            strokeWidth: 1.5,
          ),
        ),
      ),
    );
  }
}
