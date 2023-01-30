import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:movie_app/data/api_services.dart';
import 'package:movie_app/repositories/movies_repository.dart';
import 'package:provider/provider.dart';

import 'l10n/l10n.dart';
import 'movie_home/movie_home.dart';
import 'theme/theme.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();

  await dotenv.load(fileName: '.env');

  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MultiProvider(
      providers: [
        Provider<TheMovieDbService>(
          create: (_) => TheMovieDbService(),
        ),
      ],
      child: _RepositoryInitializer(
        child: Provider(
          create: (_) => MovieAppThemeData.light(),
          child: Builder(
            builder: (context) {
              return MaterialApp(
                title: 'MovieDB App',
                localizationsDelegates: const [
                  AppLocalizations.delegate,
                  GlobalMaterialLocalizations.delegate,
                  GlobalWidgetsLocalizations.delegate,
                  GlobalCupertinoLocalizations.delegate,
                ],
                supportedLocales: const [
                  Locale('en', ''),
                  Locale('es', ''),
                ],
                theme: Provider.of<MovieAppThemeData>(context).materialThemeData,
                home: const HomePage(),
              );
            }
          ),
        ),
      ),
    );
  }
}

class _RepositoryInitializer extends StatelessWidget {
  final Widget child;
  const _RepositoryInitializer({
    required this.child,
  }) : super();

  @override
  Widget build(BuildContext context) {
    final theMovieDbService =
        Provider.of<TheMovieDbService>(context, listen: false);

    return MultiRepositoryProvider(
      providers: [
        RepositoryProvider(
          create: (_) => TheMovieDbRepository(
            theMovieDbService: theMovieDbService,
          ),
        ),
      ],
      child: child,
    );
  }
}
