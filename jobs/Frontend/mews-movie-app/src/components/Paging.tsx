import { removeConsecutiveDuplicates } from "@/utilities/arrays";
import Link from "next/link";
import { useSearchParams } from "next/navigation";
import styled, { css } from "styled-components";
import { FiMoreHorizontal } from "react-icons/fi";

const showPageLink = (page: number, current: number, total: number): number => {
    if (total <= 7) {
        return page;
    }

    if (page === 1 || page === total) {
        return page;
    }

    if (Math.abs(page - current) <= 1) {
        return page;
    }

    return 0;
};

const StyledLink = styled(Link)<{ $active?: boolean }>`
    border-radius: 50%;
    width: 2.3rem;
    height: 2.3rem;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    text-decoration: none;
    color: #999;
    background-color: #fff;
    font-size: 0.8rem;

    &:focus-visible {
        outline: 3px solid var(--focus-color);
    }

    ${(props) =>
        props.$active &&
        css`
            background: var(--focus-color);
            color: white;
        `}
`;

const Filler = styled.div`
    width: 2.3rem;
    height: 2.3rem;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    color: #999;
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
                    <StyledLink
                        key={index}
                        $active={item === current}
                        href={`?${getSearchParamsForPage(
                            urlSearchParams,
                            item,
                        )}`}
                    >
                        {/* <Button key={item} $active={item === current}> */}
                        {item}
                        {/* </Button> */}
                    </StyledLink>
                ) : (
                    <Filler key={index}>
                        <FiMoreHorizontal />
                    </Filler>
                ),
            )}
        </Container>
    );
};

export default Paging;
