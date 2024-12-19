import styled from "styled-components";
import Typography from "../Typography";
import { theme } from "../../theme";

const ErrorWrapper = styled.div`
	display: flex;
	flex-direction: column;
	justify-content: center;
	align-items: center;
	margin: ${({ theme }) => theme.spacing(3)} auto;
	padding: ${({ theme }) => theme.spacing(1)};
	background-color: ${theme.palette.error.light};
	color: ${theme.palette.error.dark};
	border: 1px solid ${theme.palette.error.dark};
	border-radius: ${theme.borderRadius.small};
`;

const ErrorMessage = styled.p`
	margin: 10px 0;
	text-align: center;
`;

interface ErrorProps {
	message: string;
}

const ErrorComponent = ({ message }: ErrorProps) => {
	return (
		<ErrorWrapper>
			<Typography variant="h4" color={theme.palette.error.dark}>
				Something went wrong!
			</Typography>
			<ErrorMessage>{message}</ErrorMessage>
		</ErrorWrapper>
	);
};

export default ErrorComponent;
