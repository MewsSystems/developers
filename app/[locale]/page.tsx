import { useTranslations } from "next-intl";
import { Stack } from "@/styles/base/stack";
import { Box } from "@/styles/base/box";
import { Group } from "@/styles/base/group";
import { Text } from "@/styles/base/text";
import { Title } from "@/styles/base/title";

export default function Home() {
  const t = useTranslations();

  return (
    <Group $bg="primaryLight" $gap={32}>
      <div style={{ height: "100px", width: "100px", background: "red" }}>
        <Text>ovo je tekst</Text>
      </div>
      <div style={{ height: "100px", width: "100px", background: "blue" }}>
        <Title>Ovo je title</Title>
      </div>
    </Group>
  );
}
