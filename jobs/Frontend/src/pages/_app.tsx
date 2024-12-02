import { AppProvider, useStore } from "@/context";
import { State } from "@/context/types";
import "@/styles/globals.css";
import { MovieSearchResult } from "@/types";
import type { AppProps } from "next/app";

export default function App({ Component, pageProps }: AppProps) {
  console.log(pageProps);
  let context: State | undefined = undefined;

  if (pageProps.searchTerm) {
    context = {
      searchTerm: pageProps.searchTerm,
      searchResult: pageProps.searchResult,
      page: pageProps.searchResult.page,
      searchInProgress: false,
      started: false,
    };
  }

  return (
    <AppProvider ssrState={context}>
      <Component {...pageProps} />
    </AppProvider>
  );
}
