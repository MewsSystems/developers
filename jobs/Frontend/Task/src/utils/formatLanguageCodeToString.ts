import { NO_VALUE_PLACEHOLDER } from "../constants";

const languageNames = new Intl.DisplayNames(["en"], {
  type: "language"
});

export const formatLanguageCodeToString = (languageCode: string | undefined) => {
  if (!languageCode) {
    return NO_VALUE_PLACEHOLDER;
  }

  return languageNames.of(languageCode);
};
