export interface AlertMessagesProps {
    show?: boolean
    message?: string,
    onHide?: () => void
}

export interface AlertMessagesState {
    show: boolean;
}