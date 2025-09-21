import { Flex, Text } from "@chakra-ui/react";
import { Tabs } from "@chakra-ui/react";
import { useCallback, useState } from "react";
import { ReadRestLink } from "./ReadRestLink";
import type { ReviewDetails } from "@/features/social/types";

export function SocialTabs({
  reviews,
  totalReviews,
}: {
  reviews: ReviewDetails[];
  totalReviews: number;
}) {
  const [open, setOpen] = useState(false);

  const onClickHandler = useCallback(() => {
    setOpen(!open);
  }, []);

  return (
    <Tabs.Root defaultValue="reviews">
      <Tabs.List>
        <Tabs.Trigger value="reviews">Reviews({totalReviews})</Tabs.Trigger>
      </Tabs.List>
      <Tabs.Content value="reviews">
        <Flex>
          {reviews.map((review) => {
            return (
              <Text key={review.id} whiteSpace={"pre-line"}>
                {open ? review.fullContent : review.sliced_content}
                <ReadRestLink open={open} onClick={onClickHandler} />
              </Text>
            );
          })}
        </Flex>
      </Tabs.Content>
    </Tabs.Root>
  );
}
