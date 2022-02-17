import 'package:flutter/material.dart';

class CustomAppBar extends AppBar {
  CustomAppBar({
    Key? key,
    required BuildContext context,
    required String title,
    bool displayBackButton = false,
  }) : super(
          leading: displayBackButton ? _buildLeading(context) : null,
          title: _buildTitle(context, title: title),
        );

  static Widget? _buildLeading(BuildContext context) {
    return IconButton(
      icon: Icon(
        Icons.arrow_back,
      ),
      onPressed: () => Navigator.pop(context),
    );
  }

  static Widget _buildTitle(BuildContext context, {required String title}) {
    return Text(
      title,
      style: Theme.of(context).textTheme.headline5!.copyWith(color: Colors.white),
    );
  }

  @override
  Size get preferredSize => const Size.fromHeight(48);
}
