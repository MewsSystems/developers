import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:move_app/data/repositories/models.dart';
import 'package:move_app/data/repositories/movie_repository.dart';
import 'package:move_app/presentation/detail_screen/movie_detail_page.dart';
import '../../logic/buisness_logic.dart';

class MoviePopulated extends StatelessWidget {
  const MoviePopulated({
    Key? key,
    required this.movies,
  }) : super(key: key);

  final List<Movie> movies;

  @override
  Widget build(BuildContext context) {
    return ListView.builder(
      scrollDirection: Axis.vertical,
      shrinkWrap: true,
      itemCount: movies.length,
      itemBuilder: (context, index) => Card(
        child: ListTile(
          title: Text(movies[index].title),
          subtitle: Text(movies[index].releaseDate),
          leading:  movies[index].image,
          onTap: () {
            Navigator.of(context).push(
              MaterialPageRoute(
                builder: (_) => BlocProvider(
                  create: (_) => MovieDetailCubit(MovieRepository()),
                  child: MovieDeatail(movieID: movies[index].id,),
                ) 
              )
            );
          },
        ),
      ),
    );
  }
}