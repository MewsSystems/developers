import type { AppProps } from "next/app";
import { Provider } from "react-redux";

import { store } from "~/app/store";
import { Layout } from "~/features/ui/components/Layout";
import { GlobalStyle } from "~/features/ui/theme/global";

function MyApp({ Component, pageProps }: AppProps) {
  return (
    <Provider store={store}>
      <GlobalStyle />
      <Layout>
        <Component {...pageProps} />
      </Layout>
    </Provider>
  );
}

export default MyApp;
