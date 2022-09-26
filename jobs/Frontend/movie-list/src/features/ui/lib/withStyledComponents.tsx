/* eslint-disable @next/next/no-document-import-in-page */
import type * as NextDocumentTypes from "next/document";
import NextDocument from "next/document";
import { ServerStyleSheet } from "styled-components";

type TDocument = React.ComponentType<NextDocumentTypes.DocumentProps> & {
  getInitialProps?: (
    ctx: NextDocumentTypes.DocumentContext
  ) => Promise<NextDocumentTypes.DocumentInitialProps>;
};

const withServerSideStyles = (document: TDocument) => {
  const getInitialProps =
    document.getInitialProps ?? NextDocument.getInitialProps;

  document.getInitialProps = async (ctx) => {
    const sheet = new ServerStyleSheet();
    const originalRenderPage = ctx.renderPage;

    try {
      ctx.renderPage = () =>
        originalRenderPage({
          enhanceApp: (App) => (props) =>
            sheet.collectStyles(<App {...props} />),
        });

      const initialProps = await getInitialProps(ctx);
      const styles = [initialProps.styles, sheet.getStyleElement()];

      return { ...initialProps, styles };
    } finally {
      sheet.seal();
    }
  };

  return document;
};

export { withServerSideStyles };
