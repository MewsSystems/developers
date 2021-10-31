import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:lazy_load_scrollview/lazy_load_scrollview.dart';
import 'package:movie_search/bloc/list/list_cubit.dart';
import 'package:movie_search/model/movie_list_item/movie_list_item.dart';
import 'package:movie_search/screen/detail/detail_screen.dart';

class ListScreenView extends StatelessWidget {
  final TextEditingController editingController = TextEditingController();

  ListScreenView({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text("List"),
        ),
        body: Column(crossAxisAlignment: CrossAxisAlignment.center, children: <Widget>[
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: TextField(
              onChanged: (String input) {
                BlocProvider.of<ListCubit>(context).refresh(input);
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
            state.map(
                initial: (Initial state) {},
                loading: (Loading state) {},
                loadedData: (LoadedData state) {
                  debugPrint('Loaded page ${state.pageNumber}');
                },
                error: (Error state) {
                  Fluttertoast.showToast(
                      msg: state.errorMessage,
                      toastLength: Toast.LENGTH_SHORT,
                      gravity: ToastGravity.CENTER,
                      timeInSecForIosWeb: 1,
                      backgroundColor: Colors.red,
                      textColor: Colors.white,
                      fontSize: 16.0);
                },
                allDataLoaded: (AllDataLoaded state) {
                  Fluttertoast.showToast(
                      msg: 'All data loaded',
                      toastLength: Toast.LENGTH_SHORT,
                      gravity: ToastGravity.CENTER,
                      timeInSecForIosWeb: 1,
                      backgroundColor: Colors.green,
                      textColor: Colors.white,
                      fontSize: 16.0);
                });
          }, builder: (BuildContext context, ListState state) {
            return state.map(initial: (Initial state) {
              return _buildEmptyState(context);
            }, loading: (Loading state) {
              if (state.items.isNotEmpty) {
                return _buildItemsList(context: context, items: state.items, isLoading: true);
              } else {
                return _buildInitialLoadingState();
              }
            }, loadedData: (LoadedData state) {
              return _buildListOrEmpty(context: context, items: state.items);
            }, error: (Error state) {
              return _buildListOrEmpty(context: context, items: state.items);
            }, allDataLoaded: (AllDataLoaded value) {
              return _buildListOrEmpty(context: context, items: state.items);
            });
          })
        ]));
  }

  Widget _buildListOrEmpty(
      {required BuildContext context, required List<MovieListItem> items, bool isLoading = false}) {
    if (items.isNotEmpty) {
      return _buildItemsList(context: context, items: items, isLoading: isLoading);
    } else {
      return _buildEmptyState(context);
    }
  }

  Widget _buildEmptyState(BuildContext context) {
    return const Text('There is no data. Type something in search field.');
  }

  Widget _buildInitialLoadingState() {
    return const Center(child: CircularProgressIndicator());
  }

  Widget _buildItemsList({required BuildContext context, required List<MovieListItem> items, bool isLoading = false}) {
    return LazyLoadScrollView(
      onEndOfPage: () {
        BlocProvider.of<ListCubit>(context).loadNextPage(editingController.text);
      },
      child: Expanded(
          child: ListView.builder(
        shrinkWrap: true,
        itemCount: isLoading ? items.length + 1 : items.length,
        itemBuilder: (context, index) {
          if (index >= items.length) {
            return const Center(child: CircularProgressIndicator());
          } else {
            return ListTile(
              title: Text(items[index].originalTitle),
              onTap: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (context) => DetailScreen(movieId: items[index].id),
                  ),
                );
              },
            );
          }
        },
      )),
    );
  }
}
