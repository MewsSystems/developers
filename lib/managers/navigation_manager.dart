import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import '../scenes/home/bloc/home_bloc.dart';
import '../scenes/home/pages/home_page.dart';

class NavigationManager {}

extension NavigationManagerPages on NavigationManager {
  Widget get homePage => BlocProvider(
        create: (context) => HomeBloc(),
        child: const HomePage(),
      );
}

extension NavigationManagerNav on NavigationManager {}
