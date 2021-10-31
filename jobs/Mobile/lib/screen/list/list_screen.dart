import 'package:flutter/widgets.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:movie_search/api/api_manager.dart';
import 'package:movie_search/bloc/list/list_cubit.dart';
import 'package:provider/provider.dart';

import 'list_screen_view.dart';

class ListScreen extends StatelessWidget {
  const ListScreen({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (_) => ListCubit(Provider.of<ApiManager>(context)),
      child: ListScreenView(),
    );
  }
}
