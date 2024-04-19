import { Link, useSearchParams } from 'react-router-dom';
import { UrlSearchParamKey } from '../../../enums/urlSearchParamKey.ts';
import { PaginationRow, PaginationSeparator } from './Pagination.styled.tsx';
import { getPaginationModel } from './paginationUtils.ts';
import { Button } from '../Button.tsx';

interface PaginationProps {
    currentPage: number;
    numberOfPages: number;
}

interface PageLinkProps {
    highlighted: boolean;
    page: number;
    currentSearchParams: URLSearchParams;
}

export function Pagination({ currentPage, numberOfPages }: PaginationProps) {
    const [searchParams, _] = useSearchParams();
    const model = getPaginationModel(currentPage, numberOfPages);

    const pageLinks = model.map((item, i) => {
        const key = `${item}-${i}`;
        return (
            typeof item === 'number'
                ? <PageLink
                    key={key}
                    highlighted={item === currentPage}
                    page={item}
                    currentSearchParams={searchParams}
                />
                : <PaginationSeparator key={key}>{item}</PaginationSeparator>
        )
    });

    return (
        <PaginationRow>
            {pageLinks}
        </PaginationRow>
    );
}

function PageLink({ highlighted, page, currentSearchParams }: PageLinkProps) {
    const pageSearchParams = new URLSearchParams(currentSearchParams);

    if (page <= 1) {
        pageSearchParams.delete(UrlSearchParamKey.Page);
    } else {
        pageSearchParams.set(UrlSearchParamKey.Page, page.toString());
    }

    return (
        <Button
            as={Link}
            to={`?${pageSearchParams}`}
            className={highlighted ? 'active' : ''}
            aria-label={`Go to page ${page}`}
            onClick={() => { window.scrollTo({ top: 0, behavior: 'smooth' }) }}
        >
            { page }
        </Button>
    );
}