import { MovieDetailPageProps } from '.'

export type TopSummaryProps = Pick<
    MovieDetailPageProps,
    'detailData' | 'isLoading'
>
