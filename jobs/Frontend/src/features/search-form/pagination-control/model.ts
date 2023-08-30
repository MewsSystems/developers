export interface Props {
    readonly page: number;
    readonly totalPages: number;
    readonly onChange: (page: number) => void;
}