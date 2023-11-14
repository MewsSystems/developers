import createMiddleware from "next-intl/middleware";
import { DEFAULT_LOCALE, LOCALES } from "./util/constants";

export default createMiddleware({
  locales: LOCALES,
  defaultLocale: DEFAULT_LOCALE,
  localePrefix: "as-needed",
});

export const config = {
  matcher: ["/((?!api|_next|.*\\..*).*)"],
};
