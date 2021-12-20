import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'logic/buisness_logic.dart';
import 'package:move_app/data/repositories/movie_repository.dart';
import 'presentation/search_screen/search_movie_page.dart';


void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Movie App',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: BlocProvider(
        create: (context) => PreviewCubit(MovieRepository()),
        child: const SearchMoviePage(),
      ),
    ); 
  }
}