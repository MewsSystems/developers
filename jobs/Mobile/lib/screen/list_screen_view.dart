import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:movie_search/model/movie_list_item/movie_list_item.dart';
import 'package:movie_search/screen/bloc/list_cubit.dart';

class ListScreenView extends StatelessWidget {
  final TextEditingController editingController = TextEditingController();

  ListScreenView({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text("List"),
        ),
        body: Column(children: <Widget>[
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: TextField(
              onChanged: (String value) {
                _searchMovies(context, value);
              },
              controller: editingController,
              decoration: const InputDecoration(
                  labelText: "Search",
                  hintText: "Search",
                  prefixIcon: Icon(Icons.search),
                  border: OutlineInputBorder(borderRadius: BorderRadius.all(Radius.circular(25.0)))),
            ),
          ),
          BlocConsumer<ListCubit, ListState>(listener: (BuildContext context, ListState state) {
            if (state is Error) {
              Fluttertoast.showToast(
                  msg: state.errorMessage,
                  toastLength: Toast.LENGTH_SHORT,
                  gravity: ToastGravity.CENTER,
                  timeInSecForIosWeb: 1,
                  backgroundColor: Colors.red,
                  textColor: Colors.white,
                  fontSize: 16.0);
            }
          }, builder: (BuildContext context, ListState state) {
            return state.map(
                initial: (Initial state) {
                  return _buildEmptyState(context);
                },
                loading: _buildInitialLoadingState,
                loadedData: (LoadedData state) {
                  if (state.items.isNotEmpty) {
                    return _buildLoadedState(context, state.items);
                  } else {
                    return _buildEmptyState(context);
                  }
                },
                error: (Error state) {
                  if (state.items.isNotEmpty) {
                    return _buildLoadedState(context, state.items);
                  } else {
                    return _buildEmptyState(context);
                  }
                });
          })
        ]));
  }

  Widget _buildEmptyState(BuildContext context) {
    return const Center(child: Text('There is no data. Type something in search field.'));
  }

  Widget _buildInitialLoadingState(ListState state) {
    return const Center(child: CircularProgressIndicator());
  }

  Widget _buildLoadedState(BuildContext context, List<MovieListItem> items) {
    return ListView.builder(
      shrinkWrap: true,
      itemCount: items.length,
      itemBuilder: (context, index) {
        return ListTile(
          title: Text(items[index].originalTitle),
        );
      },
    );
  }

  void _searchMovies(BuildContext context, String input) {
    BlocProvider.of<ListCubit>(context).refresh(input);
  }
}
