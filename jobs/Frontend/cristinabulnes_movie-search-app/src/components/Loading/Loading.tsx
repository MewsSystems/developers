import styled from "styled-components";
import { theme } from "../../theme";

const LoaderContainer = styled.div`
	display: flex;
	flex-direction: column;
	justify-content: center;
	align-items: center;
	height: 100vh;
	width: 100%;
`;

const Spinner = styled.div`
	border: 4px solid rgba(255, 255, 255, 0.3);
	border-top: 4px solid ${theme.palette.primary.main};
	border-radius: 50%;
	width: 50px;
	height: 50px;
	animation: spin 1s linear infinite;

	@keyframes spin {
		0% {
			transform: rotate(0deg);
		}
		100% {
			transform: rotate(360deg);
		}
	}
`;

const MessageWrapper = styled.span`
	margin: ${theme.spacing(1)};
	fontsize: ${theme.typography.fontSize.large};
	color: ${theme.palette.text.secondary};
`;

interface LoadingProps {
	size?: string;
	message?: string;
}

const Loading = ({ size = theme.spacing(4), message }: LoadingProps) => {
	return (
		<LoaderContainer>
			<Spinner style={{ width: size, height: size }} />
			{message && <MessageWrapper>{message}</MessageWrapper>}
		</LoaderContainer>
	);
};

export default Loading;
