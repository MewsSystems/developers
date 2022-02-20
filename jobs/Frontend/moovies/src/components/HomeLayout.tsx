import styled from 'styled-components'

const StyledHomeLayout = styled.div`
    display: flex;
    flex-direction: column;
    align-items: center;
    min-height: 100vh;
`

const HomeLayout = ({ children }: any) => {

    return (
        <StyledHomeLayout>
            {children}
        </StyledHomeLayout>
    )
}

export default HomeLayout
