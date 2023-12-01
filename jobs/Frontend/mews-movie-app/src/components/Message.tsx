import styled from "styled-components";

const Content = styled.div`
    text-align: center;
`;

type Props = {
    title: string;
    children: React.ReactNode;
};

const Message = ({ title, children }: Props) => {
    return (
        <Content>
            <h2>{title}</h2>
            <div>{children}</div>
        </Content>
    );
};

export default Message;
