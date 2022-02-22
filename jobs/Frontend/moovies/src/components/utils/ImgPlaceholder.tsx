import styled from "styled-components";
interface Props {
    width: string
    height: string
}

const ImgPlaceholder = styled.div`
    width: ${(props: Props) => props.width};
    height: ${(props: Props) => props.height};
    background-color: #d8d8d8;
    text-align: center;
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: 0.95rem;
    color: #585858;
    
    & span {
        line-height: 1.3;
    }
`

export default ImgPlaceholder
