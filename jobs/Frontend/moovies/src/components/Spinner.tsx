import styled from 'styled-components'
import { keyframes } from 'styled-components'

const spin = keyframes`
    0% { transform: rotate(0deg); }
    100% { transform: rotate(360deg); }
`

const Spinner = styled.div`
    display: inline-block;
    width: 120px;
    height: 120px;
    
    &:after {
        content: " ";
        display: block;
        width: 104px;
        height: 104px;
        margin: 8px;
        border-radius: 50%;
        border: 16px solid #fff;
        border-color: #427aa1 transparent #427aa1 transparent;
        animation: ${spin} 2s linear infinite;
    }
`

export default Spinner
