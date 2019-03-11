export enum MessageType {
    ERROR = 'error',
}

export interface MessageInterface {
    message: string;
    type: MessageType;
}
