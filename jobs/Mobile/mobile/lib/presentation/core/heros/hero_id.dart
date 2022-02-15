///Helper class used to autonate the creation of [Hero] id.
class HeroTag {
  static String make({required int id, required String title}) {
    return 'hero_${id}_$title';
  }
}
