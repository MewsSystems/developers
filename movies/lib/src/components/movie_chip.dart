import 'package:flutter/material.dart';
import 'package:movies/theme.dart';

class MovieChip extends StatelessWidget {
  const MovieChip({Key? key, required this.label, this.icon, this.iconColor})
      : super(key: key);

  final String label;
  final IconData? icon;
  final Color? iconColor;
  @override
  Widget build(BuildContext context) => Chip(
        backgroundColor: chipColor,
        labelPadding: EdgeInsets.only(right: 8, left: icon == null ? 8 : 0),
        avatar: icon == null
            ? null
            : Icon(
                icon,
                size: 16,
                color: iconColor,
              ),
        label: Text(
          label,
          style: const TextStyle(color: Colors.white),
        ),
      );
}
