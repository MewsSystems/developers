import { ReactNode } from "react";
import type { Metadata } from "next";
import { notFound } from "next/navigation";
import { getTranslations } from "next-intl/server";
import { CONTENT_MAX_WIDTH, LOCALES } from "@/util/constants";
import { Providers } from "../_components/providers";

type Params = { locale: string };
type Props = { children: ReactNode; params: Params };

export async function generateMetadata({
  params: { locale },
}: {
  params: Params;
}): Promise<Metadata> {
  const t = await getTranslations("metadata");
  return {
    title: {
      template: t("template"),
      default: t("title"),
    },
    description: t("description"),
    metadataBase: new URL(process.env.WEB_URL!),
    manifest: "manifest.json",
    openGraph: {
      title: t("title"),
      description: t("description"),
      images: [{ url: "logo.png" }],
      locale,
      type: "website",
    },
    twitter: {
      card: "summary",
      title: t("title"),
      description: t("description"),
      images: [{ url: "logo.png" }],
    },
  };
}

export default function RootLayout({ children, params: { locale } }: Props) {
  if (!LOCALES.includes(locale)) {
    notFound();
  }

  return (
    <html lang={locale}>
      <body>
        <Providers locale={locale}>
          <main style={{ maxWidth: CONTENT_MAX_WIDTH, marginInline: "auto" }}>
            {children}
          </main>
        </Providers>
      </body>
    </html>
  );
}
