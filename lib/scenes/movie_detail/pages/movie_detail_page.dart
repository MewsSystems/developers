import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../../../common/widgets/smart_image.dart';
import '../bloc/movie_detail_bloc.dart';
import '../viewmodel/movie_detail_viewmodel.dart';

class MovieDetailPage extends StatelessWidget {
  const MovieDetailPage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<MovieDetailBloc, MovieDetailState>(
      builder: (context, state) {
        return state.when(
          initial: () => _getInitialState(context),
          loadSuccess: (viewModel) =>
              _getLoadSuccessState(context: context, viewModel: viewModel),
        );
      },
    );
  }
}

extension MovieDetailPageStateWidgets on MovieDetailPage {
  Widget _getInitialState(BuildContext context) {
    context.read<MovieDetailBloc>().add(const MovieDetailEvent.started());
    return Container();
  }

  Widget _getLoadSuccessState({
    required BuildContext context,
    required MovieDetailViewModel viewModel,
  }) {
    return Scaffold(
      appBar: AppBar(title: Text(viewModel.movie.title)),
      body: Column(
        children: [
          Expanded(
            child: Hero(
              tag: ValueKey(viewModel.movie.id),
              child: SmartImage(
                imageUrl: viewModel.movie.imageUrl,
              ),
            ),
          ),
          SafeArea(
            child: Padding(
              padding: const EdgeInsets.all(8.0),
              child: Column(
                children: [
                  Text(viewModel.movie.overview,
                      style: Theme.of(context)
                          .textTheme
                          .bodyText1
                          ?.copyWith(color: Colors.white))
                ],
              ),
            ),
          )
        ],
      ),
    );
  }
}
