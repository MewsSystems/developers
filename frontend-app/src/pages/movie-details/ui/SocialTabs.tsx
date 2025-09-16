import { Flex, Text } from "@chakra-ui/react"
import { Tabs } from "@chakra-ui/react"
import { slice } from 'lodash';
import { useCallback, useState } from 'react';
import type { DetailsProps } from '@/pages/movie-details/types';
import { ReadRestLink } from "./ReadRestLink";

export function SocialTabs({ detailsProps }: { detailsProps: DetailsProps }) {
    const reviews = detailsProps.movie.reviews.results;
    const reviewsToShow = slice(reviews, 0, 1);
    const [open, setOpen] = useState(false);
    const MAX_TEXT = 100;

    const onClickHandler = useCallback(() => {
        setOpen(!open);
    }, []);

    return (<Tabs.Root defaultValue="reviews">
        <Tabs.List>
            <Tabs.Trigger value="reviews">
                Reviews({reviews.length})
            </Tabs.Trigger>
        </Tabs.List>
        <Tabs.Content value="reviews">
            <Flex>
                {reviewsToShow.map(review => {
                    return (
                        <Text key={review.id} whiteSpace={"pre-line"}>
                            {review.content.slice(0, open ? review.content.length : MAX_TEXT)}<ReadRestLink open={open} onClick={onClickHandler} /></Text>
                    )
                })}
            </Flex>
        </Tabs.Content>
    </Tabs.Root>)
}