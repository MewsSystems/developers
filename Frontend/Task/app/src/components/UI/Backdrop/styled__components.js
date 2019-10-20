import styled from 'styled-components'

export const Backdrop = styled.div`
    
    width: 100%;
    height: 100%;
    position: fixed;
    z-index: 100;
    left: 0;
    top: 0;
    background-color: #ccccccba;
    
    @media (min-width: 500px) {
        display: none;
    }
`