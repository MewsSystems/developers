import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:move_app/data/repositories/movie.dart';
import 'package:move_app/data/repositories/movie_detail.dart';
import 'package:move_app/presentation/common_widgets/common_wiedgets.dart';
import '../../logic/buisness_logic.dart';


class MovieDetailPopulated extends StatelessWidget {
  const MovieDetailPopulated({ Key? key, required this.movieDetail }) : super(key: key);

  final MovieDetail movieDetail;
  
  @override
  Widget build(BuildContext context) {
    return Card(
        child: SingleChildScrollView(
          child: Column(
            children: [
              movieDetail.image,
              ListTile(
                title: Text(movieDetail.title),
                subtitle: Text(movieDetail.genres)
              ),
              Container(
                margin: const EdgeInsets.only(left: 15.0, right: 15.0, bottom: 20),
                child: Text(movieDetail.overview),
              )
              
            ],
          ),
        ),
    );
  }
}
