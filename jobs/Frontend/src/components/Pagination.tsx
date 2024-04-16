import { useSearchParams } from 'react-router-dom';
import { UrlSearchParamKey } from '../enums/urlSearchParamKey.ts';
import { ButtonLink, PaginationRow, PaginationSeparator } from './Pagination.styled.tsx';

export interface PaginationModel {
    currentPage: number;
    numberOfPages: number;
}

interface PageLinkModel {
    highlighted: boolean;
    page: number;
    currentSearchParams: URLSearchParams;
}

export function Pagination({ currentPage, numberOfPages }: PaginationModel) {
    const [searchParams, _] = useSearchParams();
    const model = getPaginationModel(currentPage, numberOfPages);
    const pageLinks = model.map((item, i) => {
        const key = `${item}-${i}`;
        return (
            typeof item === 'number'
                ? <PageLink key={key} highlighted={item === currentPage} page={item} currentSearchParams={searchParams} />
                : <PaginationSeparator key={key}>{item}</PaginationSeparator>
        )
    });

    return (
        <PaginationRow>
            {pageLinks}
        </PaginationRow>
    );
}

function PageLink({ highlighted, page, currentSearchParams }: PageLinkModel) {
    const pageSearchParams = new URLSearchParams(currentSearchParams);

    if (page <= 1) {
        pageSearchParams.delete(UrlSearchParamKey.Page);
    } else {
        pageSearchParams.set(UrlSearchParamKey.Page, page.toString());
    }

    return <ButtonLink to={`?${pageSearchParams}`} className={highlighted && 'active'}>{ page }</ButtonLink>
}

const getPaginationModel = (currentPage: number, numberOfPages: number): (number | '...')[] => {
    const currentPageMatchesAnyOf = (...pageNumbers: number[]) => pageNumbers.some(page => currentPage === page);
    const getNumberArray = (from: number, count: number) =>
        Array.from(Array(count).keys(), (_, i) => from + i);

    if (numberOfPages < 6) {
        return getNumberArray(1, numberOfPages);
    }

    if (currentPageMatchesAnyOf(1, 2)) {
        return [...getNumberArray(1, 3), '...', numberOfPages - 1, numberOfPages];
    }

    if (currentPageMatchesAnyOf(numberOfPages, numberOfPages - 1)) {
        const lowerBound = numberOfPages - 2;
        return [1, 2, '...', ...getNumberArray(lowerBound, 3)];
    }

    if (currentPageMatchesAnyOf(3)) {
        return [...getNumberArray(1, 4), '...', numberOfPages];
    }

    if (currentPageMatchesAnyOf(numberOfPages - 2)) {
        const lowerBound = numberOfPages - 3;
        return [1, '...', ...getNumberArray(lowerBound, 4)];
    }

    return [1, '...', currentPage - 1, currentPage, currentPage + 1, '...', numberOfPages]
};