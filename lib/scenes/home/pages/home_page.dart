import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../../../common/models/failure.dart';
import '../../../common/widgets/alert_box.dart';
import '../../../common/widgets/spinner.dart';
import '../bloc/home_bloc.dart';
import '../viewmodel/home_viewmodel.dart';
import '../widgets/movie_list_item.dart';
import '../widgets/search_field.dart';

class HomePage extends StatelessWidget {
  final TextEditingController textController = TextEditingController();

  HomePage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Movies"),
        bottom: PreferredSize(
          preferredSize: const Size(double.infinity, 40),
          child: SearchField(
            controller: textController,
            onChange: (value) =>
                context.read<HomeBloc>().add(HomeEvent.didChangeSearch(value)),
          ),
        ),
      ),
      body: BlocConsumer<HomeBloc, HomeState>(
        listenWhen: (prev, curr) => curr.isListenerState,
        buildWhen: (prev, curr) => !curr.isListenerState,
        listener: (context, state) {
          state.maybeWhen(
            displayAlert: (title, message, shouldPopOut, isListenerState) =>
                AlertBox(title: title, message: message).show(context).then(
                    (value) =>
                        shouldPopOut ? Navigator.of(context).pop() : null),
            orElse: () => throw UnimplementedError(
                '$state was not implemented in the listener of $this'),
          );
        },
        builder: (context, state) {
          return state.maybeWhen(
            initial: (_) => _getInitialState(context),
            loading: (viewModel, _) =>
                _getLoadingState(context: context, viewModel: viewModel),
            loadFailure: (failure, _) =>
                _getLoadFailureState(context: context, failure: failure),
            loadSuccess: (viewModel, _) =>
                _getLoadSuccessState(context: context, viewModel: viewModel),
            orElse: () => throw UnimplementedError(
                '$state was not implemented in the builder of $this'),
          );
        },
      ),
    );
  }
}

extension StateWidgets on HomePage {
  Widget _getInitialState(BuildContext context) {
    return Container();
  }

  Widget _getLoadingState(
      {required BuildContext context, HomeViewModel? viewModel}) {
    return Spinner(
      isSpinning: true,
      child: viewModel == null
          ? Container()
          : _getLoadSuccessState(context: context, viewModel: viewModel),
    );
  }

  Widget _getLoadFailureState({
    required BuildContext context,
    required Failure failure,
  }) {
    return Column(
      mainAxisAlignment: MainAxisAlignment.center,
      crossAxisAlignment: CrossAxisAlignment.center,
      children: [
        Padding(
          padding: const EdgeInsets.symmetric(horizontal: 30, vertical: 16.0),
          child: Text(failure.message,
              textAlign: TextAlign.center,
              style: Theme.of(context)
                  .textTheme
                  .headline5
                  ?.copyWith(color: Colors.grey)),
        ),
        ElevatedButton(
          onPressed: () => context
              .read<HomeBloc>()
              .add(HomeEvent.retrySearch(textController.text)),
          child: const Text("Try again!"),
        ),
      ],
    );
  }

  Widget _getLoadSuccessState(
      {required BuildContext context, required HomeViewModel viewModel}) {
    return ListView.builder(
        itemCount: viewModel.movies.length,
        itemBuilder: (context, index) {
          final movie = viewModel.movies[index];

          return MovieListItem(movie: movie);
        });
  }
}
