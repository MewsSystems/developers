/* eslint-disable @next/next/no-page-custom-font */
import { Html, Head, Main, NextScript } from "next/document";
import { withServerSideStyles } from "~/features/ui/lib/withStyledComponents";

const Document = () => (
  <Html>
    <Head>
      <link rel="preconnect" href="https://fonts.googleapis.com" />
      <link rel="preconnect" href="https://fonts.gstatic.com" crossOrigin="" />
      <link
        href="https://fonts.googleapis.com/css2?family=Roboto&display=swap"
        rel="stylesheet"
      />
    </Head>
    <body>
      <Main />
      <NextScript />
    </body>
  </Html>
);

export default withServerSideStyles(Document);
