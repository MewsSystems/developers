import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../scenes/home/bloc/home_bloc.dart';
import '../scenes/home/pages/home_page.dart';
import '../scenes/home/repository/home_repository.dart';
import 'get_it.dart';

class NavigationManager {}

extension NavigationManagerPages on NavigationManager {
  Widget get homePage => BlocProvider(
        create: (context) => HomeBloc(repository: getIt.get<HomeRepository>()),
        child: const HomePage(),
      );
}

extension NavigationManagerNav on NavigationManager {}
