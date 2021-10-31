import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_search/api/api_manager.dart';
import 'package:movie_search/bloc/details/details_cubit.dart';
import 'package:movie_search/screen/detail/detail_view.dart';
import 'package:provider/provider.dart';

class DetailScreen extends StatelessWidget {
  final int movieId;

  const DetailScreen({Key? key, required this.movieId}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (_) => DetailsCubit(Provider.of<ApiManager>(context))..loadDetails(movieId),
      child: const DetailView(),
    );
  }
}
