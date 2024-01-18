import type { AppProps } from 'next/app'
import { ReduxProvider } from '@/store/ReduxProvider';
import Head from 'next/head';
import GlobalStyle from '@/styles/Global';
import SiteBackground from '@/components/SiteBackground';

export default function App({ Component, pageProps }: AppProps) {
  return (
    <ReduxProvider>
      <Head>
        <meta name="description" content="Search Movies App" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />
      </Head>
      <GlobalStyle/>
      <SiteBackground p="24px" minHeight="100vh" position="relative" zIndex="1">
        <Component {...pageProps} />
      </SiteBackground>
    </ReduxProvider>
  )
}
