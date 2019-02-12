import styled, {createGlobalStyle} from 'styled-components'

export const StyledContent = styled.div`
	min-height: 100vh;
	padding: 24px;
	display: flex;
	flex-direction: column;
`

export default createGlobalStyle`
	* {
		background-repeat: no-repeat;
		box-sizing: border-box;
		margin: 0px;
		padding: 0px;
	}
`