import 'package:flutter/material.dart';
import 'package:mobile/presentation/core/theming/colors.dart';
import 'package:mobile/presentation/core/theming/size_config.dart';

class CustomBackButton extends StatelessWidget {
  const CustomBackButton({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return ClipOval(
      child: Container(
        color: CinephileColors.scaffoldDarkBack,
        child: InkWell(
          child: SizedBox(
              width: 60.w(),
              height: 60.h(),
              child: const Icon(
                Icons.arrow_back_ios,
                color: Colors.white,
              )),
          onTap: () {
            Navigator.pop(context);
          },
        ),
      ),
    );
  }
}
