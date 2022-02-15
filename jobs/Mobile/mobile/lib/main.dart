import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:injectable/injectable.dart';
import 'package:mobile/application/core/bloc_observer.dart';
import 'package:mobile/application/search/search_bloc.dart';
import 'package:mobile/injection.dart';
import 'package:mobile/presentation/cinephile/search/search_view.dart';
import 'package:mobile/presentation/core/theming/size_config.dart';
import 'package:mobile/presentation/core/theming/theme.dart';

void main() async {
  await _appInitializer();
}

/// Initializes dependencies.
Future<void> _appInitializer() async {
  WidgetsFlutterBinding.ensureInitialized();
  configureInjection(Environment.prod);
  BlocOverrides.runZoned(() => runApp(const Cinephile()),
      blocObserver: SimpleBlocObserver());
}

class Cinephile extends StatelessWidget {
  const Cinephile({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      theme: AppTheme.darkTheme,
      home: BlocProvider<SearchBloc>(
        create: (context) => getIt<SearchBloc>(),
        child: const Responsive(child: SearchView()),
      ),
    );
  }
}
