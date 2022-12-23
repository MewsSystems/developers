import 'package:cached_network_image/cached_network_image.dart';
import 'package:entry/entry.dart';
import 'package:flutter/material.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';
import 'package:movies/constants.dart';
import 'package:movies/src/pages/details_page.dart';

/// Displays a search bar and the list of results
class SearchPage extends StatelessWidget {
  const SearchPage({
    super.key,
  });

  static const routeName = '/';
  @override
  Widget build(BuildContext context) => Scaffold(
        body: SafeArea(
          child: Column(
            children: [
              Padding(
                padding: const EdgeInsets.all(16),
                child: TextField(
                  decoration: InputDecoration(
                    suffixIcon: const Icon(Icons.search),
                    hintText: AppLocalizations.of(context).searchMovie,
                  ),
                ),
              ),
              Expanded(
                child: ListView.builder(
                  // Providing a restorationId allows the ListView to restore the
                  // scroll position when a user leaves and returns to the app after it
                  // has been killed while running in the background.
                  restorationId: 'sampleItemListView',
                  itemCount: items.length,
                  itemBuilder: (BuildContext context, int index) {
                    final item = items[index];

                    return Entry.all(
                      delay: const Duration(milliseconds: 250),
                      child: GestureDetector(
                        onTap: () {
                          Navigator.restorablePushNamed(
                            context,
                            DetailsPage.routeName,
                          );
                        },
                        child: Container(
                          height: 168,
                          margin: const EdgeInsets.all(16),
                          child: Row(
                            children: [
                              ClipRRect(
                                borderRadius:
                                    const BorderRadius.all(Radius.circular(16)),
                                child: Hero(
                                  tag: item.id,
                                  child: CachedNetworkImage(
                                      imageUrl: item.posterUrl),
                                ),
                              ),
                              const SizedBox(width: 16),
                              Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Hero(
                                    tag: item.id + item.title,
                                    child: Text(
                                      item.title,
                                      style: const TextStyle(fontSize: 24),
                                    ),
                                  ),
                                  const SizedBox(height: 4),
                                  Text(item.releaseDate.year.toString()),
                                  Text(item.vote.toString())
                                ],
                              )
                            ],
                          ),
                        ),
                      ),
                    );
                  },
                ),
              ),
            ],
          ),
        ),
      );
}
