import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_search/bloc/details/details_cubit.dart';

class DetailView extends StatelessWidget {
  const DetailView({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text("Detail"),
        ),
        body: BlocBuilder<DetailsCubit, DetailsState>(
          builder: (BuildContext context, DetailsState state) {
            return state.map(
                initial: _buildLoadingView, loading: _buildLoadingView, loadedData: _buildBody, error: _buildErrorView);
          },
        ));
  }

  Widget _buildLoadingView(DetailsState state) {
    return const Center(child: CircularProgressIndicator());
  }

  Widget _buildErrorView(Error state) {
    return Center(child: Text(state.errorMessage));
  }

  Widget _buildBody(LoadedData state) {
    return Padding(
        padding: const EdgeInsets.symmetric(vertical: 0.0, horizontal: 10.0),
        child: Center(
            child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Text('Title: ${state.details.originalTitle}'),
            const SizedBox(
              height: 10.0,
            ),
            Text('Description: ${state.details.description}'),
            const SizedBox(
              height: 10.0,
            ),
            Text('Release date: ${state.details.releaseDate}'),
            //TODO more fields can be added if needed
          ],
        )));
  }
}
