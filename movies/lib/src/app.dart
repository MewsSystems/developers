import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:movies/src/blocs/genres_cubit.dart';
import 'package:movies/src/blocs/movie_details_cubit.dart';
import 'package:movies/src/blocs/movie_search_bloc.dart';
import 'package:movies/src/blocs/selected_movie_bloc.dart';
import 'package:movies/src/pages/details/details_page.dart';
import 'package:movies/src/pages/search/search_page.dart';
import 'package:movies/theme.dart';

/// The Widget that configures your application.
class MyApp extends StatelessWidget {
  const MyApp({
    super.key,
  });

  @override
  Widget build(BuildContext context) => MultiBlocProvider(
        providers: [
          BlocProvider(create: (context) => SelectedMovieBloc()),
          BlocProvider(create: (context) => MovieSearchBloc()),
          BlocProvider(create: (context) => GenresCubit()),
          BlocProvider(
            lazy: false,
            create: (context) =>
                MovieDetailsCubit(BlocProvider.of<SelectedMovieBloc>(context)),
          )
        ],
        child: MaterialApp(
          // Providing a restorationScopeId allows the Navigator built by the
          // MaterialApp to restore the navigation stack when a user leaves and
          // returns to the app after it has been killed while running in the
          // background.
          restorationScopeId: 'app',

          // Provide the generated AppLocalizations to the MaterialApp. This
          // allows descendant Widgets to display the correct translations
          // depending on the user's locale.
          localizationsDelegates: const [
            AppLocalizations.delegate,
            GlobalMaterialLocalizations.delegate,
            GlobalWidgetsLocalizations.delegate,
            GlobalCupertinoLocalizations.delegate,
          ],
          supportedLocales: const [
            Locale('en', ''), // English, no country code
          ],

          // Use AppLocalizations to configure the correct application title
          // depending on the user's locale.
          //
          // The appTitle is defined in .arb files found in the localization
          // directory.
          onGenerateTitle: (BuildContext context) =>
              AppLocalizations.of(context).appTitle,
          themeMode: ThemeMode.dark,
          darkTheme: themeData.copyWith(
            textTheme: GoogleFonts.outfitTextTheme(themeData.textTheme),
          ),

          // Define a function to handle named routes in order to support
          // Flutter web url navigation and deep linking.
          onGenerateRoute: (RouteSettings routeSettings) =>
              MaterialPageRoute<void>(
            settings: routeSettings,
            builder: (BuildContext context) {
              switch (routeSettings.name) {
                case DetailsPage.routeName:
                  return const DetailsPage();
                case SearchPage.routeName:
                default:
                  return const SearchPage();
              }
            },
          ),
        ),
      );
}
