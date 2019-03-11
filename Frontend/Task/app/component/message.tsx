import React from 'react';
import { MessageInterface, MessageType } from '../interface/MessageInterface';
import MessageStyle from '../style/message';

const Message: React.FC<MessageInterface> = ({ message, type }) => (
    <MessageStyle error={type === MessageType.ERROR}>{message}</MessageStyle>
);

export default Message;
