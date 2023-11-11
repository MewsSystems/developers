import { FC, ReactNode } from "react";
import { NextIntlClientProvider, useMessages } from "next-intl";
import { getCurrentTimezone } from "@/util/date";
import StyledComponentsRegistry from "@/lib/registry";

type Props = {
  locale: string;
  children: ReactNode;
};

export const Providers: FC<Props> = ({ locale, children }) => {
  const messages = useMessages();

  return (
    <NextIntlClientProvider
      locale={locale}
      messages={messages}
      timeZone={getCurrentTimezone()}
    >
      <StyledComponentsRegistry>{children}</StyledComponentsRegistry>
    </NextIntlClientProvider>
  );
};
