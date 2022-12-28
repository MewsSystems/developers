// ignore_for_file: library_private_types_in_public_api, prefer_function_declarations_over_variables

import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_search/scenes/home/widgets/loading_more_button.dart';

import '../../../common/models/failure.dart';
import '../../../common/widgets/alert_box.dart';
import '../../../common/widgets/spinner.dart';
import '../bloc/home_bloc.dart';
import '../viewmodel/home_viewmodel.dart';
import '../widgets/movie_list_item.dart';
import '../widgets/search_field.dart';

class HomePage extends StatefulWidget {
  const HomePage({Key? key}) : super(key: key);

  @override
  State<HomePage> createState() => _HomePageState();
}

class _HomePageState extends State<HomePage> {
  final ScrollController _scrollController = ScrollController();
  final TextEditingController _textController = TextEditingController();
  late final _scrollControllerListener = () {
    final hasMoreRecords = context.read<HomeBloc>().hasMoreRecords;
    final isFetchingRecords = context.read<HomeBloc>().isFetchingRecords;
    final didPassAutoFetchZone = _scrollController.offset >
        (_scrollController.position.maxScrollExtent * 0.95);

    if (didPassAutoFetchZone && hasMoreRecords && !isFetchingRecords) {
      context.read<HomeBloc>().add(const HomeEvent.requestMoreRecords());
    }
  };

  @override
  void initState() {
    super.initState();
    _scrollController.addListener(_scrollControllerListener);
  }

  @override
  void dispose() {
    _scrollController.removeListener(_scrollControllerListener);
    _scrollController.dispose();
    _textController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Movies"),
        bottom: PreferredSize(
          preferredSize: const Size(double.infinity, 40),
          child: SearchField(
            controller: _textController,
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

extension StateWidgets on _HomePageState {
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
              .add(HomeEvent.retrySearch(_textController.text)),
          child: const Text("Try again!"),
        ),
      ],
    );
  }

  Widget _getLoadSuccessState(
      {required BuildContext context, required HomeViewModel viewModel}) {
    final hasMoreRecords = context.read<HomeBloc>().hasMoreRecords;

    return ListView.builder(
        controller: _scrollController,
        itemCount: viewModel.movies.length + (hasMoreRecords ? 1 : 0),
        itemBuilder: (context, index) {
          if (hasMoreRecords && (index == viewModel.movies.length)) {
            if (viewModel.didFailToLoadMoreRecords ?? false) {
              return LoadMoreButton(
                title: "Show more records",
                onPressed: () => context
                    .read<HomeBloc>()
                    .add(const HomeEvent.requestMoreRecords()),
              );
            }

            return SizedBox(
              height: 80,
              child: Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  crossAxisAlignment: CrossAxisAlignment.center,
                  children: const [
                    CircularProgressIndicator(
                      color: Colors.grey,
                    )
                  ]),
            );
          }

          final movie = viewModel.movies[index];

          return MovieListItem(movie: movie);
        });
  }
}
