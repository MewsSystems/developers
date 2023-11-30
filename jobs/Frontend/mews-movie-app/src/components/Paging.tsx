import { removeConsecutiveDuplicates } from "@/utilities/arrays";
import Link from "next/link";
import { useSearchParams } from "next/navigation";
import styled, { css } from "styled-components";

const showPageLink = (page: number, current: number, total: number): number => {
    if (total <= 5) {
        return page;
    }
    
    if (page === 1 || page === total) {
        return page;
    }

    if (page === current - 1 || page === current || page === current + 1) {
        return page;
    }

    return 0;
};

const Button = styled.button<{ $active?: boolean }>`
    padding: 0.5rem;
    border: 1px solid #ccc;
    border-radius: 5px;

    &.active {
        background-color: #ccc;
    }

    ${(props) =>
        props.$active &&
        css`
            background: blue;
            color: white;
        `}
`;

const Container = styled.div`
    display: flex;
    gap: 0.5rem;
`;

type Props = {
    current: number;
    total: number;
};

const getSearchParamsForPage = (
    urlSearchParams: URLSearchParams,
    page: number,
) => {
    urlSearchParams.set("page", String(page));

    return urlSearchParams.toString();
};

const Paging = ({ current, total }: Props) => {
    const searchParams = useSearchParams();

    const urlSearchParams = new URLSearchParams(
        Array.from(searchParams.entries()),
    );

    const allPages = Array.from({ length: total }, (_, i) => i + 1);

    const visiblePages = allPages.map((page) =>
        showPageLink(page, current, total),
    );

    const pagesAndFillers = removeConsecutiveDuplicates(visiblePages);

    return (
        <Container>
            {pagesAndFillers.map((item, index) =>
                item ? (
                    <Link
                        key={index}
                        href={`?${getSearchParamsForPage(
                            urlSearchParams,
                            item,
                        )}`}
                    >
                        <Button key={item} $active={item === current}>
                            {item}
                        </Button>
                    </Link>
                ) : (
                    <div key={index}>...</div>
                ),
            )}
        </Container>
    );
};

export default Paging;
