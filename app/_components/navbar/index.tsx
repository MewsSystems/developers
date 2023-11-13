import { FC } from "react";
import { Navbar as StyledNavbar } from "@/styles/components/navbar";
import Image from "next/image";
import logo from "@/public/logo.png";
import { useTranslations } from "next-intl";
import { Group } from "@/styles/base/group";
import { paths } from "@/navigation/paths";
import { Link } from "../link";

export const Navbar: FC = () => {
  const { t } = useNavbar();

  return (
    <StyledNavbar>
      <Link href={paths.home()}>
        <Image src={logo} width={75} height={75} alt={t("logoAlt")} priority />
      </Link>
      <Group $gap="md">
        <Link href={paths.discovery()}>{t("link.discovery")}</Link>
        <Link href={paths.movies()}>{t("link.movies")}</Link>
        <Link href={paths.tvShows()}>{t("link.tvShows")}</Link>
      </Group>
    </StyledNavbar>
  );
};

function useNavbar() {
  const t = useTranslations("navbar");

  return { t };
}
