import { Helmet } from "react-helmet";
import { useTranslation } from "react-i18next";
import { Main } from "src/components/Main/Main";

export default function Home() {
  const { t } = useTranslation("translation");
  return (
    <>
      <Helmet>
        <title>{t("title")}</title>
      </Helmet>
      <Main />
    </>
  );
}
