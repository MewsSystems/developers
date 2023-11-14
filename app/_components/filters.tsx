import { ChangeEvent, FC } from "react";
import { usePathname, useRouter } from "@/navigation";
import { Button } from "@/styles/base/button";
import { Group } from "@/styles/base/group";
import { Stack } from "@/styles/base/stack";
import { Title } from "@/styles/base/title";
import { Input, Select } from "@/styles/components/filters";
import { generateLast100Years } from "@/util/date";
import { useTranslations } from "next-intl";
import { useSearchParams } from "next/navigation";
import { MovieType, TvType } from "@/domain/types/type";

export const SEARCH_KEY = "search";
export const YEAR_KEY = "year";

type Props = {
  onCategoryClick: (type: MovieType) => void;
};

export const Filters: FC<Props> = (props) => {
  const {
    t,
    onCategoryClick,
    handleOnChange,
    defaultSearchValue,
    defaultYearValue,
  } = useFilters(props);

  const renderYearOption = (year: number) => (
    <option key={year} value={year}>
      {year}
    </option>
  );

  return (
    <Stack $gap="md">
      <Title $ta="center">{t("title")}</Title>
      <Group $gap="sm">
        <Button onClick={() => onCategoryClick(MovieType.TopRated)}>
          {t("topRatedAction")}
        </Button>
        <Button onClick={() => onCategoryClick(MovieType.Popular)}>
          {t("popularAction")}
        </Button>
        <Button onClick={() => onCategoryClick(MovieType.Upcoming)}>
          {t("upcomingAction")}
        </Button>
      </Group>
      <Input
        type="text"
        placeholder={t("searchPlaceholder")}
        defaultValue={defaultSearchValue}
        onChange={(e) => handleOnChange(e, SEARCH_KEY)}
      />
      <Select
        id="year"
        defaultValue={defaultYearValue ?? ""}
        onChange={(e) => handleOnChange(e, YEAR_KEY)}
      >
        <option value="">{t("selectPlaceholder")}</option>
        {generateLast100Years().map(renderYearOption)}
      </Select>
    </Stack>
  );
};

function useFilters({ onCategoryClick }: Props) {
  const t = useTranslations("shared.filters");
  const searchParams = useSearchParams();
  const pathname = usePathname();
  const { replace } = useRouter();

  const handleOnChange = (
    e: ChangeEvent<HTMLInputElement | HTMLSelectElement>,
    key: string
  ) => {
    const value = e.target.value;
    const params = new URLSearchParams(searchParams);
    if (value) {
      params.set(key, value);
    } else {
      params.delete(key);
    }
    replace(`${pathname}?${params.toString()}`);
  };

  const defaultSearchValue = searchParams.get(SEARCH_KEY)?.toString();
  const defaultYearValue = searchParams.get(YEAR_KEY)?.toString();

  return {
    t,
    onCategoryClick,
    handleOnChange,
    defaultSearchValue,
    defaultYearValue,
  };
}
